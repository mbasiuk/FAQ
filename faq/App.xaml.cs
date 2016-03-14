using System.Diagnostics;
using System.Windows;

namespace Faq
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string currentProcessName = Process.GetCurrentProcess().ProcessName;
            if (Process.GetProcessesByName(currentProcessName).Length > 1)
            {
                Current.Shutdown();
            }
        }
    }
}
