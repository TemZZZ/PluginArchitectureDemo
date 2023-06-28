using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Core;
using Tools;

namespace HostApp
{
    /// <summary>
    /// Загрузчик плагинов.
    /// </summary>
    internal class PluginLoader
    {
        /// <summary>
        /// Собирает информацию обо всех плагинах, расположенных в папке.
        /// </summary>
        /// <param name="pluginsDirectory">Путь до папки с плагинами.</param>
        /// <returns>Перечень информации о плагинах.</returns>
        public List<PluginInfo> GetPluginsInfo(string pluginsDirectory)
        {
            const string dllExtension = ".dll";
            var dllPaths = Directory.GetFiles(pluginsDirectory)
                .Where(path => Path.GetExtension(path) == dllExtension);

            var pluginsInfo = new List<PluginInfo>();
            foreach (var path in dllPaths)
            {
                var dll = AssemblyTools.LoadAssembly(path);
                var pluginTypes = dll.GetTypes().Where(IsPlugin).ToList();
                foreach (var type in pluginTypes)
                {
                    var plugin = (IPlugin)Activator.CreateInstance(type);
                    pluginsInfo.Add(new PluginInfo(plugin.Name, plugin.Description, type, path));
                }

                // TODO Здесь надо бы выгрузить сборку.
                // Начиная с .NET Core 3 можно выгружать сборки, если они больше не нужны.
                // Но в .NET Framework такой возможности нет, поэтому загруженные библиотеки,
                // даже если они больше не используются, будут просто занимать оперативную память.
            }

            return pluginsInfo;
        }

        /// <summary>
        /// Загружает один плагин в домен текущего приложения.
        /// </summary>
        /// <param name="pluginInfo">Объект с информацией о плагине.</param>
        public IPlugin LoadPlugin(PluginInfo pluginInfo)
        {
            var dll = AssemblyTools.LoadAssembly(pluginInfo.DllPath);
            var pluginType = dll.GetTypes().First(type => type == pluginInfo.Type);
            var plugin = (IPlugin)Activator.CreateInstance(pluginType);
            LoadResourceDictionary(Application.Current, plugin);
            return plugin;
        }

        /// <summary>
        /// Проверяет, реализует ли переданный тип интерфейс плагина.
        /// </summary>
        /// <param name="type">Проверяемый тип.</param>
        /// <returns>true, если переданный тип реализует интерфейс плагина, иначе - false.</returns>
        private static bool IsPlugin(Type type)
        {
            return type.GetInterface(typeof(IPlugin).FullName) != null;
        }

        /// <summary>
        /// Выполняет слияние XAML словарей ресурсов плагина и приложения.
        /// </summary>
        /// <param name="application">Объект приложения.</param>
        /// <param name="plugin">Объект плагина.</param>
        private static void LoadResourceDictionary(Application application, IPlugin plugin)
        {
            var dictionaryString = plugin.GetXamlResourceDictionary();
            var resourceDictionary = (ResourceDictionary)XamlReader.Parse(dictionaryString);
            application.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}