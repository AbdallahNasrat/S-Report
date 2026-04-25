using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<IEnumerable<Report>> GetFilteredReportsAsync( int pageNumber, int pageSize, bool excludeResolved, int? cityId = null);
        public Task<Report?> GetReportWithDetailsAsync(int id,bool tracked=true);
        public Task<IEnumerable<Report>> GetReportsByUserIdAsync(int userId, int pageNumber, int pageSize);

    }
}
