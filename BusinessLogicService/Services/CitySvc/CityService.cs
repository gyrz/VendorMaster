using AutoMapper;
using BusinessLogicService.Services.CountrySvc;
using Contracts.Dto.Country;
using Contracts;
using DataAccess.Data;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Dto.City;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicService.Services.CitySvc
{
    public class CityService : ICityService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public CityService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(CitySimpleDto city)
        {
            var cityValidation =
                new CityValidator(vendorDbContext, city);

            var validationResult = cityValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var c = await vendorDbContext.Cities
                .FirstOrDefaultAsync(x => x.Id == city.Id);
            if (c == null)
            {
                c = new City();
                c = mapper.Map<City>(city);
                vendorDbContext.Cities.Add(c);
            }
            else
            {
                c = mapper.Map<City>(city);
            }

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(c.Id);
        }

        public async Task<Result<CityDto>> Get(int id)
        {
            var res = await vendorDbContext.Cities
                .Include(x=> x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<CityDto> { ResultCode = 404 };

            return new Result<CityDto>(mapper.Map<CityDto>(res));
        }

        public async Task<Result<Dictionary<int, CityDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, CityDto>>()
            {
                Data = new Dictionary<int, CityDto>()
            };

            var count = await vendorDbContext.Cities.CountAsync();
            if (idArr?.Length == count) return result;

            var res = await vendorDbContext.Cities
                .Include(x=>x.Country)
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<CityDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var c = await vendorDbContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
            if (c != null)
            {
                vendorDbContext.Cities.Remove(c);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
