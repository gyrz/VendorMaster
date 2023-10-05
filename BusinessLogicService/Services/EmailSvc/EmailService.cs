using AutoMapper;
using Contracts;
using Contracts.Dto.Email;
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

namespace BusinessLogicService.Services.EmailSvc
{
    public class EmailService : IEmailService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public EmailService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(EmailDto email)
        {
            var emailValidation =
                new EmailValidator(vendorDbContext, email);

            var validationResult = emailValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var e = await vendorDbContext.Emails.FirstOrDefaultAsync(x => x.Id == email.Id);
            if (e == null)
            {
                e = new Email();
                e = mapper.Map<Email>(email);
                vendorDbContext.Emails.Add(e);
            }
            else
            {
                e = mapper.Map<Email>(email);
            }

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(e.Id);
        }

        public async Task<Result<EmailDto>> Get(int id)
        {
            var res = await vendorDbContext.Emails
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<EmailDto> { ResultCode = 404 };

            return new Result<EmailDto>(mapper.Map<EmailDto>(res));
        }

        public async Task<Result<Dictionary<int, EmailDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, EmailDto>>()
            {
                Data = new Dictionary<int, EmailDto>()
            };

            var count = await vendorDbContext.Emails.CountAsync();
            if(idArr?.Length == count) return result;

            var res = await vendorDbContext.Emails
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<EmailDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var e = await vendorDbContext.Emails.FirstOrDefaultAsync(x => x.Id == id);
            if (e != null)
            {
                vendorDbContext.Emails.Remove(e);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }

        public async Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null)
        {
            var e = await vendorDbContext.Emails.Where(x => (vendorId == null || x.VendorId == vendorId) && !exceptIds.Contains(x.Id)).ToListAsync();
            if (e != null)
            {
                vendorDbContext.Emails.RemoveRange(e);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
