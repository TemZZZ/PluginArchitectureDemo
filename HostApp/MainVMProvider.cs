using System;
using System.Collections.Generic;
using System.ComponentModel;
using WpfLibrary;

namespace HostApp
{
    internal class MainVMProvider : IVMProvider
    {
        private const string VM1Name = "VM1";
        private const string VM2Name = "VM2";

        private readonly Dictionary<string, Func<INotifyPropertyChanged>> _vms
            = new Dictionary<string, Func<INotifyPropertyChanged>>
            {
                [VM1Name] = () => new VM1(),
                [VM2Name] = () => new VM2(),
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