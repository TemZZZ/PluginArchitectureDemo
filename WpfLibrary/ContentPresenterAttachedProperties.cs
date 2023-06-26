using System.Windows.Controls;
using System.Windows;

namespace WpfLibrary
{
    public static class ContentPresenterAttachedProperties
    {
        public static readonly DependencyProperty ContentKeyProperty
            = DependencyProperty.RegisterAttached(
                "ContentKey",
                typeof(object),
                typeof(ContentPresenterAttachedProperties),
                new PropertyMetadata(OnContentKeyChanged));

        public static readonly DependencyProperty ContentByKeySelectorProperty
            = DependencyProperty.RegisterAttached(
                "ContentByKeySelector",
                typeof(DataTemplateSelector),
                typeof(ContentPresenterAttachedProperties));

        public static object GetContentKey(ContentPresenter contentPresenter)
        {
            return contentPresenter.GetValue(ContentKeyProperty);
        }

        public static void SetContentKey(ContentPresenter contentPresenter, object value)
        {
            contentPresenter.SetValue(ContentKeyProperty, value);
        }

        public static DataTemplateSelector GetContentByKeySelector(ContentPresenter contentPresenter)
        {
            return (DataTemplateSelector)contentPresenter.GetValue(ContentByKeySelectorProperty);
        }

        public static void SetContentByKeySelector(
            ContentPresenter contentPresenter, DataTemplateSelector value)
        {
            contentPresenter.SetValue(ContentByKeySelectorProperty, value);
        }

        private static void OnContentKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d.GetType() != typeof(ContentPresenter))
            {
                return;
            }

            var contentPresenter = (ContentPresenter)d;
            var selector = (DataTemplateSelector)contentPresenter.GetValue(ContentByKeySelectorProperty);
            if (selector == null)
            {
                return;
            }

            var contentKey = contentPresenter.GetValue(ContentKeyProperty);
            contentPresenter.ContentTemplate = selector.SelectTemplate(contentKey, contentPresenter);
        }
    }
}