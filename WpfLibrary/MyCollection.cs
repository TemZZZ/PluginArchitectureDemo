using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfLibrary
{
    internal class MyCollection<T> : INotifyCollectionChanged, IRevertibleChangeTracking
        where T : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public MyCollection(params string[] propertyNames)
        {

        }

        /// <inheritdoc/>
        public bool IsChanged { get; }

        public void Add(T item)
        {
            CollectionChanged?.Invoke(
                this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        public void Remove(T item)
        {
            CollectionChanged?.Invoke(
                this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
        }

        public void Clear()
        {
            CollectionChanged?.Invoke(
                this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc/>
        public void AcceptChanges()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RejectChanges()
        {
            throw new NotImplementedException();
        }
    }
}