using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using UltrawideHelper.Configuration;
using UltrawideHelper.Data;
using UltrawideHelper.Shortcuts;
using UltrawideHelper.Taskbar;
using UltrawideHelper.Windows;

namespace UltrawideHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ConfigurationManager configurationManager;
        private WindowManager windowManager;
        private ShortcutManager shortcutManager;
        private TaskbarManager taskbarManager;
        private TaskbarIcon notifyIcon;
        private Mutex appMutex;

        private const string AppName = "UltrawideHelper";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            appMutex = new Mutex(true, AppName, out bool newMutexCreated);
            if (!newMutexCreated)
            {
                Shutdown();
                return;
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // TODO: Auto updater?

            configurationManager = new ConfigurationManager();
            configurationManager.Changed += ConfigurationManager_Changed;
            ConfigurationManager_Changed(configurationManager.ConfigFile);

            windowManager = new WindowManager(configurationManager);

            shortcutManager = new ShortcutManager(configurationManager, windowManager);

            taskbarManager = new TaskbarManager(configurationManager);

            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            appMutex?.Dispose();
            notifyIcon?.Dispose();
            taskbarManager?.Dispose();
            shortcutManager?.Dispose();
            windowManager?.Dispose();
            configurationManager?.Dispose();

            base.OnExit(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var serializableException = new SerializableException((Exception) e.ExceptionObject);
            File.WriteAllText("error.log", serializableException.ToString());
        }

        private void ConfigurationManager_Changed(ConfigurationFile newConfiguration)
        {
            var autoStartKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (newConfiguration.AutoStart)
            {
                autoStartKey.SetValue(AppName, Path.ChangeExtension(Application.ResourceAssembly.Location, ".exe"));
            }
            else if (autoStartKey.GetValue(AppName) != null)
            {
                autoStartKey.DeleteValue(AppName);
            }
        }        
    }
}
