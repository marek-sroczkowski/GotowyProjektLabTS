using FluentAssertions;
using Flurl.Http;
using LabAuthorizationTS.Models.Dtos.Products;
using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Persistence;
using LabAuthorizationTS.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace LabAuthorizationTS.Tests
{
    public class ProductControllerTestsForAdultConsumer
    {
        private readonly WebApplicationFactory<Program> factory;
        private const string baseUrl = "/api/Product/";
        private readonly IFlurlRequest request;

        public ProductControllerTestsForAdultConsumer()
        {
            factory = new WebApplicationFactory<Program>()
                  .WithWebHostBuilder(builder =>
                  {
                      builder.ConfigureServices(services =>
                      {
                          services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluatorForAdultCustomer>();
                          services.AddMvc(options => options.Filters.Add(new FakeAdultCustomerFilter()));
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
        public async Task GetAllProductsAsync_ForExistingProductsAndAdultCustomer_ReturnsOk()
        {
            var response = await request.AppendPathSegments("all").GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProductByIdAsync_ForExistingNormalProductAndAdultCustomer_ReturnsOk()
        {
            var productId = GetExampleProductId();

            var response = await request.AppendPathSegments(productId).GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProductByIdAsync_ForExistingProductForAdultsAndAdultCustomer_ReturnsOk()
        {
            var productId = GetExampleProductForAdultId();

            var response = await request.AppendPathSegments(productId).GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddProductAsync_ForCustomer_ReturnsForbidden()
        {
            var product = new NewProductDto
            {
                Name = "Test",
                Price = 99,
                OnlyForAdults = false
            };

            var response = await request.PostJsonAsync(product);

            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateProductAsync_ForCustomer_ReturnsForbidden()
        {
            var exampleId = GetExampleProductId();
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
        public async Task RemoveProductAsync_ForCustomer_ReturnsForbidden()
        {
            var productId = GetExampleProductId();

            var response = await request.AppendPathSegments(productId).DeleteAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        private long GetExampleProductId()
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            var product = new Product
            {
                Name = "Test",
                Price = 99,
                OnlyForAdults = false,
                UserId = 4
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            return product.Id;
        }

        private long GetExampleProductForAdultId()
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            var product = new Product
            {
                Name = "Test",
                Price = 99,
                OnlyForAdults = true,
                UserId = 4
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            return product.Id;
        }
    }
}