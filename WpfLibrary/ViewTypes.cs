using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfLibrary
{
    /// <summary>
    /// Список типов представлений.
    /// </summary>
    public class ViewTypes : ObservableCollection<KeyTypePair>
    {
        /// <summary>
        /// Преобразует список типов представлений в словарь.
        /// Ключ словаря - строковый ключ представления, значение - тип представления.
        /// </summary>
        /// <returns>Словарь, ключ которого - строковый ключ представления,
        /// а значение - тип представления.</returns>
        public Dictionary<string, Type> ToDictionary()
        {
            var dictionary = new Dictionary<string, Type>(Count);
            foreach (var keyTypePair in this)
            {
                dictionary[keyTypePair.Key] = keyTypePair.Type;
            }

            return dictionary;
        }
    }
}