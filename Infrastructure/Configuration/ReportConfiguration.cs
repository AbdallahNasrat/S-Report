using Microsoft.EntityFrameworkCore;
using Domain.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            //Relationships

            //one user with many reports
            builder.HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId);

            //one city with many reports
            builder.HasOne(r => r.City)
                .WithMany(C => C.Reports)
                .HasForeignKey(r => r.CityId);

            //one ReportType with many Reports

            builder.HasOne(r => r.ReportType)
                .WithMany(t => t.Reports)
                .HasForeignKey(r => r.ReportTypeId);

            //one team with many reports
            builder.HasOne(r => r.Team)
                .WithMany(t => t.Reports)
                .HasForeignKey(r => r.TeamId);


        }
    }
}
