using System.Net;
using System.Net.Http.Json;
using Catalog.IntegrationTests.Collections;
using Catalog.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace Catalog.IntegrationTests.TestCases.Products;

[Collection("Catalog")]
public class ProductTests
{
    private readonly HttpClient _client;
    private readonly DatabaseResetter _databaseResetter;

    public ProductTests(
        CatalogTestFixture fixture)
    {
        _client = fixture.Client;
        _databaseResetter =
            new DatabaseResetter(fixture);
    }


    // =========================================================
    // Create Product
    // =========================================================

    [Fact]
    public async Task CreateProduct_ShouldCreateProduct()
    {
        // Arrange
        var request = CreateProductPayload();

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/products",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        var productId =
            await response.Content
                .ReadFromJsonAsync<Guid>();

        productId
            .Should()
            .NotBeEmpty();

        // Verify that the product was actually persisted.
        var getResponse =
            await _client.GetAsync(
                $"/api/products/{productId}");

        getResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var product =
            await getResponse.Content
                .ReadFromJsonAsync<ProductResponseDto>();

        product.Should().NotBeNull();

        product!.Id
            .Should()
            .Be(productId);

        product.Name
            .Should()
            .Be(request.Name);

        product.Description
            .Should()
            .Be(request.Description);

        product.Price
            .Should()
            .Be(request.Price);
    }


    [Fact]
    public async Task CreateProduct_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request =
            CreateProductPayload(
                name: "",
                description: "",
                price: -10);

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/products",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }


    // =========================================================
    // Get Products
    // =========================================================

    [Fact]
    public async Task GetProducts_ShouldReturnRequestedPageSize()
    {
        // Arrange
        for (var i = 1; i <= 15; i++)
        {
            var request =
                CreateProductPayload(
                    name: $"Pagination Product {i}");

            var createResponse =
                await _client.PostAsJsonAsync(
                    "/api/products",
                    request);

            createResponse.EnsureSuccessStatusCode();
        }

        // Act
        var response =
            await _client.GetAsync(
                "/api/products?page=1&pageSize=10");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<PagedResultDto>();

        result.Should().NotBeNull();

        result!.Items
            .Should()
            .HaveCount(10);

        result.Page
            .Should()
            .Be(1);

        result.PageSize
            .Should()
            .Be(10);

        result.TotalCount
            .Should()
            .BeGreaterThanOrEqualTo(15);
    }

    // =========================================================
    // Get Products Second page
    // =========================================================

    [Fact]
    public async Task GetProducts_ShouldReturnSecondPage()
    {
        // Arrange
        await _databaseResetter.ResetProductsAsync();

        for (var i = 1; i <= 15; i++)
        {
            var request =
                CreateProductPayload(
                    name: $"Page Test Product {i}");

            var createResponse =
                await _client.PostAsJsonAsync(
                    "/api/products",
                    request);

            createResponse.EnsureSuccessStatusCode();
        }

        // Act
        var response =
            await _client.GetAsync(
                "/api/products?page=2&pageSize=10");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<PagedResultDto>();

        result.Should().NotBeNull();

        result!.Page
            .Should()
            .Be(2);

        result.PageSize
            .Should()
            .Be(10);

        result.Items
            .Should()
            .HaveCount(5);
    }

    // =========================================================
    // Get Products Search
    // =========================================================

    [Fact]
    public async Task GetProducts_ShouldFilterBySearch()
    {
        // Arrange
        await _client.PostAsJsonAsync(
            "/api/products",
            CreateProductPayload(
                name: "iPhone 17"));

        await _client.PostAsJsonAsync(
            "/api/products",
            CreateProductPayload(
                name: "Samsung Galaxy"));

        // Act
        var response =
            await _client.GetAsync(
                "/api/products?search=iPhone");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<PagedResultDto>();

        result.Should().NotBeNull();

        result!.Items
            .Should()
            .ContainSingle();

        result.Items[0].Name
            .Should()
            .Be("iPhone 17");
    }

    // =========================================================
    // Get Products Minimum Price Test
    // =========================================================

    [Fact]
    public async Task GetProducts_ShouldFilterByMinimumPrice()
    {
        // Arrange
        await _client.PostAsJsonAsync(
            "/api/products",
            CreateProductPayload(
                name: "Cheap Product",
                price: 50));

        await _client.PostAsJsonAsync(
            "/api/products",
            CreateProductPayload(
                name: "Expensive Product",
                price: 500));

        // Act
        var response =
            await _client.GetAsync(
                "/api/products?minPrice=100");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<PagedResultDto>();

        result.Should().NotBeNull();

        result!.Items
            .Should()
            .OnlyContain(
                x => x.Price >= 100);
    }


    // =========================================================
    // Get Products Combined Filter
    // =========================================================

    [Fact]
    public async Task GetProducts_ShouldApplyMultipleFilters()
    {
        // Arrange
        await _client.PostAsJsonAsync(
            "/api/products",
            CreateProductPayload(
                name: "Gaming Laptop",
                description: "Gaming device",
                price: 1500));

        await _client.PostAsJsonAsync(
            "/api/products",
            CreateProductPayload(
                name: "Office Laptop",
                description: "Office device",
                price: 800));

        await _client.PostAsJsonAsync(
            "/api/products",
            CreateProductPayload(
                name: "Gaming Mouse",
                description: "Gaming accessory",
                price: 100));

        // Act
        var response =
            await _client.GetAsync(
                "/api/products" +
                "?search=Gaming" +
                "&minPrice=500");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<PagedResultDto>();

        result.Should().NotBeNull();

        result!.Items
            .Should()
            .ContainSingle();

        result.Items[0].Name
            .Should()
            .Be("Gaming Laptop");
    }

    // =========================================================
    // Get Products Validation Tests
    // =========================================================

    [Fact]
    public async Task GetProducts_ShouldReturnBadRequest_WhenPageIsInvalid()
    {
        var response =
            await _client.GetAsync(
                "/api/products?page=0&pageSize=10");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnBadRequest_WhenPageSizeIsTooLarge()
    {
        var response =
            await _client.GetAsync(
                "/api/products?page=1&pageSize=101");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnBadRequest_WhenPriceRangeIsInvalid()
    {
        var response =
            await _client.GetAsync(
                "/api/products?minPrice=500&maxPrice=100");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }

    // =========================================================
    // Get Product By Id
    // =========================================================

    [Fact]
    public async Task GetProductById_ShouldReturnProduct()
    {
        // Arrange
        var request =
            CreateProductPayload(
                name: "Get By Id Test");

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/products",
                request);

        createResponse.EnsureSuccessStatusCode();

        var createdProductId =
            await createResponse.Content
                .ReadFromJsonAsync<Guid>();

        createdProductId
            .Should()
            .NotBeEmpty();

        // Act
        var response =
            await _client.GetAsync(
                $"/api/products/{createdProductId}");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var product =
            await response.Content
                .ReadFromJsonAsync<ProductResponseDto>();

        product.Should().NotBeNull();

        product!.Id
            .Should()
            .Be(createdProductId);

        product.Name
            .Should()
            .Be(request.Name);

        product.Description
            .Should()
            .Be(request.Description);

        product.Price
            .Should()
            .Be(request.Price);
    }


    [Fact]
    public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response =
            await _client.GetAsync(
                $"/api/products/{id}");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }


    // =========================================================
    // Update Product
    // =========================================================

    [Fact]
    public async Task UpdateProduct_ShouldUpdateProduct()
    {
        // Arrange
        var createRequest =
            CreateProductPayload(
                name: "Original Product",
                description: "Original Description",
                price: 100,
                stock: 10);

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/products",
                createRequest);

        createResponse.EnsureSuccessStatusCode();

        var createdProductId =
            await createResponse.Content
                .ReadFromJsonAsync<Guid>();

        createdProductId
            .Should()
            .NotBeEmpty();

        var updateRequest = new
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 250
        };

        // Act
        var updateResponse =
            await _client.PutAsJsonAsync(
                $"/api/products/{createdProductId}",
                updateRequest);

        // Assert
        updateResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        var getResponse =
            await _client.GetAsync(
                $"/api/products/{createdProductId}");

        getResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var updatedProduct =
            await getResponse.Content
                .ReadFromJsonAsync<ProductResponseDto>();

        updatedProduct.Should().NotBeNull();

        updatedProduct!.Id
            .Should()
            .Be(createdProductId);

        updatedProduct.Name
            .Should()
            .Be(updateRequest.Name);

        updatedProduct.Description
            .Should()
            .Be(updateRequest.Description);

        updatedProduct.Price
            .Should()
            .Be(updateRequest.Price);
    }


    [Fact]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var request = new
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 200
        };

        // Act
        var response =
            await _client.PutAsJsonAsync(
                $"/api/products/{id}",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var id = Guid.NewGuid();

        var request = new
        {
            Name = "",
            Description = "",
            Price = -10
        };

        // Act
        var response =
            await _client.PutAsJsonAsync(
                $"/api/products/{id}",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }


    // =========================================================
    // Delete Product
    // =========================================================

    [Fact]
    public async Task DeleteProduct_ShouldDeleteProduct()
    {
        // Arrange
        var request =
            CreateProductPayload(
                name: "Delete Test Product");

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/products",
                request);

        createResponse.EnsureSuccessStatusCode();

        var createdProductId =
            await createResponse.Content
                .ReadFromJsonAsync<Guid>();

        createdProductId
            .Should()
            .NotBeEmpty();

        // Act
        var deleteResponse =
            await _client.DeleteAsync(
                $"/api/products/{createdProductId}");

        // Assert
        deleteResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        var getResponse =
            await _client.GetAsync(
                $"/api/products/{createdProductId}");

        getResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response =
            await _client.DeleteAsync(
                $"/api/products/{id}");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProducts_ShouldSortByPriceAscending()
    {
        await _databaseResetter.ResetProductsAsync();

        foreach (var item in new[]
        {
        CreateProductPayload(name: "Expensive", price: 300),
        CreateProductPayload(name: "Cheap", price: 50),
        CreateProductPayload(name: "Medium", price: 150)
    })
        {
            var createResponse =
                await _client.PostAsJsonAsync(
                    "/api/products",
                    item);

            createResponse.EnsureSuccessStatusCode();
        }

        var response =
            await _client.GetAsync(
                "/api/products?sortBy=price&sortDirection=asc");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result =
            await response.Content.ReadFromJsonAsync<PagedResultDto>();

        result.Should().NotBeNull();

        result!.Items
            .Select(x => x.Price)
            .Should()
            .BeInAscendingOrder();
    }

    [Fact]
    public async Task GetProducts_ShouldSortByNameDescending()
    {
        await _databaseResetter.ResetProductsAsync();

        foreach (var name in new[]
        {
        "Apple",
        "Samsung",
        "Google"
    })
        {
            var createResponse =
                await _client.PostAsJsonAsync(
                    "/api/products",
                    CreateProductPayload(name: name));

            createResponse.EnsureSuccessStatusCode();
        }

        var response =
            await _client.GetAsync(
                "/api/products?sortBy=name&sortDirection=desc");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result =
            await response.Content.ReadFromJsonAsync<PagedResultDto>();

        result.Should().NotBeNull();

        result!.Items
            .Select(x => x.Name)
            .Should()
            .BeInDescendingOrder();
    }

    [Theory]
    [InlineData("unknown", "asc")]
    [InlineData("price", "random")]
    public async Task GetProducts_ShouldReturnBadRequest_WhenSortParametersAreInvalid(
    string sortBy,
    string sortDirection)
    {
        var response =
            await _client.GetAsync(
                $"/api/products?sortBy={sortBy}&sortDirection={sortDirection}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    // =========================================================
    // Test Helpers
    // =========================================================

    private static CreateProductRequestDto CreateProductPayload(
        string name = "Integration Test Product",
        string description = "Created by integration test",
        decimal price = 100,
        int stock = 10)
    {
        return new CreateProductRequestDto(
            name,
            description,
            price,
            stock);
    }


    // =========================================================
    // Test DTOs
    // =========================================================

    private sealed record CreateProductRequestDto(
        string Name,
        string Description,
        decimal Price,
        int Stock);


    private sealed record ProductResponseDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price);

    private sealed record PagedResultDto(
        List<ProductResponseDto> Items,
        int Page,
        int PageSize,
        int TotalCount,
        int TotalPages);
}