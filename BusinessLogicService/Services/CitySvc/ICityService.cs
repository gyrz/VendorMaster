using Contracts.Dto.Country;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Dto.City;

namespace BusinessLogicService.Services.CitySvc
{
    public interface ICityService
    {
        public Task<Result<int>> AddOrUpdate(CitySimpleDto city);
        public Task<Result<bool>> Remove(int id);
        public Task<Result<Dictionary<int, CityDto>>> GetList(int[] idArr);
        public Task<Result<CityDto>> Get(int id);
    }
}
