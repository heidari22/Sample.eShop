using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yocale.eShop.Utility.Data;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.WebAPI.Interfaces
{
    public interface IBasketService
    {
        Task<ResultModel<BasketViewModel>> GetOrCreateBasketForUser(string userName);
    }
}
