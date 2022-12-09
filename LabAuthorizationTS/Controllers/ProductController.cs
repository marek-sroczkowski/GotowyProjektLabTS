using LabAuthorizationTS.Authorization.Requirements;
using LabAuthorizationTS.Authorization.Utils;
using LabAuthorizationTS.Extensions;
using LabAuthorizationTS.Filters;
using LabAuthorizationTS.Models.Dtos.Products;
using LabAuthorizationTS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LabAuthorizationTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IAuthorizationService authorizationService;

        public ProductController(IProductService productService, IAuthorizationService authorizationService)
        {
            this.productService = productService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves all products except those with an age restriction")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsAsync()
        {
            var products = await productService.GetProductsAsync();

            return Ok(products);
        }

        [SwaggerOperation(Summary = "Retrieves all products including those with an age restriction")]
        [HttpGet("all")]
        //TODO endpoint dostępny tylko dla pełnoletnich użytkowników o dowolnej roli
        [Authorize("AtLeast18")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var products = await productService.GetAllProductsAsync();

            return Ok(products);
        }

        [SwaggerOperation(Summary = "Retrieves a specific product by unique id")]
        [HttpGet("{productId}")]
        [ValidateProductExistence]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(long productId)
        {
            var product = await productService.GetProductByIdAsync(productId);

            //TODO jeśli produkt tylko dla pełnoletnich, a mamy customera, który nie ma 18 lat to 403
            var authorizationResult = await authorizationService.AuthorizeAsync(User, product, new ResourceOperationRequirement(OperationType.Read));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            return Ok(product);
        }

        [SwaggerOperation(Summary = "Creates a new product")]
        [HttpPost]
        //TODO endpoint dostępny tylko dla użytkownika o roli Seller
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult> AddProductAsync([FromBody] NewProductDto newProduct)
        {
            var product = await productService.AddProductAsync(User.GetUserId(), newProduct);

            return product != null
                ? Created("api/Product/" + product.Id, null)
                : BadRequest("New product could not be added");
        }

        [SwaggerOperation(Summary = "Updates a existing product")]
        [HttpPut]
        [ValidateProductExistence]
        //TODO endpoint dostępny tylko dla użytkowników o roli Seller i Admin
        [Authorize(Roles = "Seller,Admin")]
        public async Task<ActionResult> UpdateProductAsync([FromBody] UpdatedProductDto updatedProduct)
        {
            //TODO Admin może edytować wszystko, Seller tylko swoje produkty
            var product = await productService.GetProductByIdAsync(updatedProduct.Id);
            var authorizationResult = await authorizationService.AuthorizeAsync(User, product, new ResourceOperationRequirement(OperationType.Update));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var updated = await productService.UpdateProductAsync(updatedProduct);

            return updated ? NoContent() : BadRequest("Product could not be updated");
        }

        [SwaggerOperation(Summary = "Removes a specific product")]
        [HttpDelete("{productId}")]
        [ValidateProductExistence]
        //TODO endpoint dostępny tylko dla użytkowników o roli Seller i Admin
        [Authorize(Roles = "Seller,Admin")]
        public async Task<ActionResult> RemoveProductAsync(long productId)
        {
            //TODO Admin może usuwać wszystko, Seller tylko swoje produkty
            var product = await productService.GetProductByIdAsync(productId);
            var authorizationResult = await authorizationService.AuthorizeAsync(User, product, new ResourceOperationRequirement(OperationType.Delete));
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var removed = await productService.RemoveProductAsync(productId);

            return removed ? NoContent() : BadRequest("Product could not be removed");
        }
    }
}