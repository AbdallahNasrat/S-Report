using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class Report
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public Priorities Priority { get; set; }
        public string Description { get; set; }
        public ReportStatus State { get; set; }
        public bool IsValid { get; set; }
        public string AiResult { get; set; }


        //Relationship
        public int UserID { get; set; }
        public int CityID { get; set; }
        public int TypeID { get; set; }
        public int TeamID { get; set; }

    }
}
