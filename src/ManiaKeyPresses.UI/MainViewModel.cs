using System.ComponentModel;

namespace ManiaKeyPresses.UI;

public class MainViewModel : INotifyPropertyChanged
{
    public MainViewModel()
    {
        GlobalConfig.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName is nameof(GlobalConfig.OsuClientId) or nameof(GlobalConfig.OsuClientSecret))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOsuConfigured)));
            }
        };
    }

    public bool IsOsuConfigured => !string.IsNullOrEmpty(GlobalConfig.OsuClientId) &&
                                   !string.IsNullOrEmpty(GlobalConfig.OsuClientSecret);
    
    public string? CurrentReplayFileName { get; private set; }

    public bool HasReplay => !string.IsNullOrWhiteSpace(CurrentReplayFileName);

    public bool IsDarkMode { get; private set; }

    public void UpdateReplay(string? replayFileName)
    {
        CurrentReplayFileName = replayFileName;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentReplayFileName)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasReplay)));
    }

    public void UpdateIsDarkMode(bool isDarkMode)
    {
        IsDarkMode = isDarkMode;
        
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDarkMode)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}