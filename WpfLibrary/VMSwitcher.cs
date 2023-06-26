using System.ComponentModel;
using System.Windows.Input;

namespace WpfLibrary
{
    public class VMSwitcher : VMBase
    {
        private readonly IVMProvider _vmProvider;
        private INotifyPropertyChanged _currentVM;
        private string _vmKey;

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

        public string VMKey
        {
            get => _vmKey;
            private set => SetValue(ref _vmKey, value);
        }

        public ICommand SwitchCommand { get; }

        private void Switch(object parameter)
        {
            if (parameter == null
                || parameter.GetType() != typeof(string))
            {
                return;
            }

            var vmKey = (string)parameter;
            if (_vmProvider.ContainsKey(vmKey))
            {
                VMKey = vmKey;
                CurrentVM = _vmProvider.GetVM(vmKey);
            }
        }
    }
}