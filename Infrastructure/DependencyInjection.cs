using Application.SecurityService;
using Application.Services.FileService;
using Domain.Interfaces.Repositories;
using Infrastructure.Repos;
using Infrastructure.SecurityService;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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

            return services;

        }


    }
}
