using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yocale.eShop.WebAPI.ViewModels
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public int?  Quantity { get; set; }
        public string   Description { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}
