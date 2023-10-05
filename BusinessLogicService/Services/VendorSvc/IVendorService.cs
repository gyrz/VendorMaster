using Contracts;
using Contracts.Dto.Phone;
using Contracts.Dto.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.Services.VendorSvc
{
    public interface IVendorService : IBaseService<VendorDto, VendorDto>
    {
    }
}
