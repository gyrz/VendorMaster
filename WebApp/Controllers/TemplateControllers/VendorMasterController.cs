using BusinessLogicService.Services;
using BusinessLogicService.Services.AddressSvc;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers.TemplateControllers
{
    public class VendorMasterController<T, T2> : BaseController<T, T2>
    {
        public VendorMasterController(IBase<T, T2> service, IRedisCache redisCache) : base(service, redisCache)
        {
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public override async Task<IActionResult> AddOrUpdate(T dto)
        {
            return await base.AddOrUpdate(dto);
        }

        [HttpDelete("{id}")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public override async Task<IActionResult> Remove(int id)
        {
            return await base.Remove(id);
        }
    }
}
