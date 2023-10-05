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
using Contracts.Dto.BankAccount;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicService.Services.BankAccountSvc
{
    public class BankAccountService : IBankAccountService
    {
        private readonly VendorDbContext vendorDbContext;
        private readonly IMapper mapper;
        public BankAccountService(VendorDbContext vendorDbContext, IMapper mapper)
        {
            this.vendorDbContext = vendorDbContext;
            this.mapper = mapper;
        }

        public async Task<Result<int>> AddOrUpdate(BankAccountDto bankAccount)
        {
            var bankAccountValidation =
                new BankAccountValidator(vendorDbContext, bankAccount);

            var validationResult = bankAccountValidation.Handle();
            if (!string.IsNullOrEmpty(validationResult))
            {
                return new Result<int> { ResultCode = 400, Message = validationResult };
            }

            var b = await vendorDbContext.BankAccounts
                .FirstOrDefaultAsync(x => x.Id == bankAccount.Id);
            if (b == null)
            {
                b = new BankAccount();
                b = mapper.Map<BankAccount>(bankAccount);
                vendorDbContext.BankAccounts.Add(b);
            }
            else
            {
                b = mapper.Map<BankAccount>(bankAccount);
            }

            await vendorDbContext.SaveChangesAsync();

            return new Result<int>(b.Id);
        }

        public async Task<Result<BankAccountDto>> Get(int id)
        {
            var res = await vendorDbContext.BankAccounts
                .FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Result<BankAccountDto> { ResultCode = 404 };

            return new Result<BankAccountDto>(mapper.Map<BankAccountDto>(res));
        }

        public async Task<Result<Dictionary<int, BankAccountDto>>> GetList(int[] idArr)
        {
            var result = new Result<Dictionary<int, BankAccountDto>>()
            {
                Data = new Dictionary<int, BankAccountDto>()
            };

            var count = await vendorDbContext.BankAccounts.CountAsync();
            if (idArr?.Length == count) return result;

            var res = await vendorDbContext.BankAccounts
                .Where(x => !idArr.Contains(x.Id))
                .Select(x => mapper.Map<BankAccountDto>(x))
                .ToListAsync();

            foreach (var itm in res)
            {
                result.Data.Add(itm.Id, itm);
            }

            return result;
        }

        public async Task<Result<bool>> Remove(int id)
        {
            var b = await vendorDbContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == id);
            if (b != null)
            {
                vendorDbContext.BankAccounts.Remove(b);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }

        public async Task<Result<bool>> RemoveAll(IEnumerable<int> exceptIds, int? vendorId = null )
        {
            var b = await vendorDbContext.BankAccounts.Where(x => (vendorId == null || x.VendorId == vendorId) && !exceptIds.Contains(x.Id)).ToListAsync();
            if (b != null)
            {
                vendorDbContext.BankAccounts.RemoveRange(b);
                await vendorDbContext.SaveChangesAsync();
                return new Result<bool>(true);
            }

            return new Result<bool> { Data = false, ResultCode = 404 };
        }
    }
}
