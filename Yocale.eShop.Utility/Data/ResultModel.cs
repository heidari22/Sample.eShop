

using Yocale.eShop.Utility.Exception;

namespace Yocale.eShop.Utility.Data
{
    public sealed class ResultModel<T>
    {
        public T Data { get; set; }
        public Error Error { get; set; }

        private ResultModel()
        {
        }

        public static ResultModel<T> Create(T data)
        {
            return new ResultModel<T>
            {
                Data = data,
                Error = null
            };
        }

        public static ResultModel<T> Create(Error error)
        {
            return new ResultModel<T>
            {
                Error = error,
            };
        }
    }
}
