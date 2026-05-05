using Domain.Exceptions;
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
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";

                // القاموس السحري
                var statusCodesMapping = new Dictionary<Type, int>
                {
                    { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
                    { typeof(KeyNotFoundException), StatusCodes.Status404NotFound },
                    { typeof(ArgumentException), StatusCodes.Status400BadRequest },
                    { typeof(ArgumentNullException), StatusCodes.Status400BadRequest },
                    { typeof(InvalidOperationException), StatusCodes.Status400BadRequest },
                    { typeof(Microsoft.EntityFrameworkCore.DbUpdateException), StatusCodes.Status409Conflict },
                    { typeof(NotFoundException), StatusCodes.Status404NotFound}
                };

                // لو الإيرور متسجل هياخد رقمه، لو مش متسجل هياخد 500
                int finalStatusCode = statusCodesMapping.ContainsKey(ex.GetType())
                    ? statusCodesMapping[ex.GetType()]
                    : StatusCodes.Status500InternalServerError;

                context.Response.StatusCode = finalStatusCode;

                // الرسالة: لو 500 وبرودكشن هنخفيها، غير كده هنرجع رسالتك اللي إنت كاتبها
                string finalMessage = finalStatusCode == 500 && !_env.IsDevelopment()
                    ? "حدث خطأ داخلي في الخادم، جاري العمل عليه."
                    : ex.Message;

                var response = new { StatusCode = finalStatusCode, Message = finalMessage };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }
    }
}