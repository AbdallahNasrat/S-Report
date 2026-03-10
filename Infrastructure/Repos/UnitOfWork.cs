using Domain.Entites;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private Context _context;
        private IServiceProvider _serviceProvider;
        public UnitOfWork( Context context , IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IUserRepository UsersRepo { get => _serviceProvider.GetRequiredService<IUserRepository>();}
        public IReportRepository ReportsRepo { get => _serviceProvider.GetRequiredService<IReportRepository>(); }
        public IEmployeeRepository EmployeesRepo { get => _serviceProvider.GetRequiredService<IEmployeeRepository>();}
        public INotificationRepository NotificationRepo { get => _serviceProvider.GetRequiredService<INotificationRepository>();}

        public IGenericRepository<City> CitiesRepo => _serviceProvider.GetRequiredService<IGenericRepository<City>>();

        public IGenericRepository<Media> MediaRepo => _serviceProvider.GetRequiredService<IGenericRepository<Media>>();

        public IGenericRepository<ReportType> ReportTypeRepo => _serviceProvider.GetRequiredService<IGenericRepository<ReportType>>();

        public IGenericRepository<Role> RoleRepo => _serviceProvider.GetRequiredService<IGenericRepository<Role>>();

        public IGenericRepository<Team> TeamRepo => _serviceProvider.GetRequiredService<IGenericRepository<Team>>();

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();

        }
    }
}
