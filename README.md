# CloudCommerce

CloudCommerce is a learning project for building a cloud-oriented commerce backend with .NET. The repository currently contains the Catalog service; Basket, Ordering, Identity, and Notification are planned services, not implemented projects.

## Current solution

```text
Cloud Commerce Solution.sln
├── src/Services/Catalog/
│   ├── Catalog.API/             # HTTP endpoints, middleware, hosting
│   ├── Catalog.Application/     # CQRS requests, handlers, validation, contracts
│   ├── Catalog.Domain/          # Product domain entity
│   └── Catalog.Infrastructure/  # EF Core, PostgreSQL, repository, migrations
└── Catalog.IntegrationTests/    # HTTP integration tests with PostgreSQL Testcontainers
```

All projects target **.NET 10**. The repository's `global.json` selects SDK `10.0.401`.

## Architecture and request flow

The Catalog service uses Clean Architecture boundaries with CQRS and MediatR:

```text
HTTP request
  → Minimal API endpoint
  → MediatR request and pipeline behaviors (logging, validation)
  → Application handler and repository contract
  → Infrastructure repository and EF Core DbContext
  → PostgreSQL
```

The Domain project contains `Product`. Application defines commands, queries, handlers, validators, response models, and `IProductRepository`. Infrastructure implements persistence and database configuration. The API composes these layers and exposes the HTTP surface.

## Implemented Catalog features

- Product create, get-by-ID, update, and delete endpoints.
- Product listing with page/page size, substring search over name and description, minimum/maximum price filters, and allowlisted sorting by name, price, or ID.
- Validation for product requests and list query parameters.
- Centralized Problem Details responses for validation errors, missing products, and unexpected errors.
- PostgreSQL persistence through EF Core and Npgsql, with initial and product-index migrations.
- Composite indexes on `(Name, Id)` and `(Price, Id)` to support relevant ordering patterns. These B-tree indexes are not expected to accelerate leading-wildcard substring searches by themselves.
- Request logging, correlation IDs, Swagger in Development, liveness/readiness health endpoints, and ASP.NET Core/HTTP client tracing instrumentation.
- Integration tests using xUnit, `WebApplicationFactory<Program>`, and a PostgreSQL 18 Testcontainer. The fixture applies EF Core migrations to its test database.

The general product response omits `Stock`; product update also leaves stock unchanged. Optimistic concurrency handling is not currently configured.

## Prerequisites

- .NET SDK `10.0.401` (as selected by `global.json`).
- PostgreSQL for running the API directly from Visual Studio, or Docker Desktop for the Compose setup and Testcontainers-based integration tests.

## Run locally

For direct Visual Studio launch, start PostgreSQL with the development connection settings expected by `Catalog.API` (`localhost:5433`), then set `Catalog.API` as the startup project. The HTTP launch profile uses `http://localhost:5256`; Swagger is available at `/swagger` in Development.

The API does not apply EF Core migrations automatically at startup. Apply the Catalog migrations to the selected database before using database-backed endpoints. The integration-test fixture does this automatically for its own temporary database.

## Docker Compose

`docker-compose.yml` defines PostgreSQL 18 and the Catalog API. The database is published on host port `5433`; the API is published on host port `5001` and connects to PostgreSQL through the Compose service name `catalog-db` on container port `5432`. The API image uses .NET 10 SDK and ASP.NET runtime images, matching the projects' `net10.0` target.

Compose currently uses basic `depends_on` ordering. It does not wait for PostgreSQL readiness, and the API does not run migrations on startup. For a fresh database, apply migrations before exercising database-backed API requests. The Compose credentials are development-only and must be replaced with an appropriate secrets/configuration approach before any shared or production deployment.

## Current status and next milestones

### Implemented

- Catalog API with CRUD, validation, pagination, filtering, sorting, and PostgreSQL persistence.
- EF Core migrations and composite indexes for selected sort orders.
- Docker Compose development topology for Catalog API and PostgreSQL.
- Catalog integration-test infrastructure with PostgreSQL Testcontainers.
- Basic request logging, correlation IDs, health checks, and tracing instrumentation.

### Planned

1. Review remaining Catalog operational gaps, including migration/startup workflow and Docker Compose readiness.
2. Build Basket with Redis persistence and an explicit service boundary.
3. Add asynchronous messaging when service interactions justify it.
4. Expand observability and configure exporters/backends.
5. Add an Angular client and consider cloud deployment.

No GitHub Actions workflow is currently present in the repository. RabbitMQ, Redis, the other commerce services, and Azure deployment are not implemented yet.
