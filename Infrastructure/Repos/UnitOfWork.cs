using Domain.Entites;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context; 
        private readonly IServiceProvider _serviceProvider;
        private bool _disposed = false;

        public UnitOfWork(Context context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

       
        public IUserRepository UsersRepo => _serviceProvider.GetRequiredService<IUserRepository>();
        public IReportRepository ReportsRepo => _serviceProvider.GetRequiredService<IReportRepository>();
        public IEmployeeRepository EmployeesRepo => _serviceProvider.GetRequiredService<IEmployeeRepository>();
        public INotificationRepository NotificationRepo => _serviceProvider.GetRequiredService<INotificationRepository>();

        public IGenericRepository<City> CitiesRepo => _serviceProvider.GetRequiredService<IGenericRepository<City>>();
        public IGenericRepository<Media> MediaRepo => _serviceProvider.GetRequiredService<IGenericRepository<Media>>();
        public IGenericRepository<ReportType> ReportTypeRepo => _serviceProvider.GetRequiredService<IGenericRepository<ReportType>>();
        public IGenericRepository<Role> RoleRepo => _serviceProvider.GetRequiredService<IGenericRepository<Role>>();
        public IGenericRepository<Team> TeamRepo => _serviceProvider.GetRequiredService<IGenericRepository<Team>>();
        public IGenericRepository<ReportCategory> ReportCategoryRepo => _serviceProvider.GetRequiredService<IGenericRepository<ReportCategory>>();

        
        public IGenericRepository<ReportAnalysis> ReportAnalysisRepo => _serviceProvider.GetRequiredService<IGenericRepository<ReportAnalysis>>();

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);            
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // تنظيف الـ Managed Resources (مثل الـ DbContext)
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}