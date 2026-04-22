using Domain.Entites;

public class ReportAnalysis
{
    public int Id { get; set; }
    public int ReportId { get; set; }


    public string? ReportType { get; set; }
    public string? ReportPriority { get; set; }

    public double? ConfidenceScore { get; set; }
    public string? Recomendations { get; set; }
    public string? ConvertedVoiceText { get; set; }

    //public string? ActionPlan { get; set; }      // عمود جديد لـ "action_plan"
   // public string? ProposedUnits { get; set; }   // عمود جديد لتخزين الـ "units" كـ String أو JSON
    //public string? AIImagePath { get; set; }    // عمود جديد لمسار الصورة اللي الـ AI رسم عليها

    public DateTime? AssignedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public Report Report { get; set; } = null!;
}