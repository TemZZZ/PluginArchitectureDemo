using System.Globalization;
using System.Threading;

namespace Tools
{
    /// <summary>
    /// Вспомогательный класс для переключения языковых настроек.
    /// </summary>
    public static class LanguageSwitcher
    {
        /// <summary>
        /// Переключает язык графического интерфейса потока.
        /// </summary>
        /// <param name="thread">Поток.</param>
        /// <param name="languageCode">Двухбуквенный (ru) или пятисимвольный (en-US) код языка.</param>
        public static void SwitchUILanguage(Thread thread, string languageCode)
        {
            var cultureInfo = new CultureInfo(languageCode);
            thread.CurrentUICulture = cultureInfo;
        }
    }
}