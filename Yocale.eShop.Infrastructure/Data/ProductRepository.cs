using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities;
using Yocale.eShop.ApplicationCore.Interfaces;

namespace Yocale.eShop.Infrastructure.Data
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(EShopContext dbContext) : base(dbContext)
        {
        }

        public async Task<Product> CreateProductWithSKUAsync(Product product)
        {
            await _dbContext.AddAsync(product);

            product.Sku = await GenerateSKUAsync(product.Id, product.CategoryId, product.SupplierId);// $"yp-{product.CategoryId}-{product.SupplierId}-{product.Id}";

            await _dbContext.SaveChangesAsync();
           
            return product;
        }

        public async Task<string> GenerateSKUAsync(int productId, int categoryId, int supplierId)
        {
            return await Task.Run(() => $"yp-{categoryId}-{supplierId}-{productId}");
        }
    }
    
}
