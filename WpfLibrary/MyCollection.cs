using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfLibrary
{
    public class MyCollection<T>
        : Collection<T>, INotifyPropertyChanged, INotifyCollectionChanged, IRevertibleChangeTracking
    {
        private readonly List<(NotifyCollectionChangedAction Action, int Index, T Item)> _actionsHistory
            = new List<(NotifyCollectionChangedAction, int, T)>();

        private bool _isChanged;

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

            _actionsHistory.Clear();
            IsChanged = false;
        }

        /// <inheritdoc/>
        public void RejectChanges()
        {
            if (!IsChanged)
            {
                return;
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

        private void InsertItemInternal(int index, T item)
        {
            UpdateActionsHistory(NotifyCollectionChangedAction.Add, index, item);
            UpdateIsChangedProperty();
        }

        private void RemoveItemInternal(int index)
        {
            var item = Items[index];
            UpdateActionsHistory(NotifyCollectionChangedAction.Remove, index, item);
            UpdateIsChangedProperty();
        }

        private void UpdateIsChangedProperty()
        {
            IsChanged = _actionsHistory.Count > 0;
        }

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
    }
}