using AutoMapper;
using Contracts;
using Contracts.Dto.Country;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogicService.Services.CountrySvc
{
    public class CountryService : ICountryService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public CountryService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(CountryDto country)
        {
            var countryValidation =
                new CountryValidator(vendorDbContext, country);

            var validationResult = countryValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var c = await vendorDbContext.Countries.FirstOrDefaultAsync(x => x.Id == country.Id);
            if (c == null)
            {
                c = new Country();
                vendorDbContext.Countries.Add(c);
            }

            c = mapper.Map(country, c);

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(c.Id);
        }

        public async Task<Result<CountryDto>> Get(int id)
        {
            var res = await vendorDbContext.Countries
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<CountryDto> { ResultCode = 404 };

            return new Result<CountryDto>(mapper.Map<CountryDto>(res));
        }

        public async Task<Result<Dictionary<int, CountryDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, CountryDto>>()
            {
                Data = new Dictionary<int, CountryDto>()
            };

            var count = await vendorDbContext.Countries.CountAsync();
            if(idArr?.Length == count) return result;

            var res = await vendorDbContext.Countries
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<CountryDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var c = await vendorDbContext.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (c != null)
            {
                vendorDbContext.Countries.Remove(c);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }

        public async Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null)
        {
            var c = await vendorDbContext.Countries.Where(x => !exceptIds.Contains(x.Id)).ToListAsync();
            if (c != null)
            {
                vendorDbContext.Countries.RemoveRange(c);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
