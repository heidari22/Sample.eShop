using System;
using System.Collections.Generic;
using System.Text;

namespace Yocale.eShop.ApplicationCore.Entities.BasketAggregate
{
    public class BasketItem : BaseEntity
    {
        public int Quantity { get; set; }
        public int ProductItemId { get; set; }
    }
}
