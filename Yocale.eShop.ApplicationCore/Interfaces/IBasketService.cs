using System.Threading.Tasks;
using Yocale.eShop.Utility.Data;

namespace Yocale.eShop.ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        Task<ResultModel<int>> AddItemToBasket(int basketId, int productItemId, int quantity);
        Task<ResultModel<bool>> DeleteBasketAsync(int basketId);
    }
}
