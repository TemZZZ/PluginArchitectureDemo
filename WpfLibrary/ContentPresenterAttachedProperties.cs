using System.Windows.Controls;
using System.Windows;

namespace WpfLibrary
{
    /// <summary>
    /// Прикрепляемые свойства к объектам класса ContentPresenter (представитель контента).
    /// </summary>
    public static class ContentPresenterAttachedProperties
    {
        /// <summary>
        /// Свойство зависимости ключа контента.
        /// </summary>
        public static readonly DependencyProperty ContentKeyProperty
            = DependencyProperty.RegisterAttached(
                "ContentKey",
                typeof(object),
                typeof(ContentPresenterAttachedProperties),
                new PropertyMetadata(OnContentKeyChanged));

        /// <summary>
        /// Свойство зависимости поставщика представлений по ключу.
        /// </summary>
        public static readonly DependencyProperty ViewByKeySelectorProperty
            = DependencyProperty.RegisterAttached(
                "ViewByKeySelector",
                typeof(DataTemplateSelector),
                typeof(ContentPresenterAttachedProperties));

        /// <summary>
        /// Геттер ключа контента.
        /// </summary>
        /// <param name="contentPresenter">Представитель котнента.</param>
        /// <returns>Текущий ключ контента.</returns>
        public static object GetContentKey(ContentPresenter contentPresenter)
        {
            return contentPresenter.GetValue(ContentKeyProperty);
        }

        /// <summary>
        /// Сеттер ключа контента.
        /// </summary>
        /// <param name="contentPresenter">Представитель котнента.</param>
        /// <param name="value">Новое значение ключа контента.</param>
        public static void SetContentKey(ContentPresenter contentPresenter, object value)
        {
            contentPresenter.SetValue(ContentKeyProperty, value);
        }

        /// <summary>
        /// Геттер поставщика представлений по ключу.
        /// </summary>
        /// <param name="contentPresenter">Представитель котнента.</param>
        /// <returns>Текущий поставщик представлений.</returns>
        public static DataTemplateSelector GetViewByKeySelector(ContentPresenter contentPresenter)
        {
            return (DataTemplateSelector)contentPresenter.GetValue(ViewByKeySelectorProperty);
        }

        /// <summary>
        /// Сеттер поставщика представлений.
        /// </summary>
        /// <param name="contentPresenter">Представитель котнента.</param>
        /// <param name="value">Новый объект поставщика представлений.</param>
        public static void SetViewByKeySelector(
            ContentPresenter contentPresenter, DataTemplateSelector value)
        {
            contentPresenter.SetValue(ViewByKeySelectorProperty, value);
        }

        /// <summary>
        /// Обработчик события изменения ключа контента. Передает новый ключ поставщику представлений,
        /// а поставщик представлений выдает представление, соответствующее этому ключу.
        /// Выданное представление отображается представителем контента.
        /// </summary>
        /// <param name="d">Источник события (в нормальном случае - это объект представителя контента).</param>
        /// <param name="e">Дополнительная информация,
        /// передаваемая вместе с событием (в этом методе не используется).</param>
        private static void OnContentKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d.GetType() != typeof(ContentPresenter))
            {
                return;
            }

            var contentPresenter = (ContentPresenter)d;
            var selector = (DataTemplateSelector)contentPresenter.GetValue(ViewByKeySelectorProperty);
            if (selector == null)
            {
                return;
            }

            var contentKey = contentPresenter.GetValue(ContentKeyProperty);
            contentPresenter.ContentTemplate = selector.SelectTemplate(contentKey, contentPresenter);
        }
    }
}