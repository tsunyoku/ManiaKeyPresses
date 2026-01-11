using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Avalonia.Styling;

namespace ManiaKeyPresses.UI;

public static class GlobalConfig
{
    private static readonly string ManiaKeyPressesFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ManiaKeyPresses");

    public static readonly string BeatmapPath = Path.Combine(ManiaKeyPressesFolder, "beatmaps");

    private static readonly string ConfigFile = Path.Combine(ManiaKeyPressesFolder, "config.json");

    public static string? OsuClientId { get; private set; }

    public static string? OsuClientSecret { get; private set; }

    public static ThemeVariant Theme { get; private set; } = null!;

    public static void Load()
    {
        if (!Directory.Exists(ManiaKeyPressesFolder))
            Directory.CreateDirectory(ManiaKeyPressesFolder);

        if (!File.Exists(ConfigFile))
            return;

        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigFile));

        if (config is null)
            return;

        OsuClientId = config.OsuClientId;
        OsuClientSecret = config.OsuClientSecret;
        Theme = config.Theme ?? ThemeVariant.Default;
    }

    public static void UpdateOsuClientId(string? clientId)
    {
        OsuClientId = clientId;

        NotifyPropertyChanged(nameof(OsuClientId));
        UpdateConfigFile();
    }
    
    public static void UpdateOsuClientSecret(string? clientSecret)
    {
        OsuClientSecret = clientSecret;
        
        NotifyPropertyChanged(nameof(OsuClientSecret));
        UpdateConfigFile();
    }

    public static void UpdateTheme(ThemeVariant theme)
    {
        Theme = theme;

        NotifyPropertyChanged(nameof(Theme));
        UpdateConfigFile();
    }

    private static void UpdateConfigFile()
    {
        var config = new Config
        {
            OsuClientId = OsuClientId,
            OsuClientSecret = OsuClientSecret,
            Theme = Theme,
        };

        File.WriteAllText(ConfigFile, JsonSerializer.Serialize(config));
    }
        
    public static event PropertyChangedEventHandler? PropertyChanged;
    
    private static void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }
}

file class Config
{
    public string? OsuClientId { get; init; }
    
    public string? OsuClientSecret { get; init; }
    
    public ThemeVariant? Theme { get; init; }
}