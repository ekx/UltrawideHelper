﻿using Windows.Win32;
using System;
using System.Windows.Threading;
using UltrawideHelper.Configuration;
using UltrawideHelper.Data;
using UltrawideHelper.Windows;

namespace UltrawideHelper.Taskbar;

public class TaskbarManager : IDisposable
{
    private readonly DispatcherTimer dispatcherTimer;
    private readonly Taskbar primaryTaskbar;
    private readonly IAppVisibility appVisibility;
    private readonly WindowManager windowManager;
    private ConfigurationFile currentConfiguration;

    private const string PrimaryTaskbarClassName = "Shell_TrayWnd";

    public TaskbarManager(ConfigurationManager configurationManager, WindowManager windowManager)
    {
        dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);

        appVisibility = (IAppVisibility)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("7E5FE3D9-985F-4908-91F9-EE19F9FD1514")) ?? throw new InvalidOperationException());

        var handle = PInvoke.FindWindow(PrimaryTaskbarClassName, string.Empty);
        primaryTaskbar = new Taskbar(handle.Value);

        configurationManager.Changed += ConfigurationManager_Changed;
        ConfigurationManager_Changed(configurationManager.ConfigFile);

        this.windowManager = windowManager;
    }

    public void Dispose()
    {
        dispatcherTimer.Stop();
        primaryTaskbar.SetVisibility(true);
        
        GC.SuppressFinalize(this);
    }

    private void ConfigurationManager_Changed(Data.ConfigurationFile newConfiguration)
    {
        currentConfiguration = newConfiguration;
        
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
        appVisibility.IsLauncherVisible(out var startMenuVisible);
        var profileActive = windowManager.IsAnyProfileActive;

        var showTaskbar = (currentConfiguration.HideTaskbarWhenProfileActive && !profileActive) || startMenuVisible;
        primaryTaskbar.SetVisibility(showTaskbar);
    }
}