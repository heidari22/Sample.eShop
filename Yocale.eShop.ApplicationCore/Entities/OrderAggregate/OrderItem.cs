using System;
using System.Collections.Generic;
using System.Text;

namespace Yocale.eShop.ApplicationCore.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public ProductItemOrdered ItemOrdered { get; private set; }
        public int Units { get; private set; }

        private OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered itemOrdered, int units)
        {
            ItemOrdered = itemOrdered;
            Units = units;
        }
    }
}
