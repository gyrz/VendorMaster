using Contracts.Dto.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Person
{
    public class PersonSimpleDto : BaseVendorRelationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
