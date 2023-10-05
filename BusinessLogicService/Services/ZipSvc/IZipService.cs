using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Dto.Zip;

namespace BusinessLogicService.Services.ZipSvc
{
    public interface IZipService
    {
        public Task<Result<int>> AddOrUpdate(ZipSimpleDto Zip);
        public Task<Result<bool>> Remove(int id);
        public Task<Result<Dictionary<int, ZipDto>>> GetList(int[] idArr);
        public Task<Result<ZipDto>> Get(int id);
    }
}
