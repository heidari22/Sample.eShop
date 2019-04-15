using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yocale.eShop.WebAPI.ViewModels
{
    public class BasketViewModel
    {
        public int Id { get; set; }
        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
        public string CustomerId { get; set; }

        public int TotalItemCount()
        {
            return Items.Sum(x => x.Quantity);
        }
    }

    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
