using BusinessLogicService.Services.AddressSvc;
using BusinessLogicService.Services.CitySvc;
using BusinessLogicService.Services.PersonSvc;
using Contracts;
using Contracts.Dto.Address;
using Contracts.Dto.City;
using Contracts.Dto.Enums;
using Contracts.Dto.Person;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Controllers.TemplateControllers;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/address")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.BASE)]
    public class AddressController : BaseController<AddressSimpleDto, AddressDto>
    {
        public AddressController(IAddressService addressService, IRedisCache redisCache) : base(addressService, redisCache)
        {
        }
    }
}
