using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfLibrary;

namespace HostApp
{
    internal class TemplatesVM : VMBase
    {
        private string _selectedKey;

        public TemplatesVM(IVMProvider vmProvider)
        {
            VMSwitcher = new VMSwitcher(vmProvider);
            Keys = new ObservableCollection<string>(vmProvider.Keys);
        }

        public VMSwitcher VMSwitcher { get; }

        public ObservableCollection<string> Keys { get; }

        public string SelectedKey
        {
            get => _selectedKey;
            set
            {
                if (!SetValue(ref _selectedKey, value))
                {
                    return;
                }

                VMSwitcher.SwitchCommand.Execute(value);
            }
        }
    }
}