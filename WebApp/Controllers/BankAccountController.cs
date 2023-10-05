using BusinessLogicService.Services.BankAccountSvc;
using Contracts;
using Contracts.Dto.BankAccount;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Controllers.TemplateControllers;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/bank")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.VENDORMASTER)]
    public class BankAccountController : VendorMasterController<BankAccountDto, BankAccountDto>
    {
        public BankAccountController(IBankAccountService bankService, IRedisCache redisCache) : base(bankService, redisCache)
        {
        }
    }
}
