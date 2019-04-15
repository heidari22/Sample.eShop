using Yocale.eShop.Utility.Exception;
using Yocale.eShop.Utility.Resource;

namespace Yocale.eShop.Resource.Errors
{
    public sealed class NotFoundError : Error
    {
        public NotFoundError()
        {
            this.Code = 404;
            this.Message = Shared.NotFound;
        }
    }
}
