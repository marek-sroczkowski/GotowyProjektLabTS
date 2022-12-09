using FluentAssertions;
using Flurl.Http;
using LabAuthorizationTS.Models.Dtos.Products;
using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Persistence;
using LabAuthorizationTS.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace LabAuthorizationTS.Tests
{
    public class ProductControllerTestsForSeller
    {
        private readonly WebApplicationFactory<Program> factory;
        private const string baseUrl = "/api/Product/";
        private readonly IFlurlRequest request;

        public ProductControllerTestsForSeller()
        {
            factory = new WebApplicationFactory<Program>()
                  .WithWebHostBuilder(builder =>
                  {
                      builder.ConfigureServices(services =>
                      {
                          services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluatorForSeller>();
                          services.AddMvc(options => options.Filters.Add(new FakeSellerFilter()));
                      });
                  });

            var flurlClient = new FlurlClient(factory.CreateClient());
            request = baseUrl.WithClient(flurlClient).AllowAnyHttpStatus();
        }

        [Fact]
        public async Task GetProductsAsync_ForExistingProducts_ReturnsOk()
        {
            var response = await request.GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAllProductsAsync_ForExistingProductsAndAdultSeller_ReturnsOk()
        {
            var response = await request.AppendPathSegments("all").GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProductByIdAsync_ForExistingProductAndAdultSeller_ReturnsOk()
        {
            var productId = GetExampleProductId();

            var response = await request.AppendPathSegments(productId).GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddProductAsync_ForSeller_ReturnsCreated()
        {
            var product = new NewProductDto
            {
                Name = "Test",
                Price = 99,
                OnlyForAdults = false
            };

            var response = await request.PostJsonAsync(product);

            response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async Task UpdateProductAsync_ForOwnSellerProduct_ReturnsNoContent()
        {
            var exampleId = SeedExampleProduct();
            var product = new UpdatedProductDto
            {
                Id = exampleId,
                Name = "Test2",
                Price = 100,
                OnlyForAdults = false
            };

            var response = await request.PutJsonAsync(product);

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateProductAsync_ForForeignSellerProduct_ReturnsForbidden()
        {
            var exampleId = SeedExampleProduct(5);
            var product = new UpdatedProductDto
            {
                Id = exampleId,
                Name = "Test2",
                Price = 100,
                OnlyForAdults = false
            };

            var response = await request.PutJsonAsync(product);

            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task RemoveProductAsync_ForOwnSellerProduct_ReturnsNoContent()
        {
            var productId = SeedExampleProduct();

            var response = await request.AppendPathSegments(productId).DeleteAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RemoveProductAsync_ForForeignSellerProduct_ReturnsForbidden()
        {
            var productId = SeedExampleProduct(5);

            var response = await request.AppendPathSegments(productId).DeleteAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        public long SeedExampleProduct(long userId = 4)
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            var product = new Product
            {
                Name = "Test",
                Price = 99,
                OnlyForAdults = false,
                UserId = userId
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            return product.Id;
        }

        private long GetExampleProductId()
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            return dbContext.Products.Include(p => p.User).First(p => p.User.Id == 4).Id;
        }
    }
}