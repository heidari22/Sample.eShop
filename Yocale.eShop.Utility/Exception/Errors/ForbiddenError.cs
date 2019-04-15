using Yocale.eShop.Utility.Exception;
using Yocale.eShop.Utility.Resource;

namespace Yocale.eShop.Resource.Errors
{
    public sealed class ForbiddenError : Error
    {
        public ForbiddenError()
        {
            this.Code = 403;
            this.Message = Shared.ForbiddenError;
        }
    }
 
}
