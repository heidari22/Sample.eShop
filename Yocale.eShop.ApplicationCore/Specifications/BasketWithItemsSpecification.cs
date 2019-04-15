using System;
using System.Collections.Generic;
using System.Text;
using Yocale.eShop.ApplicationCore.Entities.BasketAggregate;

namespace Yocale.eShop.ApplicationCore.Specifications
{
    public sealed class BasketWithItemsSpecification : BaseSpecification<Basket>
    {
        public BasketWithItemsSpecification(int basketId)
            : base(b => b.Id == basketId)
        {
            AddInclude(b => b.Items);
        }
        public BasketWithItemsSpecification(string buyerId)
            : base(b => b.CustomerId == buyerId)
        {
            AddInclude(b => b.Items);
        }
    }
}
