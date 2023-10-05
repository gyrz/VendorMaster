using BusinessLogicService.Services;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers.TemplateControllers
{
    public class BaseController<T, T2> : Controller
    {
        protected readonly IBaseService<T, T2> service;
        protected readonly IRedisCache redisCache;
        public BaseController(IBaseService<T, T2> service, IRedisCache redisCache)
        {
            this.service = service;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public virtual async Task<IActionResult> AddOrUpdate(T dto)
        {
            var res = await service.AddOrUpdate(dto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get(typeof(T2).ToString(), res.Data, async () => await service.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get(typeof(T2).ToString(), id, async () => await service.Get(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public virtual async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked(typeof(T2).ToString(), async (idArr) => await service.GetList(idArr));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public virtual async Task<IActionResult> Remove(int id)
        {
            var res = await redisCache.Remove<T2>(typeof(T2).ToString(), id, async () => await service.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
