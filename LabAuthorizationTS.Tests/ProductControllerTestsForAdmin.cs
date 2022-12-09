using FluentAssertions;
using Flurl.Http;
using LabAuthorizationTS.Models.Dtos.Products;
using LabAuthorizationTS.Persistence;
using LabAuthorizationTS.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace LabAuthorizationTS.Tests
{
    public class ProductControllerTestsForAdmin
    {
        private readonly WebApplicationFactory<Program> factory;
        private const string baseUrl = "/api/Product/";
        private readonly IFlurlRequest request;

        public ProductControllerTestsForAdmin()
        {
            factory = new WebApplicationFactory<Program>()
                  .WithWebHostBuilder(builder =>
                  {
                      builder.ConfigureServices(services =>
                      {
                          services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluatorForAdmin>();
                          services.AddMvc(options => options.Filters.Add(new FakeAdminFilter()));
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
        public async Task GetAllProductsAsync_ForExistingProductsAndAdultAdmin_ReturnsOk()
        {
            var response = await request.AppendPathSegments("all").GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProductByIdAsync_ForExistingProductAndAdultAdmin_ReturnsOk()
        {
            var productId = GetExampleProductId();

            var response = await request.AppendPathSegments(productId).GetAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddProductAsync_ForAdmin_ReturnsForbidden()
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
        public async Task UpdateProductAsync_ForAdmin_ReturnsNoContent()
        {
            var product = new UpdatedProductDto
            {
                Id = GetExampleProductId(),
                Name = "Test",
                Price = 99,
                OnlyForAdults = false
            };

            var response = await request.PutJsonAsync(product);

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RemoveProductAsync_ForAdmin_ReturnsNoContent()
        {
            var productId = GetExampleProductId();

            var response = await request.AppendPathSegments(productId).DeleteAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        private long GetExampleProductId()
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            return dbContext.Products.First().Id;
        }
    }
}