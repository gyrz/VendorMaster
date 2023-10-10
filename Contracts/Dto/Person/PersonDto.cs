using Contracts.Dto.Email;
using Contracts.Dto.Phone;
using Contracts.Dto.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Person
{
    public class PersonDto : BaseVendorRelationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<PhoneDto> Phones { get; set; }
        public IEnumerable<EmailDto> Emails { get; set; }
    }
}
