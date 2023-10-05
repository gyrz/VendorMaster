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
using Contracts.Dto.Address;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicService.Services.AddressSvc
{
    public class AddressService : IAddressService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public AddressService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(AddressSimpleDto address)
        {
            var addressValidation =
                new AddressValidator(vendorDbContext, address);

            var validationResult = addressValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var a = await vendorDbContext.Addresses
                .FirstOrDefaultAsync(x => x.Id == address.Id);
            if (a == null)
            {
                a = new Address();
                a = mapper.Map<Address>(address);
                vendorDbContext.Addresses.Add(a);
            }
            else
            {
                a = mapper.Map<Address>(address);
            }

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(a.Id);
        }

        public async Task<Result<AddressDto>> Get(int id)
        {
            var res = await vendorDbContext.Addresses
                .Include(x=> x.City)
                .Include(x => x.Zip)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<AddressDto> { ResultCode = 404 };

            return new Result<AddressDto>(mapper.Map<AddressDto>(res));
        }

        public async Task<Result<Dictionary<int, AddressDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, AddressDto>>()
            {
                Data = new Dictionary<int, AddressDto>()
            };

            var count = await vendorDbContext.Addresses.CountAsync();
            if (idArr?.Length == count) return result;

            var res = await vendorDbContext.Addresses
                .Include(x=>x.City)
                .Include(x=>x.Zip)
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<AddressDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var a = await vendorDbContext.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            if (a != null)
            {
                vendorDbContext.Addresses.Remove(a);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }

        public async Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null)
        {
            var a = await vendorDbContext.Addresses.Where(x => (vendorId == null || x.VendorId == vendorId) && !exceptIds.Contains(x.Id)).ToListAsync();
            if (a != null)
            {
                vendorDbContext.Addresses.RemoveRange(a);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
