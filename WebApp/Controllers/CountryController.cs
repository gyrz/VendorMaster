using BusinessLogicService.Services.CitySvc;
using BusinessLogicService.Services.CountrySvc;
using Contracts;
using Contracts.Dto.City;
using Contracts.Dto.Country;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Controllers.TemplateControllers;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/country")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class CountryController : BaseController<CountryDto, CountryDto>
    {
        public CountryController(ICountryService countryService, IRedisCache redisCache) : base(countryService, redisCache)
        {
        }
    }
}
