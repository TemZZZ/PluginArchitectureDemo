using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfLibrary
{
    public class TemplateTypes : ObservableCollection<KeyTypePair>
    {
        public Dictionary<string, Type> ToDictionary()
        {
            var dictionary = new Dictionary<string, Type>(Count);
            foreach (var keyTypePair in this)
            {
                dictionary[keyTypePair.Key] = keyTypePair.Type;
            }

            return dictionary;
        }
    }
}