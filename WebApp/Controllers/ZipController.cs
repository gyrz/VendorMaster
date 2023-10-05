using BusinessLogicService.Services.ZipSvc;
using Contracts;
using Contracts.Dto.Zip;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;
using BusinessLogicService.Services.CountrySvc;
using Contracts.Dto.Country;
using VendorMaster.Controllers.TemplateControllers;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/zip")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class ZipController : BaseController<ZipSimpleDto, ZipDto>
    {
        public ZipController(IZipService zipService, IRedisCache redisCache) : base(zipService, redisCache)
        {
        }
    }
}
