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
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            //one city with many reports
            builder.HasOne(r => r.City)
                .WithMany(C => C.Reports)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.Restrict);


            //one ReportType with many Reports

            builder.HasOne(r => r.ReportType)
                .WithMany(t => t.Reports)
                .HasForeignKey(r => r.ReportTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            //one team with many reports
            builder.HasOne(r => r.Team)
                .WithMany(t => t.Reports)
                .HasForeignKey(r => r.TeamId)
                .OnDelete(DeleteBehavior.Restrict);


            //properties
            builder.Property(p => p.Longitude).HasPrecision(18, 8);
            builder.Property(p => p.Latitude).HasPrecision(18, 8);
            builder.Property(p => p.Priority).HasConversion<string>().HasMaxLength(10);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.AiResult).HasMaxLength(500);
            builder.Property(p => p.State).HasConversion<string>().HasMaxLength(50);

            




        }
    }
}
