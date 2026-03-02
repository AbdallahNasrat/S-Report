using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            //Relationship
            //Many notifications with one employee
            builder.HasOne(n => n.Employee)
                .WithMany(e => e.Notifications)
                .HasForeignKey(n => n.EmployeeId);

            //Many notifications to one user
            builder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);
            //many notification to many cities
            builder.HasMany(n => n.Cities)
                .WithMany(c => c.Notifications)
                .UsingEntity(t => t.ToTable("CitiesNotifications"));
        }
    }
}
