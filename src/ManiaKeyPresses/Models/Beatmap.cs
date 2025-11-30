using System.Text.Json.Serialization;

namespace ManiaKeyPresses.Models;

public class Beatmap
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
}