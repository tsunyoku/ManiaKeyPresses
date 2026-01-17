using System.Text.Json.Serialization;

namespace OsuKeyPresses.Models;

public class ExportableHoldTime
{
    [JsonPropertyName("hold_time")]
    public required int HoldTime { get; init; }
    
    [JsonPropertyName("total_count")]
    public required int TotalCount { get; init; }
}