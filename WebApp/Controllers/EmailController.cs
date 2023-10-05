using BusinessLogicService.Services.EmailSvc;
using Contracts;
using Contracts.Dto.Email;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Controllers.TemplateControllers;
using VendorMaster.Extensions;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/email")]
    [Authorize]
    [RoleAuth(UserPermission.SA)]
    [ModuleAuth(ModulePermission.BASE)]
    public class EmailController : BaseController<EmailDto, EmailDto>
    {
        public EmailController(IEmailService emailService, IRedisCache redisCache) : base(emailService, redisCache)
        {
        }
    }
}
