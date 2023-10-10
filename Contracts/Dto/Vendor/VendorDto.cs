using Contracts.Dto.Address;
using Contracts.Dto.BankAccount;
using Contracts.Dto.Email;
using Contracts.Dto.Person;
using Contracts.Dto.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Vendor
{
    public class VendorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Notes { get; set; }
        public IEnumerable<AddressSimpleDto> Addresses { get; set; }
        public IEnumerable<PhoneDto> Phones { get; set; }
        public IEnumerable<BankAccountDto> BankAccounts { get; set; }
        public IEnumerable<EmailDto> Emails { get; set; }
        public IEnumerable<PersonSimpleDto> ContactPersons { get; set; }
    }
}
