using BusinessLogicService.User;
using Contracts.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(string user, string pass)
        {
            var res = await userService.LoginAsync(user, pass);
            if(res.ResultCode != 200)
                return BadRequest(res);

            return Ok(res);
        }

        [HttpGet("OnlySA")]
        [Authorize]
        [RoleAuth(UserPermission.SA)]
        public async Task<IActionResult> GetOnlySA()
        {
            return Ok("Hello World");
        }

        [HttpGet("OnlyVendor")]
        [Authorize]
        [ModuleAuth(ModulePermission.VENDORMASTER)]
        public async Task<IActionResult> GetOnlyVendor()
        {
            return Ok("Hello World");
        }

        [HttpGet("VendorAndBase")]
        [Authorize]
        [ModuleAuth(ModulePermission.VENDORMASTER, ModulePermission.BASE)]
        public async Task<IActionResult> GetVendorAndBase()
        {
            return Ok("Hello World");
        }

        [HttpGet("BothAuth")]
        [Authorize]
        [RoleAuth(UserPermission.SA)]
        [ModuleAuth(ModulePermission.VENDORMASTER, ModulePermission.BASE)]
        public async Task<IActionResult> GetBothAuth()
        {
            return Ok("Hello World");
        }
    }
}
