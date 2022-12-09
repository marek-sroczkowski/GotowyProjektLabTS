using AutoMapper;
using LabAuthorizationTS.Models.Dtos.Products;
using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Repositories.Interfaces;
using LabAuthorizationTS.Services.Interfaces;

namespace LabAuthorizationTS.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> productRepository;
        private readonly IMapper mapper;

        public ProductService(IRepository<Product> productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await productRepository.FindAllAsync();

            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await productRepository.FindAllAsync(p => !p.OnlyForAdults);

            return mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(long productId)
        {
            var product = await productRepository.FindByIdAsync(productId);

            return mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddProductAsync(long sellerId, NewProductDto newProduct)
        {
            var product = mapper.Map<Product>(newProduct);
            product.UserId = sellerId;
            var created = await productRepository.AddAndSaveAsync(product);

            return created ? mapper.Map<ProductDto>(product) : null;
        }

        public async Task<bool> UpdateProductAsync(UpdatedProductDto updatedProduct)
        {
            var product = await productRepository.FindByIdAsync(updatedProduct.Id);
            mapper.Map(updatedProduct, product);

            return await productRepository.UpdateAndSaveAsync(product);
        }

        public async Task<bool> RemoveProductAsync(long productId)
        {
            var product = await productRepository.FindByIdAsync(productId);

            return await productRepository.RemoveAndSaveAsync(product);
        }
    }
}