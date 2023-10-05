using BusinessLogicService.Services.AddressSvc;
using BusinessLogicService.Services.CitySvc;
using Contracts;
using Contracts.Dto.Address;
using Contracts.Dto.City;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Controllers.TemplateControllers;
using VendorMaster.Extensions;


namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/city")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class CityController : BaseController<CitySimpleDto, CityDto>
    {
        public CityController(ICityService cityService, IRedisCache redisCache) : base(cityService, redisCache)
        {
        }
    }
}