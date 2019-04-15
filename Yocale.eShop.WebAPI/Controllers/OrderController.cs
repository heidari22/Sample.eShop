using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities.OrderAggregate;
using Yocale.eShop.ApplicationCore.Interfaces;
using Yocale.eShop.Infrastructure.Identity;
using Yocale.eShop.Utility.Resource;
using Yocale.eShop.WebAPI.Controllers.BaseAPI;
using Yocale.eShop.WebAPI.Interfaces;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly ApplicationCore.Interfaces.IBasketService _basketService;
        private readonly Interfaces.IBasketService _basketViewModelService;
        private readonly IOrderService _orderService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private string _username = null;
        public OrderController(ApplicationCore.Interfaces.IBasketService basketService,
            WebAPI.Interfaces.IBasketService basketViewModelService,
            IProductService productService,
            IOrderService orderService,
            SignInManager<ApplicationUser> signInManage, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _basketService = basketService;
            _basketViewModelService = basketViewModelService;
            _signInManager = signInManage;
            _orderService = orderService;
        }
        public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

        [HttpPost("~/api/v1/addToBasket/{productId}")]
        [SwaggerOperation("AddToBasketAsync")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "This API add a product by Id to basket, finally it returns OrderId", type: typeof(int))]
        public async Task<IActionResult> AddToBasket(int productId)
        {
            try
            {
                await SetBasketModelAsync();

                var result = await _basketService.AddItemToBasket(BasketModel.Id, productId, 1);

                await SetBasketModelAsync();

                return ApiResult(result);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("add/{basketId}")]
        [SwaggerOperation("AddOrderAsync")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "This API create an order according the products which choosing in basket, finally it returns OrderId", type: typeof(int))]
        public async Task<IActionResult> AddOrder(int basketId, [FromBody]AddressViewModel addressViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (basketId <= 0)
                    {
                        return BadRequest(Shared.InvalidBasket);
                    }

                    await SetBasketModelAsync();

                    var result = await _orderService.CreateOrderAsync(basketId, new Address(addressViewModel.Street, addressViewModel.City, addressViewModel.State, addressViewModel.Country, addressViewModel.ZipCode));

                    if (result != null && ((result.Error != null && result.Error.Code == (int)HttpStatusCode.BadRequest) || (result.Data > 0)))
                        await _basketService.DeleteBasketAsync(BasketModel.Id);

                    return ApiResult(result);
                }
                catch
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }

            return BadRequest(Shared.InvalidBasket);
        }
        
        private async Task SetBasketModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                BasketModel = (await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name)).Data;
            }
            else
            {
                GetOrSetBasketCookieAndUserName();
                BasketModel = (await _basketViewModelService.GetOrCreateBasketForUser(_username)).Data;
            }
        }

        private void GetOrSetBasketCookieAndUserName()
        {
            if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            {
                _username = Request.Cookies[Constants.BASKET_COOKIENAME];
            }
            if (_username != null) return;

            _username = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true };
            cookieOptions.Expires = DateTime.Today.AddYears(10);
            Response.Cookies.Append(Constants.BASKET_COOKIENAME, _username, cookieOptions);
        }
    }
}