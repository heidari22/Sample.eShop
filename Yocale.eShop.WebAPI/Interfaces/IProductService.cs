using System.Threading.Tasks;
using Yocale.eShop.Utility.Data;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.WebAPI.Interfaces
{
    public interface IProductService : ICachedProductService
    {
        Task<ResultModel<ProductDetailViewModel>> AddProductWithSKU(int categoryId, int supplierId, string name, int? quantity, string description);
        Task<ResultModel<bool>> UpdateProduct(int productId, int categoryId, int supplierId, string name, int? quantity, string description);
        Task<ResultModel<bool>> DeleteProduct(int id);
    }
}
