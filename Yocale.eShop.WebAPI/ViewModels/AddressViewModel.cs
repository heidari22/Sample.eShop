using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yocale.eShop.WebAPI.ViewModels
{
    public class AddressViewModel
    {
        public String Street { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String Country { get; set; }

        public String ZipCode { get; set; }
    }
}
