using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class ReportAnalysisConfiguration : IEntityTypeConfiguration<ReportAnalysis>
    {
        public void Configure(EntityTypeBuilder<ReportAnalysis> builder)
        {
            builder.HasIndex(a => a.ReportId)
            .IsUnique();
            builder.HasOne(a => a.Report)
            .WithOne(r => r.ReportAnalysis) 
            .HasForeignKey<ReportAnalysis>(a => a.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.ReportId)
                .IsRequired();

            builder.Property(a => a.ReportType)
                .HasMaxLength(100)
                .IsRequired(false); 

            builder.Property(a => a.ReportPriority)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.Recomendations) 
                .HasMaxLength(1000) 
                .IsRequired(false);

            // ٤. إعدادات الـ ConfidenceScore
            builder.Property(a => a.ConfidenceScore)
                .HasPrecision(5, 2); // مثلاً 0.95 أو 95.50


            builder.Property(a => a.AssignedAt)
                .IsRequired(false);

            builder.Property(a => a.ResolvedAt)
                .IsRequired(false);
        }
    }
}
