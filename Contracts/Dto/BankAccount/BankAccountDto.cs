using Contracts.Dto.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.BankAccount
{
    public class BankAccountDto : BaseVendorRelationDto
    {
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string BankName { get; set; }
    }
}
