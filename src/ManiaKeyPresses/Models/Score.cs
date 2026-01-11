using System.Text.Json.Serialization;

namespace ManiaKeyPresses.Models;

public class Score
{
    [JsonPropertyName("user_id")]
    public required int UserId { get; init; }
}