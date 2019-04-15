using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using Yocale.eShop.Utility.Data;
using Yocale.eShop.WebAPI.Controllers.BaseAPI;
using Yocale.eShop.WebAPI.Interfaces;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _productService = productService;
        }
        
        [HttpGet("[action]")]
        [SwaggerOperation("GetAllProductsAsync")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "", type: typeof(ProductIndexViewModel))]
        public async Task<IActionResult> List(int? supplierId, int? categoryId, int? page)
        {
            try
            {
                var result = await _productService.GetProductList(--page ?? 0, Constants.ITEMS_PER_PAGE, supplierId, categoryId);

                return ApiResult(result);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation("GetProductByIdAsync")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "", type: typeof(ProductDetailViewModel))]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _productService.GetById(id);

                return ApiResult(result);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("add")]
        [SwaggerOperation("AddProductAsync")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "", type: typeof(ProductDetailViewModel))]
        public async Task<IActionResult> AddProduct([FromBody]ProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _productService.AddProductWithSKU(model.CategoryId, model.SupplierId, model.Name, model.Quantity, model.Description);

                    return ApiResult(result);
                }

                return BadRequest();
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("update/{id}")]
        [SwaggerOperation("UpdateProductAsync")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _productService.UpdateProduct(id, model.CategoryId, model.SupplierId, model.Name, model.Quantity, model.Description);

                    return ApiResult(result);
                }
                catch
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [SwaggerOperation("DeleteProductAsync")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _productService.DeleteProduct(id);
                
                    return ApiResult(result);
                }
                catch 
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }

            return BadRequest();
        }
    }
}