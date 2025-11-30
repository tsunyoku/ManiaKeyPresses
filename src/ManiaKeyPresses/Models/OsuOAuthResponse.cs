using System.Text.Json.Serialization;

namespace ManiaKeyPresses.Models;

public class OsuOAuthResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }
    
    [JsonPropertyName("expires_in")]
    public required long ExpiresIn { get; init; }
}