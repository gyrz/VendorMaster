using Contracts.Dto.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Zip
{
    public class ZipDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }

        public CountryDto Country { get; set; }
    }
}
