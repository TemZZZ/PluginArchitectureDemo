using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfLibrary
{
    /// <summary>
    /// Базовый класс вью-модели, которая может отслеживать изменения свойств
    /// инкапсулированных в ней объектов, позволяет принять или откатить эти изменения.
    /// </summary>
    public class ChangeTrackingVMBase : VMBase, IRevertibleChangeTracking
    {
        /// <summary>
        /// Словарь изменений объектов. Ключ - пара "владелец свойства" и "сеттер свойства",
        /// значение - "текущее значение свойства".
        /// </summary>
        private readonly Dictionary<(object Owner, Delegate Setter), object> _changes
            = new Dictionary<(object, Delegate), object>();

        /// <summary>
        /// Есть ли изменения хоть у одного свойства.
        /// </summary>
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

        /// <summary>
        /// Задает новое значение отслеживаемому свойству.
        /// </summary>
        /// <typeparam name="TOwner">Тип объекта-владельца свойства.</typeparam>
        /// <typeparam name="TProperty">Тип свойства.</typeparam>
        /// <param name="owner">Объект-владелец свойства.</param>
        /// <param name="setter">Сеттер свойства.</param>
        /// <param name="oldValue">Старое (текущее) значение свойства.</param>
        /// <param name="newValue">Новое значение свойства.</param>
        /// <param name="vmPropertyName">Имя свойства этой вью-модели,
        /// об изменении которого будет выслано событие PropertyChanged.</param>
        /// <returns></returns>
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