using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public enum MediaType
    {
        Image = 1,
        Audio = 2,
        Video = 3
    }
    public class Media
    {
        public int Id { get; set; }
        
        public string FilePath { get; set; }
        public MediaType Type { get; set; }


        //Relationships
        public int ReportId { get; set; }
        public Report Report { get; set; }
    }
}
