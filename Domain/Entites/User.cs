using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class User
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public Genders Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Volunteer { get; set; }
        public decimal Rate { get; set; }
        public string? FcmToken { get; set; }
        public bool IsNotificationEnabled { get; set; } = true; 


        //Relationship
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public Employee EmployeeProfile { get; set; }



        //volunteer Module
        public int TotalPoints { get; set; } = 0;
        public string? Avatar { get; set; }
        public ICollection<ReportVolunteer> Missions { get; set; }
        public ICollection<PointsLog> PointsHistory { get; set; }
        public ICollection<UserAchievement> UserAchievements { get; set; }


    }
}
