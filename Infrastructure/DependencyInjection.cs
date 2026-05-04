using Application.SecurityService;
using Application.Services.AiServices;
using Application.Services.FileService;
using Application.Services.NotificationServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Notifications;
using Infrastructure.Repos;
using Infrastructure.SecurityService;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration) {
            var connectionStr = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Context>(options => options.UseSqlServer(connectionStr));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepo<>));
            services.AddScoped<IUserRepository,UserRepo>();
            services.AddScoped<IReportRepository,ReportRepo>();
            services.AddScoped<IEmployeeRepository,EmployeeRepo>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<INotificationRepository, NotificationRepo>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
            services.AddScoped<INotificationService, NotificationService>();


            //AI Models

            // 1. إعداد سيرفر الـ AI بتاع الصور (هنديله اسم "ImageAIServer")
            var aiSettings = configuration.GetSection("AIServer");
            services.AddHttpClient("ImageAIServer", client =>
            {
                client.BaseAddress = new Uri(aiSettings["Url"]);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", aiSettings["Token"]);
                client.Timeout = TimeSpan.FromMinutes(3);
            });

            // 2. إعداد سيرفر الـ AI بتاع الصوت والنصوص (هنديله اسم "VoiceTextAIServer")
            var aiSettings2 = configuration.GetSection("AIServer2");
            services.AddHttpClient("VoiceTextAIServer", client =>
            {
                client.BaseAddress = new Uri(aiSettings2["Url"]); // ✅ صلحنا البج هنا
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", aiSettings2["Token"]); // ✅ وهنا كمان
                client.Timeout = TimeSpan.FromMinutes(3);
            });

            // 3. تسجيل الـ AIService نفسه مرة واحدة بس كـ Scoped أو Transient
            services.AddScoped<IAIService, AIService>();

            return services;

        }


    }
}
