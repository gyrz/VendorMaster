using BusinessLogicService.Services.CitySvc;
using BusinessLogicService.Services.CountrySvc;
using Contracts;
using Contracts.Dto.City;
using Contracts.Dto.Country;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/country")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class CountryController : Controller
    {
        private readonly ICountryService countryService;
        private readonly IRedisCache redisCache;

        public CountryController(ICountryService countryService, IRedisCache redisCache)
        {
            this.countryService = countryService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public async Task<IActionResult> AddOrUpdate(CountryDto countryDto)
        {
            var res = await countryService.AddOrUpdate(countryDto);
            if (res.ResultCode == 400)
                return BadRequest(res);
            if (res.ResultCode == 404)
                return NotFound(res);

            var resGet = await redisCache.Get<CountryDto>(typeof(CountryDto).ToString(), res.Data, async () => await countryService.Get(res.Data));

            return Ok(resGet);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<CountryDto>(typeof(CountryDto).ToString(), id, async () => await countryService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<CountryDto>(typeof(CountryDto).ToString(), async (int[] idArr) => await countryService.GetList(idArr));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await redisCache.Remove<bool>(typeof(CountryDto).ToString(), id, async () => await countryService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
