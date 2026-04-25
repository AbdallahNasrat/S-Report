using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class ReportCategoryConfiguration : IEntityTypeConfiguration<ReportCategory>
    {
        public void Configure(EntityTypeBuilder<ReportCategory> builder)
        {
            /* FIRE
            INFRASTRUCTURE_DAMAGE
            WATER_DISASTER
            TRAFFIC_ACCIDENT
            CRIME_SCENE
            HARASSMENT 
            MEDICAL_EMERGENCY 
            NORMAL*/
            builder.HasData(
                new ReportCategory { Id = 1, Name = "FIRE" },
                new ReportCategory { Id = 2, Name = "INFRASTRUCTURE_DAMAGE" },
                new ReportCategory { Id = 3, Name = "WATER_DISASTER" },
                new ReportCategory { Id = 4, Name = "TRAFFIC_ACCIDENT" },
                new ReportCategory { Id = 5, Name = "CRIME_SCENE" },
                new ReportCategory { Id = 6, Name = "HARASSMENT" },
                new ReportCategory { Id = 7, Name = "MEDICAL_EMERGENCY" },
                new ReportCategory { Id = 8, Name = "NORMAL" },

                new ReportCategory { Id = 9, Name = "Accident" },
                new ReportCategory { Id = 10, Name = "Environmental" },
                new ReportCategory { Id = 11, Name = "Health" }
                );            
            }
        }
    }

