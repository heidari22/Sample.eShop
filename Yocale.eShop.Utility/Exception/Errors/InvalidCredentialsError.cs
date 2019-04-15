using Yocale.eShop.Utility.Exception;
using Yocale.eShop.Utility.Resource;

namespace Yocale.eShop.Resource.Errors
{
    public sealed class InvalidCredentialsError : Error
    {
        public InvalidCredentialsError() 
        {
            this.Code = 401;
            this.Message = Shared.InvalidCredentials;
        }
    }
}
