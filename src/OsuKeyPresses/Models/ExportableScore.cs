using System.Text.Json.Serialization;

namespace OsuKeyPresses.Models;

public class ExportableScore
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Id { get; init; }
    
    [JsonPropertyName("legacy_score_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? LegacyScoreId { get; init; }

    [JsonPropertyName("legacy_total_score")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? LegacyTotalScore { get; init; }
    
    [JsonPropertyName("accuracy")]
    public required double Accuracy { get; init; }
    
    [JsonPropertyName("max_combo")]
    public required int MaxCombo { get; init; }
    
    [JsonPropertyName("ruleset")]
    public required string Ruleset { get; init; }
    
    [JsonPropertyName("statistics")]
    public required ExportableScoreStatistics Statistics { get; init; }
}

public class ExportableScoreStatistics
{
    [JsonPropertyName("perfect_count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PerfectCount { get; init; }
    
    [JsonPropertyName("great_count")]
    public required int GreatCount { get; init; }
    
    [JsonPropertyName("good_count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? GoodCount { get; init; }
    
    [JsonPropertyName("ok_count")]
    public required int OkCount { get; init; }
    
    [JsonPropertyName("meh_count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MehCount { get; init; }
    
    [JsonPropertyName("miss_count")]
    public required int MissCount { get; init; }
}