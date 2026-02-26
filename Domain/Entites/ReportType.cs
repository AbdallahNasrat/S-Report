using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class ReportType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Relationships
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();

    }
}
