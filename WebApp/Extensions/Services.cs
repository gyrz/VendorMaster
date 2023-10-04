using BusinessLogicService.Logger;
using BusinessLogicService.User;

namespace VendorMaster.Extensions
{
    public static class Services
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
