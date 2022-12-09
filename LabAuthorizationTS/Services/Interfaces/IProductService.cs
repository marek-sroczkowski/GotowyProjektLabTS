using LabAuthorizationTS.Models.Dtos.Products;

namespace LabAuthorizationTS.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddProductAsync(long sellerId, NewProductDto newProduct);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(long productId);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<bool> RemoveProductAsync(long productId);
        Task<bool> UpdateProductAsync(UpdatedProductDto updatedProduct);
    }
}