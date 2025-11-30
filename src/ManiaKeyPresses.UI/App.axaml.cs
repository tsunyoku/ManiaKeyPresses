using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using osu.Framework.Logging;

namespace ManiaKeyPresses.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        GlobalConfig.Load();
        Logger.Enabled = false;

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}