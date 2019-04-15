using System.Collections.Generic;
using System.Threading.Tasks;
using Yocale.eShop.Utility.Data;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.WebAPI.Interfaces
{
    public interface ICachedProductService
    {
        Task<ResultModel<ProductIndexViewModel>> GetProductList(int pageIndex, int itemsPage, int? supplierId, int? categoryId);
        Task<ResultModel<List<SelectListModel>>> GetSuppliers();
        Task<ResultModel<List<SelectListModel>>> GetCategories();
        Task<ResultModel<ProductDetailViewModel>> GetById(int id);
    }
}
