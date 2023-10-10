using Contracts.Dto.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Email
{
    public class EmailDto : BaseVendorRelationDto
    {
        public int? PersonId { get; set; }
        public string Address { get; set; }
    }
}
