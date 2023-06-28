namespace Core
{
    /// <summary>
    /// Интерфейс фасадного класса, который используется для доступа к содержимого плагина.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Уникальный ключ плагина.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Имя плагина.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание плагина.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Возвращает XAML словарь ресурсов в виде строки.
        /// </summary>
        /// <returns>XAML словарь ресурсов в виде строки.</returns>
        string GetXamlResourceDictionary();
    }
}