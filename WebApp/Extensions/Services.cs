using AutoMapper;
using BusinessLogicService;
using BusinessLogicService.Logger;
using BusinessLogicService.Services.CitySvc;
using BusinessLogicService.Services.CountrySvc;
using BusinessLogicService.Services.UserSvc;
using BusinessLogicService.Services.ZipSvc;
using DataAccess.Cache;
using StackExchange.Redis;

namespace VendorMaster.Extensions
{
    public static class Services
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IZipService, ZipService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("localhost:6379"));
            services.AddSingleton<IMapper>(x => (new AutoMapperCfg()).InitializeAutomapper());
            services.AddScoped<IRedisCache, RedisCache>();
        }
    }
}
