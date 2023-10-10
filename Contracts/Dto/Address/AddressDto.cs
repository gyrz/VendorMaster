using Contracts.Dto.City;
using Contracts.Dto.Zip;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Address
{
    public class AddressDto
    {
        public int Id { get; set; }
        public AddressType Type { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int VendorId { get; set; }
        public int ZipId { get; set; }
        public int CityId { get; set; }

        public ZipDto Zip { get; set; }
        public CityDto City { get; set; }
    }
}
