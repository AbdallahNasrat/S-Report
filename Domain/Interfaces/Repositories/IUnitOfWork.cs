using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IUnitOfWork  :IDisposable
    {
        public Task<int> SaveChangesAsync();

        //Specific repos
        public IUserRepository UsersRepo { get; }
        public IReportRepository ReportsRepo { get; }
        public IEmployeeRepository EmployeesRepo { get; }
        public INotificationRepository NotificationRepo { get;  }


        //Generic Repos

        public IGenericRepository<City> CitiesRepo { get;}
        public IGenericRepository<Media> MediaRepo { get;}
        public IGenericRepository<ReportType> ReportTypeRepo { get;}
        public IGenericRepository<Role> RoleRepo { get;}
        public IGenericRepository<Team> TeamRepo { get;}
        public IGenericRepository<ReportAnalysis> ReportAnalysisRepo { get;}
        public IGenericRepository<ReportCategory> ReportCategoryRepo { get;}

        public IGenericRepository<ReportVolunteer> ReportVolunteersRepo { get; }
        public IGenericRepository<PointsLog> PointsLogsRepo { get; }
        public IGenericRepository<UserAchievement> UserAchievementsRepo { get; }





    }
}
