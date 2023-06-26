using System.ComponentModel;
using System.Windows.Input;

namespace WpfLibrary
{
    public class VMSwitcher : VMBase
    {
        private readonly IVMProvider _vmProvider;
        private INotifyPropertyChanged _currentVM;
        private string _key;

        public VMSwitcher(IVMProvider vmProvider)
        {
            _vmProvider = vmProvider;
            SwitchCommand = new RelayCommand(Switch);
        }

        public INotifyPropertyChanged CurrentVM
        {
            get => _currentVM;
            private set => SetValue(ref _currentVM, value);
        }

        public string Key
        {
            get => _key;
            private set => SetValue(ref _key, value);
        }

        public ICommand SwitchCommand { get; }

        private void Switch(object parameter)
        {
            if (parameter == null
                || parameter.GetType() != typeof(string))
            {
                return;
            }

            var key = (string)parameter;
            if (_vmProvider.ContainsKey(key))
            {
                Key = key;
                CurrentVM = _vmProvider.GetVM(key);
            }
        }
    }
}