using System.Linq;
using System.Threading;
using System.Windows;
using Tools;

namespace HostApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            LanguageSwitcher.SwitchUILanguage(Thread.CurrentThread, "ru");

            // Языковые ресурсы программы с GUI (*.resources.dll) загружаются только после создания окна.
            var mainWindow = new MainWindow();

            const string pluginsDirectory = @"C:\Repos\PluginArchitectureDemo\Plugin1\bin\Debug";
            var loader = new PluginLoader();
            var pluginsInfo = loader.GetPluginsInfo(pluginsDirectory);
            loader.LoadPlugin(pluginsInfo.First());

            mainWindow.DataContext = new MainVM();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}