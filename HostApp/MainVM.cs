using WpfLibrary;

namespace HostApp
{
    internal class MainVM : VMBase
    {
        public MainVM()
        {
            MainVMSwitcher = new VMSwitcher(new MainVMProvider());
        }

        public VMSwitcher MainVMSwitcher { get; }
    }
}