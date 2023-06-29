using System;
using System.ComponentModel;

namespace WpfLibrary
{
    /// <summary>
    /// Аргументы события, возникающего после переключения вью-моделей.
    /// </summary>
    public class VMSwitchedEventArgs : EventArgs
    {
        /// <summary>
        /// Создает объект аргументов события, возникающего после переключения вью-моделей.
        /// </summary>
        /// <param name="oldVM">Предыдущая вью-модель.</param>
        /// <param name="currentVM">Текущая вью-модель.</param>
        public VMSwitchedEventArgs(INotifyPropertyChanged oldVM, INotifyPropertyChanged currentVM)
        {
            OldVM = oldVM;
            CurrentVM = currentVM;
        }

        /// <summary>
        /// Предыдущая вью-модель.
        /// </summary>
        public INotifyPropertyChanged OldVM { get; }

        /// <summary>
        /// Текущая вью-модель.
        /// </summary>
        public INotifyPropertyChanged CurrentVM { get; }
    }
}