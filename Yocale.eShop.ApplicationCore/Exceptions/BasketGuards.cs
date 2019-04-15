using Ardalis.GuardClauses;
using Yocale.eShop.ApplicationCore.Entities.BasketAggregate;

namespace Yocale.eShop.ApplicationCore.Exceptions
{
    public static class BasketGuards
    {
        public static void NullBasket(this IGuardClause guardClause, int basketId, Basket basket)
        {
            if (basket == null)
                throw new BasketNotFoundException(basketId);
        }
    }
}
