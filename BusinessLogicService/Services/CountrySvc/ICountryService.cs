using Contracts;
using Contracts.Dto.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.Services.CountrySvc
{
    public interface ICountryService
    {
        public Task<Result<int>> AddOrUpdate(CountryDto country);
        public Task<Result<bool>> Remove(int id);
        public Task<Result<Dictionary<int, CountryDto>>> GetList(int[] idArr);
        public Task<Result<CountryDto>> Get(int id);

    }
}
