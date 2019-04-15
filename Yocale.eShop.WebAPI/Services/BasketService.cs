using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities;
using Yocale.eShop.ApplicationCore.Entities.BasketAggregate;
using Yocale.eShop.ApplicationCore.Interfaces;
using Yocale.eShop.ApplicationCore.Specifications;
using Yocale.eShop.Resource.Errors;
using Yocale.eShop.Utility.Data;
using Yocale.eShop.WebAPI.ViewModels;
using IBasketService = Yocale.eShop.WebAPI.Interfaces.IBasketService;

namespace Yocale.eShop.WebAPI.Services
{
    public class BasketService : IBasketService
    {
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<Product> _itemRepository;

        public BasketService(IAsyncRepository<Basket> basketRepository,
            IAsyncRepository<Product> itemRepository)
        {
            _basketRepository = basketRepository;
            _itemRepository = itemRepository;
        }

        public async Task<ResultModel<BasketViewModel>> GetOrCreateBasketForUser(string userName)
        {
            try
            {
                var basketSpec = new BasketWithItemsSpecification(userName);

                var basket = (await _basketRepository.ListAsync(basketSpec)).FirstOrDefault();

                if (basket == null)
                {
                    return (await CreateBasketForUser(userName)).ToResultModel();
                }
                return (await CreateViewModelFromBasket(basket)).ToResultModel();
            }
            catch
            {
                return ResultModel<BasketViewModel>.Create(new InternalServerError());
            }
        }

        private async Task<BasketViewModel> CreateViewModelFromBasket(Basket basket)
        {
            var viewModel = new BasketViewModel();
            viewModel.Id = basket.Id;
            viewModel.CustomerId = basket.CustomerId;
            viewModel.Items = await GetBasketItems(basket.Items);
            return viewModel;
        }

        private async Task<BasketViewModel> CreateBasketForUser(string userId)
        {
            var basket = new Basket() { CustomerId = userId };
            basket = await _basketRepository.AddAsync(basket);

            return new BasketViewModel()
            {
                CustomerId = basket.CustomerId,
                Id = basket.Id,
                Items = new List<BasketItemViewModel>()
            };
        }

        private async Task<List<BasketItemViewModel>> GetBasketItems(IReadOnlyCollection<BasketItem> basketItems)
        {
            var items = new List<BasketItemViewModel>();

            foreach (var item in basketItems)
            {
                var itemModel = new BasketItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    ProductItemId = item.ProductItemId
                };
                var productItem = await _itemRepository.GetByIdAsync(item.ProductItemId);
                itemModel.ProductName = productItem.Name;
                items.Add(itemModel);
            }

            return items;
        }
    }
}
