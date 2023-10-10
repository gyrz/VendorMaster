using Contracts.Dto.Vendor;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Address
{
    public class AddressSimpleDto : BaseVendorRelationDto
    {
        public AddressType Type { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int ZipId { get; set; }
        public int CityId { get; set; }
    }
}
