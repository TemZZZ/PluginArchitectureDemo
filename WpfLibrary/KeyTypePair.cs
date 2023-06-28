using System;

namespace WpfLibrary
{
    /// <summary>
    /// Пара значений "строковый ключ" и "тип".
    /// </summary>
    public class KeyTypePair
    {
        /// <summary>
        /// Строковый ключ.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public Type Type { get; set; }
    }
}