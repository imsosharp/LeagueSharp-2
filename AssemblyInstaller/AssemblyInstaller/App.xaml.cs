using System;
using System.IO;
using GalaSoft.MvvmLight.Threading;

namespace AssemblyInstaller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        static App()
        {
            DispatcherHelper.Initialize();
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Repositories"));
        }
    }
}
