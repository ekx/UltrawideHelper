using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using System.Windows.Threading;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Accessibility;
using UltrawideHelper.Configuration;
using UltrawideHelper.Data;

namespace UltrawideHelper.Windows;

public class WindowManager : IDisposable
{
    private readonly ConfigurationManager configurationManager;
    private readonly Dictionary<nint, Window> windows;
    private readonly DispatcherTimer dispatcherTimer;
    private readonly WINEVENTPROC winEventProc;
    private readonly HWINEVENTHOOK winEventHook;
        
    public WindowManager(ConfigurationManager configurationManager)
    {
        this.configurationManager = configurationManager;

        windows = new Dictionary<nint, Window>();

        Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Children, OnWindowOpened);

        winEventProc = new WINEVENTPROC(OnFocusChanged);
        winEventHook = PInvoke.SetWinEventHook(PInvoke.EVENT_SYSTEM_FOREGROUND, PInvoke.EVENT_SYSTEM_FOREGROUND, new HINSTANCE(IntPtr.Zero), winEventProc, 0, 0, PInvoke.WINEVENT_OUTOFCONTEXT);

        dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
        dispatcherTimer.Start();
    }

    public void Dispose()
    {
        dispatcherTimer.Stop();

        foreach (var window in windows)
        {
            window.Value.RevertWindowComposition();
        }

        windows.Clear();

        Automation.RemoveAllEventHandlers();
        PInvoke.UnhookWinEvent(winEventHook);
    }
        
    private void OnFocusChanged(HWINEVENTHOOK hWinEventHook, uint @event, HWND hwnd, int idObject, int idChild, uint idEventThread, uint dwmsEventTime)
    {
        foreach (var window in windows.Values)
        {
            window.OnFocusChanged(hwnd);
        }
    }

    private void OnWindowOpened(object sender, AutomationEventArgs automationEventArgs)
    {
        if (!configurationManager.ConfigFile.AutoApplyProfiles)
        {
            return;
        }

        var element = sender as AutomationElement;

        if (element == null)
        {
            return;
        }

        var profile = configurationManager.ConfigFile.WindowProfiles.SingleOrDefault(p => new Regex(p.TitleRegex).IsMatch(element.Current.Name));

        if (profile == null)
        {
            return;
        }

        var process = Process.GetProcessById(element.Current.ProcessId);

        if (!process.ProcessName.Equals(profile.ProcessName))
        {
            return;
        }

        ApplyWindowComposition(element.Current.NativeWindowHandle, profile.WindowComposition);
    }

    private void DispatcherTimer_Tick(object sender, EventArgs e)
    {
        var windowsToRemove = windows.Select(pair => pair.Key).Where(handle => !PInvoke.IsWindow(new HWND(handle)));

        foreach (var key in windowsToRemove)
        {
            windows.Remove(key);
        }
    }

    public void ApplyWindowComposition(nint windowHandle, WindowComposition windowComposition)
    {
        if (!windows.ContainsKey(windowHandle))
        {
            windows.Add(windowHandle, new Window(windowHandle));
        }

        windows[windowHandle].ApplyWindowComposition(windowComposition);
    }
}