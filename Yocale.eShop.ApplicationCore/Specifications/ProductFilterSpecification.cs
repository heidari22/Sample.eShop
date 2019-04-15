﻿using System;
using System.Collections.Generic;
using System.Text;
using Yocale.eShop.ApplicationCore.Entities;

namespace Yocale.eShop.ApplicationCore.Specifications
{
    
    public class ProductFilterSpecification : BaseSpecification<Product>
    {
        public ProductFilterSpecification(int? supplierId, int? categoryId)
            : base(i => (!supplierId.HasValue || i.SupplierId == supplierId) &&
                (!categoryId.HasValue || i.CategoryId == categoryId))
        {
        }
    }
}

