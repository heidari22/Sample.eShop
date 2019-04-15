using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;
using Yocale.eShop.ApplicationCore.Interfaces;

namespace Yocale.eShop.ApplicationCore.Entities.CustomerAggregate
{
    public class Customer : BaseEntity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }
        
        private Customer()
        {
        }

        public Customer(string identity) : this()
        {
            Guard.Against.NullOrEmpty(identity, nameof(identity));
            IdentityGuid = identity;
        }
    }
}
