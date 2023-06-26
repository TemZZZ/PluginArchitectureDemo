using System.ComponentModel;

namespace WpfLibrary
{
    public interface IVMProvider
    {
        INotifyPropertyChanged GetVM(string key);
        bool ContainsKey(string key);
    }
}