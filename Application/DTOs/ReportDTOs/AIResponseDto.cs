using System.Text.Json.Serialization;

public class AIResponseDto
{
    [JsonPropertyName("batch_results")]
    public List<BatchResult> BatchResults { get; set; } = new();
}

public class BatchResult
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("priority")]
    public string Priority { get; set; } = string.Empty;

    [JsonPropertyName("recommendation")]
    public Recommendation Recommendation { get; set; } = new();

    [JsonPropertyName("visual_analysis")]
    public string? VisualAnalysisBase64 { get; set; } // الصورة المرسومة (Base64)
}

public class Recommendation
{
    [JsonPropertyName("units")]
    public Dictionary<string, int> Units { get; set; } = new();

    [JsonPropertyName("action_plan")]
    public string ActionPlan { get; set; } = string.Empty;
}