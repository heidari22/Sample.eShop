using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities.BasketAggregate;
using Yocale.eShop.ApplicationCore.Exceptions;
using Yocale.eShop.ApplicationCore.Interfaces;
using Yocale.eShop.Resource.Errors;
using Yocale.eShop.Utility.Data;

namespace Yocale.eShop.ApplicationCore.Services
{
    public class BasketService : IBasketService
    {
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<BasketItem> _basketItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAppLogger<BasketService> _logger;

        public BasketService(IAsyncRepository<Basket> basketRepository,
             IProductRepository productRepository,
            IAppLogger<BasketService> logger,
            IAsyncRepository<BasketItem> basketItemRepository)
        {
            _basketRepository = basketRepository;
            _logger = logger;
            _basketItemRepository = basketItemRepository;
            _productRepository = productRepository;
        }

        public async Task<ResultModel<int>> AddItemToBasket(int basketId, int productItemId, int quantity)
        {
            try
            {
                var basket = await _basketRepository.GetByIdAsync(basketId);

                if (basket == null)
                    return ResultModel<int>.Create(new NotFoundError());

                var currentQuntityInBasket = basket.AddItem(productItemId, quantity);

                var productItem = await _productRepository.GetByIdAsync(productItemId);
                Guard.Against.NullProduct(productItemId, productItem);
                Guard.Against.ProductUnavailable(currentQuntityInBasket, productItem);

                await _basketRepository.UpdateAsync(basket);

                return basket.Id.ToResultModel();
            }
            catch(Exception ex)
            {
                if (ex.GetType().FullName ==
                       "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return ResultModel<int>.Create(new NotFoundError());
                }

                if(ex.Message.Contains($"No product found with id {basketId}"))
                    return ResultModel<int>.Create(new NotFoundError() { Message = $"No product found with basketId {basketId}" });

                if (ex.Message.Contains($"Unavailable product with "))
                    return ResultModel<int>.Create(new BadRequestError() { Message = $"Unavailable product with basketId {basketId}" });

                return ResultModel<int>.Create(new InternalServerError());
            }
        }

        public async Task<ResultModel<bool>> DeleteBasketAsync(int basketId)
        {
            try
            {
                var basket = await _basketRepository.GetByIdAsync(basketId);

                if (basket == null)
                    return ResultModel<bool>.Create(new NotFoundError());

                foreach (var item in basket.Items.ToList())
                {
                    await _basketItemRepository.DeleteAsync(item);
                }

                await _basketRepository.DeleteAsync(basket);
                return true.ToResultModel();
            }
            catch (Exception ex)
            {
                if (ex.GetType().FullName ==
                       "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return ResultModel<bool>.Create(new NotFoundError());
                }
                return ResultModel<bool>.Create(new InternalServerError());
            }

        }
        
    }
}
