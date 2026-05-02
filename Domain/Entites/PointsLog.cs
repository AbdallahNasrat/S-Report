using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class PointsLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
