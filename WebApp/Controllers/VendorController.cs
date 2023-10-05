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

namespace VendorMaster.Controllers
{
    [ApiController]
    [Route("api/vendor")]
    [Authorize]
    [RoleAuth(UserPermission.MGR)]
    [ModuleAuth(ModulePermission.VENDORMASTER)]
    public class VendorController : Controller
    {
        private readonly IVendorService vendorService;
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
            IPhoneService phoneService)
        {
            this.vendorService = vendorService;
            this.redisCache = redisCache;
            this.addressService = addressService;
            this.personService = personService;
            this.emailService = emailService;
            this.bankAccountService = bankAccountService;
            this.phoneService = phoneService;
        }

        [HttpPost("")]
        [HasWritePermissionForModule(ModulePermission.VENDORMASTER)]
        public async Task<IActionResult> AddOrUpdate(VendorDto vendorDto)
        {
            // Add or update vendor first
            var res = await vendorService.AddOrUpdate(vendorDto);
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

            redisCache.Get<VendorDto>(typeof(VendorDto).ToString(), res.Data, async () => await vendorService.Get(res.Data));

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await redisCache.Get<VendorDto>(typeof(VendorDto).ToString(), id, async () => await vendorService.Get(id));
            if(res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var res = await redisCache.GetListUntracked<VendorDto>(typeof(VendorDto).ToString(), async (int[] idArr) => await vendorService.GetList(idArr));
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
            var res = await redisCache.Remove<VendorDto>(typeof(VendorDto).ToString(), id, async () => await vendorService.Remove(id));
            if (res.ResultCode == 400)
                return BadRequest(res);

            if (res.ResultCode == 404)
                return NotFound(res);

            return Ok(res);
        }
    }
}
