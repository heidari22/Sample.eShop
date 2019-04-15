using System;
using System.Collections.Generic;
using System.Text;
using Yocale.eShop.ApplicationCore.Entities;

namespace Yocale.eShop.ApplicationCore.Specifications
{
    public class ProductFilterPaginatedSpecification : BaseSpecification<Product>
    {
        public ProductFilterPaginatedSpecification(int skip, int take, int? supplierId, int? categoryId)
            : base(i => (!supplierId.HasValue || i.SupplierId == supplierId) &&
                (!categoryId.HasValue || i.CategoryId == categoryId))
        {
            ApplyPaging(skip, take);
        }
    }
}
