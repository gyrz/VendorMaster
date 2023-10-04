using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Vendor : BaseEntity
    {
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Notes { get; set; }

        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
        public IEnumerable<BankAccount> BankAccounts { get; set; }
        public IEnumerable<Email> Emails { get; set; }
        public IEnumerable<Person> ContactPersons { get; set; }
    }
}
