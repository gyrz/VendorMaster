using BusinessLogicService.Logger;

namespace VendorMaster.Extensions
{
    public class ExceptionLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionLogger(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        private string GetFullMethodName(HttpContext context)
        {
            var controller = context.GetRouteValue("controller")?.ToString();
            var action = context.GetRouteValue("action")?.ToString();
            return $"{controller}.{action}";
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(GetFullMethodName(context) + " - " + ex.ToString());

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Unexpected exception | Check logs for more information!");
            }
        }
    }
}
