using System.Linq;
using WpfLibrary;

namespace HostApp
{
    internal class MainVM : VMBase
    {
        public MainVM(IVMProvider templateVMProvider)
        {
            var vmProvider = new MainVMProvider(templateVMProvider);
            MainVMSwitcher = new VMSwitcher(vmProvider);
            MainVMSwitcher.SwitchCommand.Execute(vmProvider.Keys.First());
        }

        public VMSwitcher MainVMSwitcher { get; }
    }
}