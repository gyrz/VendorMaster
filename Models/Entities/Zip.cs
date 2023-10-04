using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Zip : BaseEntity
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}
