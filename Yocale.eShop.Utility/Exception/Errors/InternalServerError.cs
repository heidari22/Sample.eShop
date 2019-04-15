using Yocale.eShop.Utility.Exception;
using Yocale.eShop.Utility.Resource;

namespace Yocale.eShop.Resource.Errors
{
    public sealed class InternalServerError : Error
    {
        public InternalServerError()
        {
            this.Code = 500;
            this.Message = Shared.ServerError;
        }
    }
}
