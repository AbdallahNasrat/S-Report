using System.Net;
using System.Text.Json;

namespace SReport.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // لو الريكويست سليم، كمل طريقك عادي
                await _next(context);
            }
            catch (Exception ex)
            {
                // لو حصل أي إيرور في أي مكان في السيستم، هنمسكه هنا
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // لو إحنا في بيئة التطوير (Local) بنعرض تفاصيل الإيرور، لو في السيرفر (Production) بنخفيها
                object response; // بنعرفه كـ object عام

                if (_env.IsDevelopment())
                {
                    // هنا بياخد 3 خصائص
                    response = new { StatusCode = context.Response.StatusCode, Message = ex.Message, Details = ex.StackTrace?.ToString() };
                }
                else
                {
                    // هنا بياخد خاصيتين براحته خالص
                    response = new { StatusCode = context.Response.StatusCode, Message = "An internal server error occurred, it is being worked on." };
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}