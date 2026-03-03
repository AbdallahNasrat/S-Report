using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Media
    {
        public int Id { get; set; }
        
        public string FilePath { get; set; }

        //Relationships
        public int ReportId { get; set; }
        public Report Report { get; set; }
    }
}
