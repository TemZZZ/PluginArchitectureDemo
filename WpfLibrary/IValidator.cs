using System.Collections.Generic;

namespace WpfLibrary
{
    /// <summary>
    /// Интерфейс классов валидаторов.
    /// </summary>
    /// <typeparam name="T">Тип контекста валидации.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Валидирует переданный контекст.
        /// Обычно в качестве контекста выступает объект какого-либо класса.
        /// </summary>
        /// <param name="validationContext">Контекст валидации.</param>
        /// <returns>Словарь ошибок валидации.
        /// Ключ - имя свойства, значение - список ошибок валидации.</returns>
        IReadOnlyDictionary<string, List<string>> Validate(T validationContext);
    }
}