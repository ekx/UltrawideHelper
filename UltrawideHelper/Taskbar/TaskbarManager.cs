using Microsoft.Windows.Sdk;
using System;
using System.Windows.Threading;
using UltrawideHelper.Configuration;

namespace UltrawideHelper.Taskbar
{
    public class TaskbarManager : IDisposable
    {
        private DispatcherTimer dispatcherTimer;
        private Taskbar primaryTaskbar;
        private IAppVisibility appVisibility;

        private const string PrimaryTaskbarClassName = "Shell_TrayWnd";

        public TaskbarManager(ConfigurationManager configurationManager)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);

            appVisibility = (IAppVisibility)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("7E5FE3D9-985F-4908-91F9-EE19F9FD1514")));

            var handle = PInvoke.FindWindow(PrimaryTaskbarClassName, string.Empty);
            primaryTaskbar = new Taskbar(handle.Value);

            configurationManager.Changed += ConfigurationManager_Changed;
            ConfigurationManager_Changed(configurationManager.ConfigFile);
        }

        public void Dispose()
        {
            dispatcherTimer.Stop();
            primaryTaskbar.SetVisibility(true);
        }

        private void ConfigurationManager_Changed(Data.ConfigurationFile newConfiguration)
        {
            if (newConfiguration.HideTaskbar)
            {
                dispatcherTimer.Start();
            }
            else
            {
                dispatcherTimer.Stop();
                primaryTaskbar.SetVisibility(true);
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            appVisibility.IsLauncherVisible(out bool visible);
            primaryTaskbar.SetVisibility(visible);
        }
    }
}
