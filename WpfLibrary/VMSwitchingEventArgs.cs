using System;
using System.ComponentModel;

namespace WpfLibrary
{
    /// <summary>
    /// Аргументы события, возникающего перед переключением вью-моделей.
    /// </summary>
    public class VMSwitchingEventArgs : EventArgs
    {
        /// <summary>
        /// Создает объект аргументов события, возникающего перед переключением вью-моделей.
        /// </summary>
        /// <param name="currentVM">Текущая вью-модель.</param>
        /// <param name="newVM">Новая вью-модель.</param>
        public VMSwitchingEventArgs(INotifyPropertyChanged currentVM, INotifyPropertyChanged newVM)
        {
            CurrentVM = currentVM;
            NewVM = newVM;
        }

        /// <summary>
        /// Текущая вью-модель.
        /// </summary>
        public INotifyPropertyChanged CurrentVM { get; }

        /// <summary>
        /// Новая вью-модель.
        /// </summary>
        public INotifyPropertyChanged NewVM { get; }

        /// <summary>
        /// Флаг завершения обработки события.
        /// </summary>
        public bool Handled { get; set; }
    }
}