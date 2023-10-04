using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }

        public IEnumerable<City> Cities { get; set; }
        // sometimes when someone on the edge of the city, the zip code is different and relates to other city
        // so we need to add a list of zip codes to country, not to the city
        public IEnumerable<Zip> ZipCodes { get; set; } 

    }
}
