using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using tiktok_organizer.ViewModels;
using tiktok_organizer.Views;

namespace tiktok_organizer
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                (desktop.MainWindow as MainWindow).AfterInit();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
