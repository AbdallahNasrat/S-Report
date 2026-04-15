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
        Task<IEnumerable<Report>> GetFilteredReportsAsync(int cityId, int pageNumber, int pageSize, bool excludeResolved);
        public Task<Report?> GetReportWithDetailsAsync(int id);
        public Task<IEnumerable<Report>> GetReportsByUserIdAsync(int userId, int pageNumber, int pageSize);

    }
}
