using Domain.Entites;
using Domain.Interfaces.Repositories;
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

    }
}
