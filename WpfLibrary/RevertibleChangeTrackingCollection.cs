using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfLibrary
{
    /// <summary>
    /// Коллекция, позволяющая откатить или принять изменения, произведенные с ней,
    /// а также изменения свойств элементов, принадлежащих этой коллекции.
    /// </summary>
    /// <typeparam name="T">Тип элементов коллекции. Чтобы отслеживать изменения свойств,
    /// элементы должны реализовывать интерфейс <see cref="INotifyPropertyChanged"/>.</typeparam>
    public class RevertibleChangeTrackingCollection<T> :
        Collection<T>,
        INotifyCollectionChanged,
        INotifyPropertyChanged,
        IRevertibleChangeTracking
    {
        /// <summary>
        /// История добавления и удаления элементов коллекции.
        /// В списке хранятся произведенное действие (добавление, удаление), индекс,
        /// по которому было произведено действие, и ссылка на добавленный или удаленный элемент.
        /// </summary>
        private readonly List<(NotifyCollectionChangedAction Action, int Index, T Item)>
            _actionsHistory = new List<(NotifyCollectionChangedAction, int, T)>();

        /// <summary>
        /// История изменения значений свойств элементов.
        /// Ключом словаря является ссылка на элемент коллекции и имя свойства.
        /// Значение словаря содержит старое и текущее значения свойства.
        /// </summary>
        private readonly
            Dictionary<(T Item, string PropertyName), (object OldValue, object CurrentValue)>
            _propertiesChangesHistory = new Dictionary<(T, string), (object, object)>();

        /// <summary>
        /// Имена отслеживаемых свойств.
        /// </summary>
        private readonly string[] _trackingProperties;

        /// <summary>
        /// Реализует ли тип элементов коллекции интерфейс <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        private readonly bool _isItemTypeNotifier;

        /// <summary>
        /// Имеются ли в коллекции изменения.
        /// </summary>
        private bool _isChanged;

        /// <summary>
        /// Создает объект коллекции, позволяющей откатить или принять изменения,
        /// произведенные с ней, а также изменения свойств элементов, принадлежащих этой коллекции.
        /// </summary>
        /// <param name="trackingProperties">Имена отслеживаемых свойств. Может быть null.
        /// Имена могут быть переданы только если тип элементов коллекции
        /// реализует интерфейс <see cref="INotifyPropertyChanged"/>.</param>
        public RevertibleChangeTrackingCollection(params string[] trackingProperties)
        {
            if (trackingProperties == null)
            {
                _trackingProperties = Array.Empty<string>();
                return;
            }

            CheckItemType();
            foreach (var propertyName in trackingProperties)
            {
                CheckProperty(propertyName);
            }

            _isItemTypeNotifier = true;
            _trackingProperties = trackingProperties;
        }

        /// <summary>
        /// Создает объект коллекции, позволяющей откатить или принять изменения,
        /// произведенные с ней, а также изменения свойств элементов, принадлежащих этой коллекции.
        /// </summary>
        public RevertibleChangeTrackingCollection()
            : this(null) { }

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public bool IsChanged
        {
            get => _isChanged;
            set => SetProperty(ref _isChanged, value);
        }

        /// <inheritdoc/>
        public void AcceptChanges()
        {
            if (!IsChanged)
            {
                return;
            }

            foreach (var key in _propertiesChangesHistory.Keys.ToList())
            {
                var value = _propertiesChangesHistory[key].CurrentValue;
                _propertiesChangesHistory[key] = (value, value);
            }

            _actionsHistory.Clear();
            UnsubscribeItems();
            IsChanged = false;
        }

        /// <inheritdoc/>
        public void RejectChanges()
        {
            if (!IsChanged)
            {
                return;
            }

            foreach (var key in _propertiesChangesHistory.Keys.ToList())
            {
                var value = _propertiesChangesHistory[key].OldValue;
                _propertiesChangesHistory[key] = (value, value);
            }

            var i = _actionsHistory.Count - 1;
            while (i >= 0)
            {
                var (action, index, item) = _actionsHistory[i];
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Items.RemoveAt(index);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        Items.Insert(index, item);
                        break;
                }

                i--;
            }

            _actionsHistory.Clear();
            UnsubscribeItems();
            IsChanged = false;
        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, T item)
        {
            InsertItemInternal(index, item);
            CollectionChanged?.Invoke(
                this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
            base.InsertItem(index, item);
        }

        /// <inheritdoc/>
        protected override void RemoveItem(int index)
        {
            RemoveItemInternal(index);
            CollectionChanged?.Invoke(
                this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            base.RemoveItem(index);
        }

        /// <inheritdoc/>
        protected override void SetItem(int index, T item)
        {
            RemoveItemInternal(index);
            InsertItemInternal(index, item);
            CollectionChanged?.Invoke(
                this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace));
            base.SetItem(index, item);
        }

        /// <inheritdoc/>
        protected override void ClearItems()
        {
            if (Count == 0)
            {
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                RemoveItemInternal(i);
            }

            CollectionChanged?.Invoke(
                this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            base.ClearItems();
        }

        /// <summary>
        /// Проверяет тип элементов коллекции. Тип элементов коллекции
        /// должен реализовывать интерфейс <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        /// <exception cref="ArgumentException">Если тип элементов коллекции
        /// не реализует интерфейс <see cref="INotifyPropertyChanged"/>.</exception>
        private static void CheckItemType()
        {
            if (typeof(T).GetInterface(nameof(INotifyPropertyChanged)) == null)
            {
                throw new ArgumentException(
                    $"Item type does not implement {nameof(INotifyPropertyChanged)} interface.");
            }
        }

        /// <summary>
        /// Проверяет свойство элемента коллекции по имени.
        /// Свойство должно существовать, а также иметь геттер и сеттер.
        /// </summary>
        /// <param name="propertyName">Имя свойство.</param>
        /// <exception cref="ArgumentException">Если свойство не существует,
        /// иле не имеет либо геттера, либо сеттера.</exception>
        private static void CheckProperty(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException(
                    $"The type not contain property with name \"{propertyName}\".");
            }

            if (!propertyInfo.CanRead)
            {
                throw new ArgumentException($"The property \"{propertyName}\" has not getter.");
            }

            if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException($"The property \"{propertyName}\" has not setter.");
            }
        }

        /// <summary>
        /// Обновляет историю добавления/удаления элементов коллекции.
        /// </summary>
        /// <param name="action">Действие (добавление или удаление).</param>
        /// <param name="index">Индекс, по которому произведено действие.</param>
        /// <param name="item">Ссылка на добавляемый или удаляемый элемент.</param>
        private void UpdateActionsHistory(NotifyCollectionChangedAction action, int index, T item)
        {
            var oppositeAction = NotifyCollectionChangedAction.Remove;
            if (action == NotifyCollectionChangedAction.Remove)
            {
                oppositeAction = NotifyCollectionChangedAction.Add;
            }

            var oppositeRecordIndex = _actionsHistory.FindLastIndex(record =>
                record.Action == oppositeAction
                && record.Index == index
                && EqualityComparer<T>.Default.Equals(record.Item, item));

            if (oppositeRecordIndex < 0)
            {
                _actionsHistory.Add((action, index, item));
            }
            else
            {
                _actionsHistory.RemoveAt(oppositeRecordIndex);
            }
        }

        /// <summary>
        /// Добавляет элемент в коллекцию.
        /// </summary>
        /// <param name="index">Индекс.</param>
        /// <param name="item">Элемент.</param>
        private void InsertItemInternal(int index, T item)
        {
            if (item != null
                && !Items.Contains(item))
            {
                foreach (var propertyName in _trackingProperties)
                {
                    var value = typeof(T).GetProperty(propertyName).GetValue(item);
                    _propertiesChangesHistory[(item, propertyName)] = (value, value);
                }

                if (_isItemTypeNotifier)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += OnItemPropertyChanged;
                }
            }

            UpdateActionsHistory(NotifyCollectionChangedAction.Add, index, item);
            UpdateIsChangedProperty();
        }

        /// <summary>
        /// Удаляет элемент из коллекции по его индексу в коллекции.
        /// </summary>
        /// <param name="index">Индекс.</param>
        private void RemoveItemInternal(int index)
        {
            var item = Items[index];
            if (item != null
                && _isItemTypeNotifier)
            {
                ((INotifyPropertyChanged)item).PropertyChanged -= OnItemPropertyChanged;
            }

            UpdateActionsHistory(NotifyCollectionChangedAction.Remove, index, item);
            UpdateIsChangedProperty();
        }

        /// <summary>
        /// Обновляет значение свойства <see cref="IsChanged"/>.
        /// </summary>
        private void UpdateIsChangedProperty()
        {
            if (_actionsHistory.Count > 0)
            {
                IsChanged = true;
                return;
            }

            foreach (var (oldValue, currentValue) in _propertiesChangesHistory.Values)
            {
                if (!EqualityComparer<object>.Default.Equals(oldValue, currentValue))
                {
                    IsChanged = true;
                    return;
                }
            }

            IsChanged = false;
        }

        /// <summary>
        /// Устанавливает значение полю, и, если новое значение отличается от текущего,
        /// бросает событие <see cref="PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="TField">Тип поля.</typeparam>
        /// <param name="field">Поле.</param>
        /// <param name="value">Новое значение.</param>
        /// <param name="propertyName">Имя свойства, с которым связано поле.</param>
        private void SetProperty<TField>(
            ref TField field, TField value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, value))
            {
                return;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Обработчик события <see cref="PropertyChanged"/> элемента коллекции.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_trackingProperties.Contains(e.PropertyName))
            {
                return;
            }

            var item = (T)sender;
            var propertyName = e.PropertyName;
            var newValue = typeof(T).GetProperty(propertyName).GetValue(item);
            var (oldValue, _) = _propertiesChangesHistory[(item, propertyName)];
            _propertiesChangesHistory[(item, propertyName)] = (oldValue, newValue);
            UpdateIsChangedProperty();
        }

        /// <summary>
        /// Отписывает элементы коллекции от обработчика <see cref="OnItemPropertyChanged"/>.
        /// </summary>
        private void UnsubscribeItems()
        {
            if (_isItemTypeNotifier)
            {
                foreach (var item in Items)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= OnItemPropertyChanged;
                }
            }
        }
    }
}