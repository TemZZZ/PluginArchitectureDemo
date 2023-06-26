using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
            const string pluginsDirectory = @"C:\Repos\PluginArchitectureDemo\Plugin1\bin\Debug";
            var loader = new PluginLoader();
            var pluginsInfo = loader.GetPluginsInfo(pluginsDirectory);
            loader.LoadPlugin(Current, pluginsInfo.First());

            base.OnStartup(e);
        }
    }
}