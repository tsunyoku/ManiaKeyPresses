using System.Text.Json.Serialization;

namespace OsuKeyPresses.Models;

public class ScoreAnalysis
{
    [JsonPropertyName("hold_times")]
    public required ExportableHoldTime[][] HoldTimes { get; init; }
    
    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ExportableUser? User { get; init; }
    
    [JsonPropertyName("score")]
    public required ExportableScore Score { get; init; }
}