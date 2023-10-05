using BusinessLogicService.Services.VendorSvc;
using Contracts;
using Contracts.Dto.Vendor;
using Contracts.Dto.Enums;
using DataAccess.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorMaster.Extensions;
using BusinessLogicService.Services.AddressSvc;
using Contracts.Dto.Address;
using System.Text;
using BusinessLogicService.Services.PhoneSvc;
using BusinessLogicService.Services.EmailSvc;
using BusinessLogicService.Services.BankAccountSvc;
using BusinessLogicService.Services.PersonSvc;
using Contracts.Dto.Phone;
using Contracts.Dto.BankAccount;
using Contracts.Dto.Person;
using Contracts.Dto.Email;
using VendorMaster.Handlers.VendorHandler;
using VendorMaster.Controllers.TemplateControllers;

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/vendor")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.VENDORMASTER)]
    public class VendorController : VendorMasterController<VendorDto, VendorDto>
    {
        private readonly IAddressService addressService;
        private readonly IPhoneService phoneService;
        private readonly IEmailService emailService;
        private readonly IBankAccountService bankAccountService;
        private readonly IPersonService personService;
        private readonly IRedisCache redisCache;

        public VendorController(
            IVendorService vendorService, 
            IRedisCache redisCache, 
            IAddressService addressService,
            IPersonService personService,
            IEmailService emailService,
            IBankAccountService bankAccountService,
            IPhoneService phoneService) : base(vendorService, redisCache)
        {
            this.redisCache = redisCache;
            this.addressService = addressService;
            this.personService = personService;
            this.emailService = emailService;
            this.bankAccountService = bankAccountService;
            this.phoneService = phoneService;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public override async Task<IActionResult> AddOrUpdate(VendorDto vendorDto)
        {
            // Add or update vendor first
            var res = await service.AddOrUpdate(vendorDto);
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            VendorHandleIterator vendorHandleIterator = new VendorHandleIterator(
                new VendorHandle<AddressSimpleDto, AddressDto>
                    (res.Data, vendorDto.Addresses, addressService, redisCache),
                new VendorHandle<PhoneDto, PhoneDto>
                    (res.Data, vendorDto.Phones, phoneService, redisCache),
                new VendorHandle<BankAccountDto, BankAccountDto>
                    (res.Data, vendorDto.BankAccounts, bankAccountService, redisCache),
                new VendorHandle<PersonSimpleDto, PersonDto>
                    (res.Data, vendorDto.ContactPersons, personService, redisCache),
                new VendorHandle<EmailDto, EmailDto>
                    (res.Data, vendorDto.Emails, emailService, redisCache)
                );

            res.Message = await vendorHandleIterator.Start();

            redisCache.Get<VendorDto>(typeof(VendorDto).ToString(), res.Data, async () => await service.Get(res.Data));

            return Ok(res);
        }
    }
}
