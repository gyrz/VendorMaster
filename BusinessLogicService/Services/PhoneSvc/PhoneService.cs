using AutoMapper;
using Contracts;
using Contracts.Dto;
using Contracts.Dto.Phone;
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

namespace BusinessLogicService.Services.PhoneSvc
{
    public class PhoneService : IPhoneService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public PhoneService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(PhoneDto phone)
        {
            var phoneValidation =
                new PhoneValidator(vendorDbContext, phone);

            var validationResult = phoneValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var p = await vendorDbContext.Phones.FirstOrDefaultAsync(x => x.Id == phone.Id);
            if (p == null)
            {
                p = new Phone();
                vendorDbContext.Phones.Add(p);
            }

            p = mapper.Map(phone, p);
            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(p.Id);
        }

        public async Task<Result<PhoneDto>> Get(int id)
        {
            var res = await vendorDbContext.Phones
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<PhoneDto> { ResultCode = 404 };

            return new Result<PhoneDto>(mapper.Map<PhoneDto>(res));
        }

        public async Task<Result<Dictionary<int, PhoneDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, PhoneDto>>()
            {
                Data = new Dictionary<int, PhoneDto>()
            };

            var count = await vendorDbContext.Phones.CountAsync();
            if(idArr?.Length == count) return result;

            var res = await vendorDbContext.Phones
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<PhoneDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var p = await vendorDbContext.Phones.FirstOrDefaultAsync(x => x.Id == id);
            if (p != null)
            {
                vendorDbContext.Phones.Remove(p);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }

        public async Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null)
        {
            var p = await vendorDbContext.Phones.Where(x => (vendorId == null || x.VendorId == vendorId) && !exceptIds.Contains(x.Id)).ToListAsync();
            if (p != null)
            {
                vendorDbContext.Phones.RemoveRange(p);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
