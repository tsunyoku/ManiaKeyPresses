using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Avalonia.Styling;

namespace OsuKeyPresses.UI;

public static class GlobalConfig
{
    private static readonly string OsuKeyPressesFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ManiaKeyPresses");

    public static readonly string BeatmapPath = Path.Combine(OsuKeyPressesFolder, "beatmaps");

    private static readonly string ConfigFile = Path.Combine(OsuKeyPressesFolder, "config.json");
    
    public static readonly string LogFile = Path.Combine(OsuKeyPressesFolder, "runtime.log");

    public static string? OsuClientId { get; private set; }

    public static string? OsuClientSecret { get; private set; }

    public static string Theme { get; private set; } = "Dark";

    public static long LocalId { get; private set; } = 1;
    
    public static bool IsDarkMode => Theme == "Dark";

    public static void Load()
    {
        if (!Directory.Exists(OsuKeyPressesFolder))
            Directory.CreateDirectory(OsuKeyPressesFolder);

        if (!File.Exists(ConfigFile))
            return;

        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigFile));

        if (config is null)
            return;

        OsuClientId = config.OsuClientId;
        OsuClientSecret = config.OsuClientSecret;

        if (config.ThemeName is not null)
            Theme = config.ThemeName;

        if (config.LocalId is not null)
            LocalId = config.LocalId!.Value;
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
        Theme = (string)theme.Key;

        NotifyPropertyChanged(nameof(Theme));
        UpdateConfigFile();
    }

    public static void UpdateLocalId(long localId)
    {
        LocalId = localId;
        
        NotifyPropertyChanged(nameof(LocalId));
        UpdateConfigFile();
    }

    private static void UpdateConfigFile()
    {
        var config = new Config
        {
            OsuClientId = OsuClientId,
            OsuClientSecret = OsuClientSecret,
            ThemeName = Theme,
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
    
    public string? ThemeName { get; init; }
    
    public long? LocalId { get; init; }
}