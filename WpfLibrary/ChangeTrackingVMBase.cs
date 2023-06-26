using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfLibrary
{
    internal class ChangeTrackingVMBase : VMBase, IRevertibleChangeTracking
    {
        private readonly Dictionary<(object Owner, Delegate Setter), object> _changes
            = new Dictionary<(object, Delegate), object>();

        private bool _isChanged;

        /// <inheritdoc/>
        public bool IsChanged
        {
            get => _isChanged;
            private set => SetValue(ref _isChanged, value);
        }

        /// <inheritdoc/>
        public void AcceptChanges()
        {
            foreach (var change in _changes)
            {
                var owner = change.Key.Owner;
                var setter = change.Key.Setter;
                var value = change.Value;
                setter.DynamicInvoke(owner, value);
            }

            RejectChanges();
        }

        /// <inheritdoc/>
        public void RejectChanges()
        {
            _changes.Clear();
            IsChanged = false;
        }

        protected bool SetTrackingValue<TOwner, TProperty>(
            TOwner owner,
            Action<TOwner, TProperty> setter,
            TProperty oldValue,
            TProperty newValue,
            [CallerMemberName] string vmPropertyName = null)
        {
            if (EqualityComparer<TProperty>.Default.Equals(oldValue, newValue))
            {
                _changes.Remove((owner, setter));
                IsChanged = _changes.Count > 0;
                return false;
            }

            _changes[(owner, setter)] = newValue;
            RaisePropertyChanged(vmPropertyName);
            IsChanged = true;

            return true;
        }
    }
}