using AutoMapper;
using Contracts.Dto.Address;
using Contracts.Dto.BankAccount;
using Contracts.Dto.City;
using Contracts.Dto.Country;
using Contracts.Dto.Email;
using Contracts.Dto.Person;
using Contracts.Dto.Phone;
using Contracts.Dto.Vendor;
using Contracts.Dto.Zip;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService
{
    public class AutoMapperCfg
    {
        public Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<Address, AddressSimpleDto>();
                cfg.CreateMap<AddressSimpleDto, Address>();

                cfg.CreateMap<BankAccount, BankAccountDto>();
                cfg.CreateMap<BankAccountDto, BankAccount>();

                cfg.CreateMap<City, CityDto>();
                cfg.CreateMap<CitySimpleDto, City>();

                cfg.CreateMap<Country, CountryDto>();
                cfg.CreateMap<CountryDto, Country>();

                cfg.CreateMap<Email, EmailDto>();
                cfg.CreateMap<EmailDto, Email>();

                cfg.CreateMap<Person, PersonDto>();
                cfg.CreateMap<Person, PersonSimpleDto>();
                cfg.CreateMap<PersonSimpleDto, Person>();

                cfg.CreateMap<Phone, PhoneDto>();
                cfg.CreateMap<PhoneDto, Phone>();

                cfg.CreateMap<Vendor, VendorDto>();
                cfg.CreateMap<VendorSimpleDto, Vendor>();
                cfg.CreateMap<VendorDto, VendorSimpleDto>();

                cfg.CreateMap<Zip, ZipDto>();
                cfg.CreateMap<ZipSimpleDto, Zip>();

            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
