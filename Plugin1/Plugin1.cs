using System;
using System.Windows;
using System.Windows.Markup;
using Core;

namespace Plugin1
{
    public class Plugin1 : IPlugin
    {
        /// <inheritdoc/>
        public string Name => "Plugin1";

        /// <inheritdoc/>
        public string Description => "My name is Plugin1";

        /// <inheritdoc/>
        public string GetXamlResourceDictionary()
        {
            var resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri(
                "/Plugin1;component/ResourceDictionary.xaml",
                UriKind.RelativeOrAbsolute);

            return XamlWriter.Save(resourceDictionary);
        }
    }
}