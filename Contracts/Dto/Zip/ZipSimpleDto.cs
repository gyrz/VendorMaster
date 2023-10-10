using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.Zip
{
    public class ZipSimpleDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }
    }
}
