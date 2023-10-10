using AutoMapper;
using Contracts;
using Contracts.Dto;
using Contracts.Dto.Person;
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

namespace BusinessLogicService.Services.PersonSvc
{
    public class PersonService : IPersonService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public PersonService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(PersonSimpleDto phone)
        {
            var phoneValidation =
                new PersonValidator(vendorDbContext, phone);

            var validationResult = phoneValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var p = await vendorDbContext.Persons.FirstOrDefaultAsync(x => x.Id == phone.Id);
            if (p == null)
            {
                p = new Person();
                vendorDbContext.Persons.Add(p);
            }

            p = mapper.Map(phone, p);

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(p.Id);
        }

        public async Task<Result<PersonDto>> Get(int id)
        {
            var res = await vendorDbContext.Persons
                .Include(x=>x.Emails)
                .Include(x=>x.Phones)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<PersonDto> { ResultCode = 404 };

            return new Result<PersonDto>(mapper.Map<PersonDto>(res));
        }

        public async Task<Result<Dictionary<int, PersonDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, PersonDto>>()
            {
                Data = new Dictionary<int, PersonDto>()
            };

            var count = await vendorDbContext.Persons.CountAsync();
            if(idArr?.Length == count) return result;

            var res = await vendorDbContext.Persons
                .Include(x => x.Emails)
                .Include(x => x.Phones)
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<PersonDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var emails = await vendorDbContext.Emails.Where(x => x.VendorId == null && x.PersonId == id).ToListAsync();
            var phones = await vendorDbContext.Phones.Where(x => x.VendorId == null && x.PersonId == id).ToListAsync();
            vendorDbContext.Emails.RemoveRange(emails);
            vendorDbContext.Phones.RemoveRange(phones);

            var p = await vendorDbContext.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (p != null)
            {
                vendorDbContext.Persons.Remove(p);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }

        public async Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null)
        {
            var p = await vendorDbContext.Persons.Where(x => (vendorId == null || x.VendorId == vendorId) && !exceptIds.Contains(x.Id)).ToListAsync();
            if (p != null)
            {
                var personIdList = p.Select(x => x.Id).ToList();
                var emails = await vendorDbContext.Emails.Where(x => x.VendorId == null && personIdList.Contains(x.PersonId ?? -1)).ToListAsync();
                var phones = await vendorDbContext.Phones.Where(x => x.VendorId == null && personIdList.Contains(x.PersonId ?? -1)).ToListAsync();
                vendorDbContext.Emails.RemoveRange(emails);
                vendorDbContext.Phones.RemoveRange(phones);

                vendorDbContext.Persons.RemoveRange(p);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
