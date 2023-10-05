using BusinessLogicService.Services.CitySvc;
using Contracts;
using Contracts.Dto.City;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/city")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class CityController : Controller
    {
        private readonly ICityService cityService;
        private readonly IRedisCache redisCache;

        public CityController(ICityService cityService, IRedisCache redisCache)
        {
            this.cityService = cityService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public async Task<IActionResult> AddOrUpdate(CitySimpleDto cityDto)
        {
            var res = await cityService.AddOrUpdate(cityDto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get<CityDto>(typeof(CityDto).ToString(), res.Data, async () => await cityService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<CityDto>(typeof(CityDto).ToString(), id, async () => await cityService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<CityDto>(typeof(CityDto).ToString(), async (int[] idArr) => await cityService.GetList(idArr));
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
            var res = await redisCache.Remove<bool>(typeof(CityDto).ToString(), id, async () => await cityService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
