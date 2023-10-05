using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Email : BaseEntity
    {
        public int? PersonId { get; set; }
        public int? VendorId { get; set; }
        public string Address { get; set; }

        public Person? Person { get; set; }
        public Vendor? Vendor { get; set; }
    }
}
