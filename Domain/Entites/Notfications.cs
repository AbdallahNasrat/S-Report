using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Notfications
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }

        //Relationship
        public int UserID { get; set; }
        public int RelatedReportID { get; set; }
        public int CityID { get; set; }


    }
}
