using BusinessLogicService.Services.EmailSvc;
using Contracts;
using Contracts.Dto.Email;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/email")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class EmailController : Controller
    {
        private readonly IEmailService emailService;
        private readonly IRedisCache redisCache;

        public EmailController(IEmailService emailService, IRedisCache redisCache)
        {
            this.emailService = emailService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public async Task<IActionResult> AddOrUpdate(EmailDto emailDto)
        {
            var res = await emailService.AddOrUpdate(emailDto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get<EmailDto>(typeof(EmailDto).ToString(), res.Data, async () => await emailService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<EmailDto>(typeof(EmailDto).ToString(), id, async () => await emailService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<EmailDto>(typeof(EmailDto).ToString(), async (int[] idArr) => await emailService.GetList(idArr));
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
            var res = await redisCache.Remove<EmailDto>(typeof(EmailDto).ToString(), id, async () => await emailService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
