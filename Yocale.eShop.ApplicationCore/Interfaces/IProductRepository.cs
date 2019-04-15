using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities;

namespace Yocale.eShop.ApplicationCore.Interfaces
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<Product> CreateProductWithSKUAsync(Product product);
        Task<string> GenerateSKUAsync(int productId, int categoryId, int supplierId);
    }
}
