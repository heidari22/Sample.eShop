using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities;
using Yocale.eShop.ApplicationCore.Entities.BasketAggregate;
using Yocale.eShop.ApplicationCore.Entities.OrderAggregate;
using Yocale.eShop.ApplicationCore.Exceptions;
using Yocale.eShop.ApplicationCore.Interfaces;
using Yocale.eShop.Resource.Errors;
using Yocale.eShop.Utility.Data;

namespace Yocale.eShop.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<Product> _productRepository;

        public OrderService(IAsyncRepository<Basket> basketRepository,
            IAsyncRepository<Product> productRepository,
            IAsyncRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
        }

        public async Task<ResultModel<int>> CreateOrderAsync(int basketId, Address shippingAddress)
        {
            try
            {
                var basket = await _basketRepository.GetByIdAsync(basketId);
                Guard.Against.NullBasket(basketId, basket);
                var items = new List<OrderItem>();
                foreach (var item in basket.Items)
                {
                    var productItem = await _productRepository.GetByIdAsync(item.ProductItemId);
                    Guard.Against.NullProduct(item.ProductItemId, productItem);
                    Guard.Against.ProductUnavailable(item.Quantity, productItem);

                    var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name);
                    var orderItem = new OrderItem(itemOrdered, item.Quantity);
                    items.Add(orderItem);

                    productItem.Quantity -= item.Quantity;
                }

                var order = new Order(basket.CustomerId, shippingAddress, items);

                return (await _orderRepository.AddAsync(order)).Id.ToResultModel();
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains($"No product found with "))
                    return ResultModel<int>.Create(new BadRequestError() { Message = "No product found" });

                if (ex.Message.Contains($"Unavailable product with "))
                    return ResultModel<int>.Create(new BadRequestError() { Message = "Unavailable product" });

                if (ex.Message.Contains($"No basket found with id {basketId}"))
                    return ResultModel<int>.Create(new NotFoundError() { Message = $"No basket found with id {basketId}" });

                return ResultModel<int>.Create(new InternalServerError());
            }
        }
    }
}
