using System;
using System.Linq;
using System.Reflection;

namespace Tools
{
    /// <summary>
    /// Инструменты для работы со сборками.
    /// </summary>
    public class AssemblyTools
    {
        /// <summary>
        /// Проверяет, загружена ли уже сборка в текущий домен приложения.
        /// Если не загружена, то загружает ее и возвращает объект этой сборки.
        /// Если сборка уже загружена, то возвращает объект уже загруженной сборки.
        /// Сравнение сборок происходит на основании полного имени сборки (assembly.GetName().FullName).
        /// </summary>
        /// <param name="path">Путь до файла сборки.</param>
        /// <returns>Объект загруженной сборки.</returns>
        public static Assembly LoadAssembly(string path)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var targetAssemblyName = AssemblyName.GetAssemblyName(path);
            var existingAssembly = loadedAssemblies.FirstOrDefault(
                assembly => assembly.GetName().FullName == targetAssemblyName.FullName);
            return existingAssembly ?? Assembly.LoadFrom(path);
        }
    }
}