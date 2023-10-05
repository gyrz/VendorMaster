using BusinessLogicService.Services.PersonSvc;
using Contracts;
using Contracts.Dto.Person;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/person")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.VENDORMASTER)]
    public class PersonController : Controller
    {
        private readonly IPersonService personService;
        private readonly IRedisCache redisCache;

        public PersonController(IPersonService personService, IRedisCache redisCache)
        {
            this.personService = personService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public async Task<IActionResult> AddOrUpdate(PersonSimpleDto personDto)
        {
            var res = await personService.AddOrUpdate(personDto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get<PersonDto>(typeof(PersonDto).ToString(), res.Data, async () => await personService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<PersonDto>(typeof(PersonDto).ToString(), id, async () => await personService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<PersonDto>(typeof(PersonDto).ToString(), async (int[] idArr) => await personService.GetList(idArr));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpDelete("{id}")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await redisCache.Remove<PersonDto>(typeof(PersonDto).ToString(), id, async () => await personService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
