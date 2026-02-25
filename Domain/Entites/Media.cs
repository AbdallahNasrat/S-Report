using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    internal class Media
    {
        public int ID { get; set; }
        public string FilePath { get; set; }

        //Relationships
        public int ReportID { get; set; }
    }
}
