using System.Collections.Generic;
using System.Linq;
using Yocale.eShop.ApplicationCore.Interfaces;

namespace Yocale.eShop.ApplicationCore.Entities.BasketAggregate
{
    public class Basket : BaseEntity, IAggregateRoot
    {
        public string CustomerId { get; set; }
        private readonly List<BasketItem> _items = new List<BasketItem>();
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public int AddItem(int productItemId, int quantity = 1)
        {
            if (!Items.Any(i => i.ProductItemId == productItemId))
            {
                _items.Add(new BasketItem()
                {
                    ProductItemId = productItemId,
                    Quantity = quantity,
                });

                return quantity;
            }

            var existingItem = Items.FirstOrDefault(i => i.ProductItemId == productItemId);

            existingItem.Quantity += quantity;

            return existingItem.Quantity;
        }
    }
}
