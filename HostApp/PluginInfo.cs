using System;

namespace HostApp
{
    /// <summary>
    /// Информация о плагине.
    /// </summary>
    internal class PluginInfo
    {
        /// <summary>
        /// Создает объект информации о плагине.
        /// </summary>
        /// <param name="name">Имя плагина.</param>
        /// <param name="description">Описание плагина.</param>
        /// <param name="type">Тип данных объекта плагина.</param>
        /// <param name="dllPath">Путь до файла библиотеки плагина.</param>
        public PluginInfo(string name, string description, Type type, string dllPath)
        {
            Name = name;
            Description = description;
            Type = type;
            DllPath = dllPath;
        }

        /// <summary>
        /// Имя плагина.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Описание плагина.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Тип данных объекта плагина.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Путь до файла библиотеки плагина.
        /// </summary>
        public string DllPath { get; }
    }
}