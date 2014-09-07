using AssemblyInstaller.Model;
using AssemblyInstaller.ViewModel;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.ServiceLocation;

namespace AssemblyInstaller.Helpers
{
    public static class LogFile
    {
        public static void Write(string assembly, string message)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => ServiceLocator.Current.GetInstance<MainViewModel>().Log.Add(new LogEntity(assembly, message)));
        }
    }
}
