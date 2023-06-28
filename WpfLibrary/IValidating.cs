using System.Collections.Generic;

namespace WpfLibrary
{
    /// <summary>
    /// Интерфейс классов, которые имеют механизм валидации значений своих свойств.
    /// </summary>
    public interface IValidating
    {
        /// <summary>
        /// Валиден ли объект (обычно объект валиден, когда валидны значения всех его свойств).
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Возвращает словарь последних ошибок валидации объекта. Ключ - имя свойства,
        /// значение - список ошибок валидации.
        /// </summary>
        /// <returns>Словарь последних ошибок валидации объекта.</returns>
        /// <remarks>Используется именно метод, а не свойство,
        /// чтобы не было соблазна прибиндиться к словарю.</remarks>
        IReadOnlyDictionary<string, List<string>> GetLastErrors();
    }
}