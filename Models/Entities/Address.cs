using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public enum AddressType
    {
        Billing,
        Delivery
    }

    public class Address : BaseEntity
    {
        public AddressType Type { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int VendorId { get; set; }
        public int ZipId { get; set; }
        public int CityId { get; set; }

        public Zip Zip { get; set; }
        public City City { get; set; }
        public Vendor Vendor { get; set; }
    }
}
