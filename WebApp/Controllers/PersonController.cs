using BusinessLogicService.Services.PersonSvc;
using Contracts;
using Contracts.Dto.Person;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;
using BusinessLogicService.Services;
using VendorMaster.Controllers.TemplateControllers;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/person")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.VENDORMASTER)]
    public class PersonController : VendorMasterController<PersonSimpleDto, PersonDto>
    {
        public PersonController(IPersonService personService, IRedisCache redisCache) : base(personService, redisCache)
        {
        }
    }
}
