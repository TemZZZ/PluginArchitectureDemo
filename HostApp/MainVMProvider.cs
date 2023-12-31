﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfLibrary;

namespace HostApp
{
    internal class MainVMProvider : IVMProvider
    {
        private const string TemplatesVMKey = "TemplatesVMKey";
        private const string SettingsVMKey = "SettingsVMKey";

        private readonly Dictionary<string, Func<INotifyPropertyChanged>> _vms;

        public MainVMProvider(IVMProvider templateVMProvider)
        {
            _vms = new Dictionary<string, Func<INotifyPropertyChanged>>
            {
                [TemplatesVMKey] = () => new TemplatesVM(templateVMProvider),
                [SettingsVMKey] = () => new SettingsVM()
            };
        }

        /// <inheritdoc/>
        public ObservableCollection<string> Keys => new ObservableCollection<string>
        {
            TemplatesVMKey,
            SettingsVMKey
        };

        /// <inheritdoc/>
        public INotifyPropertyChanged GetVM(string key)
        {
            return _vms[key].Invoke();
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            return _vms.ContainsKey(key);
        }
    }
}