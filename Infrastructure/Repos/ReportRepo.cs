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
                .Include(r => r.ReportAnalysis)
                .AsNoTracking();
        }
        public ReportRepo(Context context) : base(context)
        {           
        }

        public async Task<IEnumerable<Report>> GetFilteredReportsAsync( int pageNumber, int pageSize, bool excludeResolved, int? cityId = null)
        {
            IQueryable<Report> query = GetReportsQuery();
            if (cityId.HasValue) {
                query = query.Where(r => r.CityId == cityId);
            }     

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

        public async Task<Report?> GetReportWithDetailsAsync(int id, bool tracked = true)
        {
            
            var query = _context.Reports
                .Include(r => r.User)
                .Include(r => r.Team)
                .Include(r => r.City)
                .Include(r => r.ReportAnalysis)
                .Include(r => r.Medias)
                .AsQueryable(); // نجهزه كـ Query

            
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Report>> GetReportsByUserIdAsync(int userId, int pageNumber, int pageSize) {
            var reports = GetReportsQuery()
                .Where(r => r.UserId==userId)
                .OrderByDescending(r => r.Id)
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
            return await reports;


        }








    }
}
