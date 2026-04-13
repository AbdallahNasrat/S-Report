using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Repositories;
using Infrastructure.Repos;
using Application.Services.FileService;
using Domain.Services;


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

            return services;

        }


    }
}
