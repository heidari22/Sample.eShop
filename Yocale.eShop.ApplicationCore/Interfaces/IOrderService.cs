using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities.OrderAggregate;
using Yocale.eShop.Utility.Data;

namespace Yocale.eShop.ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task<ResultModel<int>> CreateOrderAsync(int basketId, Address shippingAddress);
    }
}
