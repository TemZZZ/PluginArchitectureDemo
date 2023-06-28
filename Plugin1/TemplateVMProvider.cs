using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfLibrary;

namespace Plugin1
{
    internal class TemplateVMProvider : IVMProvider
    {
        private readonly string TemplateVM1Key = Properties.Strings.Plugin1Strings.Template1Name;
        private readonly string TemplateVM2Key = Properties.Strings.Plugin1Strings.Template2Name;

        private readonly Dictionary<string, Func<INotifyPropertyChanged>> _vms;

        public TemplateVMProvider()
        {
            _vms = new Dictionary<string, Func<INotifyPropertyChanged>>
            {
                [TemplateVM1Key] = () => new TemplateVM1(),
                [TemplateVM2Key] = () => new TemplateVM2()
            };
        }

        /// <inheritdoc/>
        public ObservableCollection<string> Keys => new ObservableCollection<string>
        {
            TemplateVM1Key,
            TemplateVM2Key
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