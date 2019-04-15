using System.Collections.Generic;
using Yocale.eShop.Utility.Data;

namespace Yocale.eShop.WebAPI.ViewModels
{
    public class ProductIndexViewModel
    {
        public PagedList<ProductDetailViewModel> ProductList { get; set; }
        public IEnumerable<SelectListModel> Suppliers { get; set; }
        public IEnumerable<SelectListModel> Categories { get; set; }
        public int? SupplierFilterApplied { get; set; }
        public int? CategoryFilterApplied { get; set; }
    }
}
