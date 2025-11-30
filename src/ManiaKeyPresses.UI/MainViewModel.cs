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
    
    public string? CurrentReplayPath { get; private set; }

    public bool HasReplay => !string.IsNullOrWhiteSpace(CurrentReplayPath);

    public void UpdateReplay(string? replayPath)
    {
        CurrentReplayPath = replayPath;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentReplayPath)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasReplay)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}