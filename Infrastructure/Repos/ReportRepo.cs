using Domain.Entites;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
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
        //assistant function
        private IQueryable<Report> GetReportsQuery()
        {
            return _context.Reports
                .Include(r => r.User)
                .Include(r => r.Medias)
                .Include(r => r.City)
                .Include(r => r.Team)
                .AsNoTracking();
        }
        public ReportRepo(Context context) : base(context)
        {           
        }

        public async Task<IEnumerable<Report>> GetFilteredReportsAsync(int cityId, int pageNumber, int pageSize, bool excludeResolved)
        {
            IQueryable<Report> query = GetReportsQuery()
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

        public async Task<Report?> GetReportWithDetailsAsync(int id)
        {
            var report = await _context.Reports
                .Include(r => r.User)
                .Include(r => r.Team)
                .Include(r => r.City)
                .Include(r => r.Medias).AsNoTracking().FirstOrDefaultAsync(r => r.Id==id);
            return report;
        }
        public async Task<IEnumerable<Report>> GetReportsByUserIdAsync(int userId, int pageNumber, int pageSize) {
            var reports = GetReportsQuery()
                .Where(r => r.Id==userId)
                .OrderByDescending(r => r.Id)
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
            return await reports;


        }








    }
}
