using System.Windows.Controls;
using System.Windows;

namespace WpfLibrary
{
    /// <summary>
    /// Поставщик представлений по строковому ключу.
    /// </summary>
    public class ViewByStringKeySelector : DataTemplateSelector
    {
        /// <summary>
        /// Текущий шаблон для представления контента.
        /// </summary>
        private DataTemplate _currentTemplate;

        /// <summary>
        /// Зарегистрированные типы представлений.
        /// </summary>
        public ViewTypes ViewTypes { get; set; } = new ViewTypes();

        /// <inheritdoc/>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null
                || item.GetType() != typeof(string)
                || ViewTypes == null)
            {
                return _currentTemplate;
            }

            var templateTypesDictionary = ViewTypes.ToDictionary();
            var key = (string)item;
            if (!templateTypesDictionary.ContainsKey(key))
            {
                return _currentTemplate;
            }

            var templateType = templateTypesDictionary[key];
            _currentTemplate = new DataTemplate
            {
                VisualTree = new FrameworkElementFactory(templateType)
            };

            return _currentTemplate;
        }
    }
}