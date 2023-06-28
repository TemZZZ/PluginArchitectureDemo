using System.Collections.Generic;
using System.ComponentModel;

namespace WpfLibrary
{
    /// <summary>
    /// Интерфейс класса поставщика вью-моделей.
    /// </summary>
    public interface IVMProvider
    {
        /// <summary>
        /// Ключи вью-моделей, зарегистрированных у поставщика.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Возвращает вью-модель по ключу.
        /// </summary>
        /// <param name="key">Ключ вью-модели.</param>
        /// <returns>Объект вью-модели.</returns>
        INotifyPropertyChanged GetVM(string key);

        /// <summary>
        /// Проверяет, зарегистрирован ли ключ у поставщика.
        /// </summary>
        /// <param name="key">Проверяемый ключ.</param>
        /// <returns>true, если ключ зарегистрирован у поставщика, иначе - false.</returns>
        bool ContainsKey(string key);
    }
}