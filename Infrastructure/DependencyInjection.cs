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

            var aiSettings = configuration.GetSection("AIServer");
            services.AddHttpClient<IAIService, AIService>(client =>
            {
                client.BaseAddress = new Uri(aiSettings["Url"]);
                // إضافة التوكن في الـ Header بشكل دائم للـ Client ده
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", aiSettings["Token"]);

                client.Timeout = TimeSpan.FromMinutes(3);
            });

            return services;

        }


    }
}
