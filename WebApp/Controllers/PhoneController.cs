using BusinessLogicService.Services.PhoneSvc;
using Contracts;
using Contracts.Dto.Phone;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;
using VendorMaster.Controllers.TemplateControllers;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/phone")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.BASE)]
    public class PhoneController : BaseController<PhoneDto, PhoneDto>
    {
        public PhoneController(IPhoneService phoneService, IRedisCache redisCache) : base(phoneService, redisCache)
        {
        }
    }
}
