using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Dto.Zip;
using Contracts.Dto.Vendor;

namespace BusinessLogicService.Services.ZipSvc
{
    public interface IZipService : IBase<ZipSimpleDto, ZipDto>
    {
    }
}
