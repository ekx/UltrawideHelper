using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using H.NotifyIcon;
using UltrawideHelper.Configuration;
using UltrawideHelper.Data;
using UltrawideHelper.Shortcuts;
using UltrawideHelper.Taskbar;
using UltrawideHelper.Update;
using UltrawideHelper.Windows;

namespace UltrawideHelper;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private ConfigurationManager configurationManager;
    private WindowManager windowManager;
    private ShortcutManager shortcutManager;
    private TaskbarManager taskbarManager;
    private TaskbarIcon notifyIcon;
    private Mutex appMutex;

    private const string AppOwner = "ekx";
    private const string AppName = "UltrawideHelper";
    private const string NotifyIconResourceName = "NotifyIcon";
    private const string ErrorLogFileName = "error.log";
    private const string AutoRunRegistryKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string ExecutableExtension = ".exe";

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

        configurationManager = new ConfigurationManager();
        configurationManager.Changed += ConfigurationManager_Changed;
        ConfigurationManager_Changed(configurationManager.ConfigFile);

        if (configurationManager.ConfigFile.AutoUpdate)
        {
            var updateManager = new UpdateManager(AppOwner, AppName);
            updateManager.CheckForNewVersion();
        }

        windowManager = new WindowManager(configurationManager);

        shortcutManager = new ShortcutManager(configurationManager, windowManager);

        taskbarManager = new TaskbarManager(configurationManager, windowManager);

        notifyIcon = (TaskbarIcon)FindResource(NotifyIconResourceName);
        notifyIcon?.ForceCreate();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        CleanUp();

        base.OnExit(e);
    }

    private void CleanUp()
    {
        appMutex?.Dispose();
        notifyIcon?.Dispose();
        taskbarManager?.Dispose();
        shortcutManager?.Dispose();
        windowManager?.Dispose();
        configurationManager?.Dispose();
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var serializableException = new SerializableException((Exception) e.ExceptionObject);
        
        var fileDirectory = Path.GetDirectoryName(ResourceAssembly.Location);
        var filePath = Path.Combine(fileDirectory ?? throw new InvalidOperationException(), ErrorLogFileName);

        File.WriteAllText(filePath, serializableException.ToString());

        CleanUp();
    }

    private void ConfigurationManager_Changed(ConfigurationFile newConfiguration)
    {
        var autoStartKey = Registry.CurrentUser.OpenSubKey(AutoRunRegistryKeyName, true);

        if (autoStartKey == null)
            return;
        
        if (newConfiguration.AutoStart)
        {
            autoStartKey.SetValue(AppName, Path.ChangeExtension(ResourceAssembly.Location, ExecutableExtension));
        }
        else if (autoStartKey.GetValue(AppName) != null)
        {
            autoStartKey.DeleteValue(AppName);
        }
    }        
}