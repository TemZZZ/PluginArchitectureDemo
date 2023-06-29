using System;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfLibrary
{
    /// <summary>
    /// Переключатель вью-моделей.
    /// </summary>
    public class VMSwitcher : VMBase
    {
        /// <summary>
        /// Поставщик вью-моделей.
        /// </summary>
        private readonly IVMProvider _vmProvider;

        /// <summary>
        /// Текущая вью-модель.
        /// </summary>
        private INotifyPropertyChanged _currentVM;

        /// <summary>
        /// Ключ текущей вью-модели.
        /// </summary>
        private string _currentKey;

        /// <summary>
        /// Создает объект переключателя вью-моделей.
        /// </summary>
        /// <param name="vmProvider">Поставщик вью-моделей.</param>
        public VMSwitcher(IVMProvider vmProvider)
        {
            _vmProvider = vmProvider;
            SwitchCommand = new RelayCommand(Switch);
        }

        /// <summary>
        /// Событие, возникающее перед переключением вью-моделей.
        /// </summary>
        public event EventHandler<VMSwitchingEventArgs> VMSwitching;

        /// <summary>
        /// Событие, возникающее после переключения вью-моделей.
        /// </summary>
        public event EventHandler<VMSwitchedEventArgs> VMSwitched;

        /// <summary>
        /// Текущая вью-модель.
        /// </summary>
        public INotifyPropertyChanged CurrentVM
        {
            get => _currentVM;
            private set => SetValue(ref _currentVM, value);
        }

        /// <summary>
        /// Ключ текущей вью-модели.
        /// </summary>
        public string CurrentKey
        {
            get => _currentKey;
            private set => SetValue(ref _currentKey, value);
        }

        /// <summary>
        /// Команда переключения вью-моделей. Принимает в качестве параметра ключ вью-модели.
        /// </summary>
        public ICommand SwitchCommand { get; }

        /// <summary>
        /// Переключает вью-модель по ключу.
        /// </summary>
        /// <param name="parameter">Ключ вью-модели.</param>
        private void Switch(object parameter)
        {
            if (parameter == null
                || parameter.GetType() != typeof(string))
            {
                return;
            }

            var key = (string)parameter;
            if (!_vmProvider.ContainsKey(key))
            {
                return;
            }

            var oldVM = _currentVM;
            var newVM = _vmProvider.GetVM(key);
            var eventArgs = new VMSwitchingEventArgs(oldVM, newVM);
            VMSwitching?.Invoke(this, eventArgs);
            if (eventArgs.Handled)
            {
                return;
            }

            CurrentKey = key;
            CurrentVM = newVM;
            VMSwitched?.Invoke(this, new VMSwitchedEventArgs(oldVM, newVM));
        }
    }
}