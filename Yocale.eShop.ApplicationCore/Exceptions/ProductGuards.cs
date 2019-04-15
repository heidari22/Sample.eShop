using Ardalis.GuardClauses;
using Yocale.eShop.ApplicationCore.Entities;

namespace Yocale.eShop.ApplicationCore.Exceptions
{
    public static class ProductGuards
    {
        public static void NullProduct(this IGuardClause guardClause, int productId, Product product)
        {
            if (product == null)
                throw new ProductNotFoundException(productId);
        }

        public static void ProductUnavailable(this IGuardClause guardClause, int quantity, Product product)
        {
            if (!product.Quantity.HasValue || product.Quantity.Value < quantity)
                throw new ProductUnavailableException(product.Id);
        }
    }
}
