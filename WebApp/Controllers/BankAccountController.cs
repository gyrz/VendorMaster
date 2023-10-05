using BusinessLogicService.Services.BankAccountSvc;
using Contracts;
using Contracts.Dto.BankAccount;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/bank")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.VENDORMASTER)]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService bankService;
        private readonly IRedisCache redisCache;

        public BankAccountController(IBankAccountService bankService, IRedisCache redisCache)
        {
            this.bankService = bankService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public async Task<IActionResult> AddOrUpdate(BankAccountDto bankDto)
        {
            var res = await bankService.AddOrUpdate(bankDto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get<BankAccountDto>(typeof(BankAccountDto).ToString(), res.Data, async () => await bankService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<BankAccountDto>(typeof(BankAccountDto).ToString(), id, async () => await bankService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<BankAccountDto>(typeof(BankAccountDto).ToString(), async (int[] idArr) => await bankService.GetList(idArr));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await redisCache.Remove<BankAccountDto>(typeof(BankAccountDto).ToString(), id, async () => await bankService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
