using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;
using Yocale.eShop.ApplicationCore.Interfaces;

namespace Yocale.eShop.ApplicationCore.Entities.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        private Order()
        {
        }

        public Order(string customerId, Address shipToAddress, List<OrderItem> items)
        {
            Guard.Against.NullOrEmpty(customerId, nameof(customerId));
            Guard.Against.Null(shipToAddress, nameof(shipToAddress));
            Guard.Against.Null(items, nameof(items));

            CustomerId = customerId;
            ShipToAddress = shipToAddress;
            _orderItems = items;
        }

        public string CustomerId { get; private set; }

        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;

        public Address ShipToAddress { get; private set; }

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
 
        public int TotalItemCount()
        {
            var total = 0;
            foreach (var item in _orderItems)
            {
                total += item.Units;
            }
            return total;
        }
    }
}
