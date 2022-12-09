using LabAuthorizationTS.Models.Dtos.Products;
using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LabAuthorizationTS.Filters
{
    public class ValidateProductExistenceAttribute : TypeFilterAttribute
    {
        public ValidateProductExistenceAttribute() : base(typeof(ValidateProductExistenceFilterImpl))
        {
        }

        private class ValidateProductExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IRepository<Product> productRepository;

            public ValidateProductExistenceFilterImpl(IRepository<Product> productRepository)
            {
                this.productRepository = productRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("productId"))
                {
                    var productId = context.ActionArguments["productId"] as long?;
                    if (productId.HasValue)
                    {
                        if (!await productRepository.ContainsAsync(o => o.Id == productId.Value))
                        {
                            context.Result = new NotFoundObjectResult(productId.Value);
                            return;
                        }
                    }
                }
                else if (context.ActionArguments.ContainsKey("updatedProduct"))
                {
                    if (context.ActionArguments["updatedProduct"] is UpdatedProductDto product)
                    {
                        if (!await productRepository.ContainsAsync(o => o.Id == product.Id))
                        {
                            context.Result = new NotFoundObjectResult(product.Id);
                            return;
                        }
                    }
                }

                await next();
            }
        }
    }
}