using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class BankAccount : BaseEntity
    {
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string BankName { get; set; }
        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }
    }
}
