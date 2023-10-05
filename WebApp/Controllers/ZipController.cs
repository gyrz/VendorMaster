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

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/zip")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class ZipController : Controller
    {
        private readonly IZipService zipService;
        private readonly IRedisCache redisCache;

        public ZipController(IZipService zipService, IRedisCache redisCache)
        {
            this.zipService = zipService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public async Task<IActionResult> AddOrUpdate(ZipSimpleDto zipDto)
        {
            var res = await zipService.AddOrUpdate(zipDto);
            if (res.ResultCode == 400)
                return BadRequest(res);
            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get<ZipDto>(typeof(ZipDto).ToString(), res.Data, async () => await zipService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<ZipDto>(typeof(ZipDto).ToString(), id, async () => await zipService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<ZipDto>(typeof(ZipDto).ToString(), async (int[] idArr) => await zipService.GetList(idArr));
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
            var res = await redisCache.Remove<ZipDto>(typeof(ZipDto).ToString(), id, async () => await zipService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
