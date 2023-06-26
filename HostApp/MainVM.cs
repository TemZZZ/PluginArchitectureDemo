using System.Linq;
using WpfLibrary;

namespace HostApp
{
    internal class MainVM : VMBase
    {
        public MainVM()
        {
            var vmProvider = new MainVMProvider();
            MainVMSwitcher = new VMSwitcher(vmProvider);
            MainVMSwitcher.SwitchCommand.Execute(vmProvider.Keys.First());
        }

        public VMSwitcher MainVMSwitcher { get; }
    }
}