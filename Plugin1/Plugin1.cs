﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Core;

namespace Plugin1
{
    public class Plugin1 : IPlugin
    {
        public Plugin1()
        {
            Properties.Strings.Plugin1Strings.Culture = new CultureInfo("ru-RU");

            Name = Properties.Strings.Plugin1Strings.Name;
            Description = Properties.Strings.Plugin1Strings.Description;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Description { get; }

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