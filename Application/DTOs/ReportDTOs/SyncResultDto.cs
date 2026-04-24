public class SyncResultDto
{
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public List<FailedReportInfo> FailedReports { get; set; } = new();
}