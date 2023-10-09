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
using VendorMaster.Handlers;

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

            var forced = true;

            TranzactionIterator vendorHandleIterator = new TranzactionIterator(
                forced,
                new VendorTranzaction<AddressSimpleDto, AddressDto>
                    (res.Data, vendorDto.Addresses, addressService, redisCache),
                new VendorTranzaction<PhoneDto, PhoneDto>
                    (res.Data, vendorDto.Phones, phoneService, redisCache),
                new VendorTranzaction<BankAccountDto, BankAccountDto>
                    (res.Data, vendorDto.BankAccounts, bankAccountService, redisCache),
                new VendorTranzaction<PersonSimpleDto, PersonDto>
                    (res.Data, vendorDto.ContactPersons, personService, redisCache),
                new VendorTranzaction<EmailDto, EmailDto>
                    (res.Data, vendorDto.Emails, emailService, redisCache)
                );

            var iterationResult = await vendorHandleIterator.Start();
            res.Message = iterationResult.Item1;

            if(forced && !string.IsNullOrEmpty(iterationResult.Item2))
            {
                res.Message += "Rollback errors: ";
                res.Message += iterationResult.Item2;
            }

            if(forced && !string.IsNullOrEmpty(res.Message))
            {
                await service.Remove(res.Data);
                return BadRequest(res);
            }
            else
            {
                redisCache.Get<VendorDto>(typeof(VendorDto).ToString(), res.Data, async () => await service.Get(res.Data));
            }

            return Ok(res);
        }
    }
}
