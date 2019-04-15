using System;

namespace Yocale.eShop.ApplicationCore.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(int basketId) : base($"No product found with id {basketId}")
        {
        }

        protected ProductNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        public ProductNotFoundException(string message) : base(message)
        {
        }

        public ProductNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
