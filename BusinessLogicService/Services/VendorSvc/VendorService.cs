using AutoMapper;
using Contracts;
using Contracts.Dto.Vendor;
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

namespace BusinessLogicService.Services.VendorSvc
{
    public class VendorService : IVendorService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public VendorService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(VendorDto vendor)
        {
            var vendorValidation =
                new VendorValidator(vendorDbContext, vendor);

            var validationResult = vendorValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var simpleDto = mapper.Map<VendorSimpleDto>(vendor);

            var v = await vendorDbContext.Vendors.FirstOrDefaultAsync(x => x.Id == vendor.Id);
            if (v == null)
            {
                v = new Vendor();
                v = mapper.Map<Vendor>(simpleDto);
                vendorDbContext.Vendors.Add(v);
            }
            else
            {
                v = mapper.Map<Vendor>(simpleDto);
            }

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(v.Id);
        }

        public async Task<Result<VendorDto>> Get(int id)
        {
            var res = await vendorDbContext.Vendors
                .Include(x => x.ContactPersons)
                .Include(x => x.Addresses)
                    .ThenInclude(x =>x.City)
                        .ThenInclude(x=>x.Country)
                .Include(x => x.Addresses)
                    .ThenInclude(x => x.Zip)
                .Include(x => x.Phones)
                .Include(x => x.Emails)
                .Include(x => x.BankAccounts)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<VendorDto> { ResultCode = 404 };

            return new Result<VendorDto>((new VendorMapper(mapper)).ToDto(res));
        }

        public async Task<Result<Dictionary<int, VendorDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, VendorDto>>()
            {
                Data = new Dictionary<int, VendorDto>()
            };

            var count = await vendorDbContext.Vendors.CountAsync();
            if(idArr?.Length == count) return result;

            VendorMapper vendorMapper = new VendorMapper(mapper);

            var res = await vendorDbContext.Vendors
                .Include(x => x.ContactPersons)
                .Include(x => x.Addresses)
                    .ThenInclude(x => x.City)
                        .ThenInclude(x => x.Country)
                .Include(x => x.Addresses)
                    .ThenInclude(x => x.Zip)
                .Include(x => x.Phones)
                .Include(x => x.Emails)
                .Include(x => x.BankAccounts)
                .Where(x => !idArr.Contains(x.Id))
                .Select(x=> vendorMapper.ToDto(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var emails = await vendorDbContext.Emails.Where(x => x.VendorId == id && x.PersonId == null).ToListAsync();
            var phones = await vendorDbContext.Phones.Where(x => x.VendorId == id && x.PersonId == null).ToListAsync();
            vendorDbContext.Emails.RemoveRange(emails);
            vendorDbContext.Phones.RemoveRange(phones);

            var v = await vendorDbContext.Vendors.FirstOrDefaultAsync(x => x.Id == id);
            if (v != null)
            {
                vendorDbContext.Vendors.Remove(v);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }

        public async Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null)
        {
            var v = await vendorDbContext.Vendors.Where(x => !exceptIds.Contains(x.Id)).ToListAsync();
            if (v != null)
            {
                var vendorIdList = v.Select(x => x.Id).ToList();
                var emails = await vendorDbContext.Emails.Where(x => x.PersonId == null && vendorIdList.Contains(x.VendorId ?? -1)).ToListAsync();
                var phones = await vendorDbContext.Phones.Where(x => x.PersonId == null && vendorIdList.Contains(x.VendorId ?? -1)).ToListAsync();
                vendorDbContext.Emails.RemoveRange(emails);
                vendorDbContext.Phones.RemoveRange(phones);

                vendorDbContext.Vendors.RemoveRange(v);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
