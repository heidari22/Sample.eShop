using System;

namespace Yocale.eShop.ApplicationCore.Exceptions
{
    public class ProductUnavailableException : Exception
    {
        public ProductUnavailableException(int productId) : base($"Unavailable product with id {productId}")
        {
        }

        protected ProductUnavailableException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        public ProductUnavailableException(string message) : base(message)
        {
        }

        public ProductUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
