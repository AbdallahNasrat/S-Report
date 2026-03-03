using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;

namespace Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Relationships 
            //one role with many users
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);


            //one city with many users
            builder.HasOne(u => u.City)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.Restrict);


            //properties
            builder.HasIndex(p => p.NationalId).IsUnique();
            builder.HasIndex(p => p.Email).IsUnique();
            builder.Property(p => p.NationalId).HasMaxLength(14).IsFixedLength();
            builder.Property(p => p.FName).HasMaxLength(100);
            builder.Property(p => p.SName).HasMaxLength(100);
            builder.Property(p => p.Address).HasMaxLength(300);
            builder.Property(p => p.Email).HasMaxLength(200);
            builder.Property(p => p.Password).HasMaxLength(100);
            builder.Property(p => p.Phone).HasMaxLength(11).IsFixedLength();
            builder.Property(p => p.Gender).HasConversion<string>().HasMaxLength(10);
            builder.Property(p => p.Rate).HasPrecision(3, 2).HasDefaultValue(5.00);
            builder.Property(p => p.Volunteer).HasDefaultValue(true);



        }
    }
}
