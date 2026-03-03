using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //Relationships

             //one team  with many employees
            builder.HasOne(e => e.Team)
                .WithMany(t => t.Employees)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
            



            //one role  with many employees
            builder.HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);


            // properties Configurations
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.NationalId).IsUnique();
            builder.HasIndex(p => p.Email).IsUnique();

            builder.Property(p => p.NationalId).HasMaxLength(14).IsFixedLength();
            builder.Property(p => p.Phone).HasMaxLength(11).IsFixedLength();
            builder.Property(p => p.Gender).HasConversion<string>().HasMaxLength(10);
            builder.Property(p => p.FName).HasMaxLength(100);
            builder.Property(p => p.SName).HasMaxLength(100);
            builder.Property(p => p.Email).HasMaxLength(200);
            builder.Property(p => p.Password).HasMaxLength(100);
            builder.Property(p => p.Address).HasMaxLength(300);



        }
    }
}
