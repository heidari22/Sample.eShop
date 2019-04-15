using System;
using System.Collections.Generic;
using System.Text;

namespace Yocale.eShop.ApplicationCore.Entities
{
    public partial class Product : IEquatable<Product>
    {
        public bool Equals(Product other)
        {
            return other.Id == this.Id;
        }

        public Product(string sku, out long sequenceNumber)
        {
            var skuArr = sku.Split('-');

            var prefix= Convert.ToInt32(skuArr[0]);

            CategoryId = Convert.ToInt32(skuArr[1]);

            SupplierId = Convert.ToInt32(skuArr[2]);

            sequenceNumber = Convert.ToInt32(skuArr[3]);
        }
    }
}
