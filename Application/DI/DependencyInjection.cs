using Application.Services.AdminServices;
using Application.Services.EmployeeServices;
using Application.Services.GeneralServices;
using Application.Services.ReportServices;
using Application.Services.TeamService;
using Application.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services) {
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILookupsService, LookupService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IVolunteerService, VolunteerService>();


            return services;

        }
    }
}
