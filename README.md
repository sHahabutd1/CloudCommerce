# CloudCommerce

CloudCommerce is a cloud-native e-commerce platform built with modern .NET technologies and microservices architecture.

The goal of this project is to demonstrate enterprise-level backend engineering practices including distributed systems, clean architecture, asynchronous communication, and cloud deployment.

---

## Architecture

The system follows Microservices Architecture.

Services:

- Catalog Service
- Basket Service
- Ordering Service
- Identity Service
- Notification Service

Each service is independently developed, deployed, and scaled.

---

## Technology Stack

### Backend

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- Minimal APIs

### Architecture

- Clean Architecture
- Domain Driven Design (DDD)
- CQRS Pattern
- Mediator Pattern

### Database

- PostgreSQL
- Redis

### Messaging

- RabbitMQ
- Event Driven Communication

### DevOps

- Docker
- Docker Compose
- GitHub Actions
- Azure Container Apps

### Monitoring

- OpenTelemetry
- Application Insights

---

## Current Progress

- [x] Solution structure
- [x] Catalog Service created
- [x] Clean Architecture setup
- [ ] PostgreSQL integration
- [ ] CQRS implementation
- [ ] Docker Compose
- [ ] Azure Deployment


---

## Author

Developed by Shahab Tolouee
Senior .NET Full Stack Engineer


--

## Development Ports

Catalog PostgreSQL : localhost:5433
Redis              : localhost:6379
RabbitMQ           : localhost:5672
