using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Vendor
{
    public class BaseVendorRelationDto
    {
        public int Id { get; set; }
        public int? VendorId { get; set; }
    }
}
