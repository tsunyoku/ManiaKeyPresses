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

    public bool IsOsuConfigured => GlobalConfig.IsOsuConfigured;
    public bool IsReplayLoaded { get; private set; }
    
    public void SetReplayLoaded(bool isLoaded)
    {
        if (IsReplayLoaded == isLoaded)
            return;

        IsReplayLoaded = isLoaded;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsReplayLoaded)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}