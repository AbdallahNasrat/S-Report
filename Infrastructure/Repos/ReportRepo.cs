using Domain.Entites;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class ReportRepo : GenericRepo<Report>, IReportRepository
    {
        public ReportRepo(Context context) : base(context)
        {           
        }

        public async Task<IEnumerable<Report>> GetFilteredReportsAsync(int cityId, int pageNumber, int pageSize, bool excludeResolved)
        {
            IQueryable<Report> query = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Medias)
                .Include(r => r.City)
                .Include(r => r.Team)
                .Where(r => r.CityId == cityId);

            if (excludeResolved)
            {
                query = query.Where(r => r.State != ReportStatus.Resolved);
            }

            
            return await query
                .OrderByDescending(r => r.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
