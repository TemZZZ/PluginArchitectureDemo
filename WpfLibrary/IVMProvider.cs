using System.Collections.Generic;
using System.ComponentModel;

namespace WpfLibrary
{
    public interface IVMProvider
    {
        IEnumerable<string> Keys { get; }
        INotifyPropertyChanged GetVM(string key);
        bool ContainsKey(string key);
    }
}