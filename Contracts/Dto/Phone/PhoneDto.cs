using Contracts.Dto.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Phone
{
    public class PhoneDto : BaseVendorRelationDto
    {
        public int? PersonId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
