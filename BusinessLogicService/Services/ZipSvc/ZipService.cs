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
using Contracts.Dto.Zip;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicService.Services.ZipSvc
{
    public class ZipService : IZipService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public ZipService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(ZipSimpleDto zip)
        {
            var zipValidation =
                new ZipValidator(vendorDbContext, zip);

            var validationResult = zipValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var c = await vendorDbContext.ZipCodes.FirstOrDefaultAsync(x => x.Id == zip.Id);
            if (c == null)
            {
                c = new Zip();
                c = mapper.Map<Zip>(zip);
                vendorDbContext.ZipCodes.Add(c);
            }
            else
            {
                c = mapper.Map<Zip>(zip);
            }

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(c.Id);
        }

        public async Task<Result<ZipDto>> Get(int id)
        {
            var res = await vendorDbContext.ZipCodes
                .Include(x=>x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<ZipDto> { ResultCode = 404 };

            return new Result<ZipDto>(mapper.Map<ZipDto>(res));
        }

        public async Task<Result<Dictionary<int, ZipDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, ZipDto>>()
            {
                Data = new Dictionary<int, ZipDto>()
            };

            var count = await vendorDbContext.ZipCodes.CountAsync();
            if (idArr?.Length == count) return result;

            var res = await vendorDbContext.ZipCodes
                .Include(x=>x.Country)
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<ZipDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var c = await vendorDbContext.ZipCodes.FirstOrDefaultAsync(x => x.Id == id);
            if (c != null)
            {
                vendorDbContext.ZipCodes.Remove(c);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
