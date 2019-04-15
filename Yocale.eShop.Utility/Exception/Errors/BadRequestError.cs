using Yocale.eShop.Utility.Exception;
using Yocale.eShop.Utility.Resource;

namespace Yocale.eShop.Resource.Errors
{
    public sealed class BadRequestError : Error
    {
        public BadRequestError()
        {
            this.Code = 400;
            this.Message = Shared.BadRequest;
        }
    }
}
