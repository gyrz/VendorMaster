using BusinessLogicService.Services.PhoneSvc;
using Contracts;
using Contracts.Dto.Phone;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/phone")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.BASE)]
    public class PhoneController : Controller
    {
        private readonly IPhoneService phoneService;
        private readonly IRedisCache redisCache;

        public PhoneController(IPhoneService phoneService, IRedisCache redisCache)
        {
            this.phoneService = phoneService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public async Task<IActionResult> AddOrUpdate(PhoneDto phoneDto)
        {
            var res = await phoneService.AddOrUpdate(phoneDto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get<PhoneDto>(typeof(PhoneDto).ToString(), res.Data, async () => await phoneService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<PhoneDto>(typeof(PhoneDto).ToString(), id, async () => await phoneService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<PhoneDto>(typeof(PhoneDto).ToString(), async (int[] idArr) => await phoneService.GetList(idArr));
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
            var res = await redisCache.Remove<PhoneDto>(typeof(PhoneDto).ToString(), id, async () => await phoneService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
