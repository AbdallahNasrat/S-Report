using Domain.Entites;

public class ReportAnalysis
{
    public int Id { get; set; }
    public int ReportId { get; set; }

    public double? ConfidenceScore { get; set; }
  
    public DateTime? AssignedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public Report Report { get; set; } = null!;
}