using BusinessLogicService.Services.AddressSvc;
using BusinessLogicService.Services.CitySvc;
using Contracts;
using Contracts.Dto.Address;
using Contracts.Dto.City;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/address")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.BASE)]
    public class AddressController : Controller
    {
        private readonly IAddressService addressService;
        private readonly IRedisCache redisCache;

        public AddressController(IAddressService addressService, IRedisCache redisCache)
        {
            this.addressService = addressService;
            this.redisCache = redisCache;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.BASE)]
        public async Task<IActionResult> AddOrUpdate(AddressSimpleDto addressDto)
        {
            var res = await addressService.AddOrUpdate(addressDto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            redisCache.Get<AddressDto>(typeof(AddressDto).ToString(), res.Data, async () => await addressService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<AddressDto>(typeof(AddressDto).ToString(), id, async () => await addressService.Get(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<AddressDto>(typeof(AddressDto).ToString(), async (int[] idArr) => await addressService.GetList(idArr));
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
            var res = await redisCache.Remove<AddressDto>(typeof(AddressDto).ToString(), id, async () => await addressService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
