using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yocale.eShop.ApplicationCore.Entities.OrderAggregate
{
    /// <summary>
    /// ValueObject
    /// </summary>
    public class ProductItemOrdered 
    {
        public ProductItemOrdered(int productItemId, string productName)
        {
            Guard.Against.OutOfRange(productItemId, nameof(productItemId), 1, int.MaxValue);
            Guard.Against.NullOrEmpty(productName, nameof(productName));

            ProductItemId = productItemId;
            ProductName = productName;
        }

        private ProductItemOrdered()
        {
        }

        public int ProductItemId { get; private set; }
        public string ProductName { get; private set; }
    }
}
