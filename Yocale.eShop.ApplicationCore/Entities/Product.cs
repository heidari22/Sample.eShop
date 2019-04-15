using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yocale.eShop.ApplicationCore.Entities
{
    public partial class Product : BaseEntity
    {
        public Product()
        {
        }

        public string Name { get; set; }

        public string Sku { get; set; }

        public string Description { get; set; }

        public int? Quantity { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

    }
}
