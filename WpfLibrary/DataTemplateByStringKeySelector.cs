using System.Windows.Controls;
using System.Windows;

namespace WpfLibrary
{
    public class DataTemplateByStringKeySelector : DataTemplateSelector
    {
        private DataTemplate _currentTemplate;

        public TemplateTypes TemplateTypes { get; set; } = new TemplateTypes();

        /// <inheritdoc/>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null
                || item.GetType() != typeof(string)
                || TemplateTypes == null)
            {
                return _currentTemplate;
            }

            var templateTypesDictionary = TemplateTypes.ToDictionary();
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