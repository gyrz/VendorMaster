using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
        public IEnumerable<Email> Emails { get; set; }
    }
}
