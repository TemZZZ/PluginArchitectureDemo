using System;
using System.Collections.Generic;
using System.ComponentModel;
using WpfLibrary;

namespace HostApp
{
    internal class MainVMProvider : IVMProvider
    {
        private const string TemplatesKey = "TemplatesKey";
        private const string SettingsKey = "SettingsKey";

        private readonly Dictionary<string, Func<INotifyPropertyChanged>> _vms
            = new Dictionary<string, Func<INotifyPropertyChanged>>
            {
                [TemplatesKey] = () => new TemplatesVM(),
                [SettingsKey] = () => new SettingsVM(),
            };

        /// <inheritdoc/>
        public IEnumerable<string> Keys => new List<string>
        {
            TemplatesKey,
            SettingsKey
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