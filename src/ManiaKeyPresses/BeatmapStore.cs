using System.Net.Http.Headers;
using System.Text.Json;
using ManiaKeyPresses.Models;

namespace ManiaKeyPresses;

internal class BeatmapStore
{
    private readonly string _osuClientId;
    private readonly string _osuClientSecret;

    private readonly string _beatmapCache;
    
    private string? _accessToken;

    public BeatmapStore(string osuClientId, string osuClientSecret, string? beatmapCacheDirectory = null)
    {
        _osuClientId = osuClientId;
        _osuClientSecret = osuClientSecret;
        _beatmapCache = beatmapCacheDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "beatmaps");

        if (!Directory.Exists(_beatmapCache))
            Directory.CreateDirectory(_beatmapCache);
    }

    public byte[] GetBeatmapOsuFile(string checksum)
    {
        var beatmapPath = Path.Combine(_beatmapCache, checksum);
        
        if (File.Exists(beatmapPath))
            return File.ReadAllBytes(beatmapPath);

        var beatmapId = GetBeatmapId(checksum);

        var osuFile = GetOsuFile(beatmapId);
        
        File.WriteAllBytes(beatmapPath, osuFile);

        return osuFile;
    }

    private static byte[] GetOsuFile(int beatmapId)
    {
        using var httpClient = new HttpClient();
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://osu.ppy.sh/osu/{beatmapId}");

        var response = httpClient.Send(request);

        response.EnsureSuccessStatusCode();
        
        using var stream = response.Content.ReadAsStream();
        using var memoryStream = new MemoryStream();

        stream.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }

    private int GetBeatmapId(string checksum)
    {
        var accessToken = GetAccessToken();
        
        using var httpClient = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://osu.ppy.sh/api/v2/beatmaps/lookup?checksum={checksum}");
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = httpClient.Send(request);

        response.EnsureSuccessStatusCode();
        
        using var jsonResponse = response.Content.ReadAsStream();
        var beatmap = JsonSerializer.Deserialize<Beatmap>(jsonResponse);
        
        return beatmap!.Id;
    }

    private string GetAccessToken()
    {
        if (_accessToken is not null)
            return _accessToken;

        var request = new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");

        request.Content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("client_id", _osuClientId),
            new KeyValuePair<string, string>("client_secret", _osuClientSecret),
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("scope", "public"),
        ]);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        using var httpClient = new HttpClient();
        var response = httpClient.Send(request);
        
        response.EnsureSuccessStatusCode();

        using var jsonResponse = response.Content.ReadAsStream();
        var accessToken = JsonSerializer.Deserialize<OsuOAuthResponse>(jsonResponse);

        _accessToken = accessToken!.AccessToken;

        return accessToken.AccessToken;
    }
}