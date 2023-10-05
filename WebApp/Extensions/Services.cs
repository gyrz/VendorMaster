using AutoMapper;
using BusinessLogicService;
using BusinessLogicService.Logger;
using BusinessLogicService.Services.AddressSvc;
using BusinessLogicService.Services.BankAccountSvc;
using BusinessLogicService.Services.CitySvc;
using BusinessLogicService.Services.CountrySvc;
using BusinessLogicService.Services.EmailSvc;
using BusinessLogicService.Services.PersonSvc;
using BusinessLogicService.Services.PhoneSvc;
using BusinessLogicService.Services.UserSvc;
using BusinessLogicService.Services.VendorSvc;
using BusinessLogicService.Services.ZipSvc;
using DataAccess.Cache;
using StackExchange.Redis;

namespace VendorMaster.Extensions
{
    public static class Services
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPhoneService, PhoneService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IZipService, ZipService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("localhost:6379"));
            services.AddSingleton<IMapper>(x => (new AutoMapperCfg()).InitializeAutomapper());
            services.AddScoped<IRedisCache, RedisCache>();
        }
    }
}
