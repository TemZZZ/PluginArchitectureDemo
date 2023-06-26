using System;
using System.Windows.Input;

namespace WpfLibrary
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action<object> action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> action)
            : this(action, null) { }

        public RelayCommand(Action action, Func<bool> canExecute)
            : this(_ => action.Invoke(), canExecute) { }

        public RelayCommand(Action action)
            : this(action, null) { }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null
                ? true
                : _canExecute.Invoke();
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}