using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Core;

namespace HostApp
{
    internal class PluginLoader
    {
        public List<PluginInfo> GetPluginsInfo(string pluginsDirectory)
        {
            var dllPaths = Directory.GetFiles(pluginsDirectory)
                .Where(path => Path.GetExtension(path) == ".dll");

            var pluginsInfo = new List<PluginInfo>();
            foreach (var path in dllPaths)
            {
                var dll = Assembly.LoadFile(path);
                var pluginTypes = dll.GetTypes().Where(IsPlugin).ToList();
                foreach (var type in pluginTypes)
                {
                    var plugin = (IPlugin)Activator.CreateInstance(type);
                    pluginsInfo.Add(new PluginInfo(plugin.Name, plugin.Description, type, path));
                }
            }

            return pluginsInfo;
        }

        public void LoadPlugin(Application application, PluginInfo pluginInfo)
        {
            var dll = Assembly.LoadFile(pluginInfo.DllPath);
            var pluginType = dll.GetTypes().First(type => type == pluginInfo.Type);
            var plugin = (IPlugin)Activator.CreateInstance(pluginType);

            LoadResourceDictionary(application, plugin);
        }

        private static bool IsPlugin(Type type)
        {
            return type.GetInterface(nameof(IPlugin)) != null;
        }

        private static void LoadResourceDictionary(Application application, IPlugin plugin)
        {
            var dictionaryString = plugin.GetXamlResourceDictionary();
            var resourceDictionary = (ResourceDictionary)XamlReader.Parse(dictionaryString);
            application.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}