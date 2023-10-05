using AutoMapper;
using Contracts.Dto.Address;
using Contracts.Dto.BankAccount;
using Contracts.Dto.Email;
using Contracts.Dto.Person;
using Contracts.Dto.Phone;
using Contracts.Dto.Vendor;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.Services.VendorSvc
{
    public class VendorMapper
    {
        private readonly IMapper mapper;

        public VendorMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public VendorDto ToDto(Vendor vendor)
        {
            var dto = new VendorDto();
            dto.Id = vendor.Id;
            dto.Name = vendor.Name;
            dto.Name2 = vendor.Name2;
            dto.Notes = vendor.Notes;

            dto.Addresses = vendor.Addresses.Select(x => mapper.Map<AddressSimpleDto>(x));
            dto.BankAccounts = vendor.BankAccounts.Select(x => mapper.Map<BankAccountDto>(x));
            dto.ContactPersons = vendor.ContactPersons.Select(x => mapper.Map<PersonSimpleDto>(x));
            dto.Phones = vendor.Phones.Select(x => mapper.Map<PhoneDto>(x));
            dto.Emails = vendor.Emails.Select(x => mapper.Map<EmailDto>(x));


            return dto;
        }
    }
}
