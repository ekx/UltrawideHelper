using Microsoft.Windows.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using System.Windows.Threading;
using UltrawideHelper.Configuration;
using UltrawideHelper.Data;

namespace UltrawideHelper.Windows
{
    public class WindowManager : IDisposable
    {
        private ConfigurationManager configurationManager;
        private Dictionary<nint, Window> windows;
        private DispatcherTimer dispatcherTimer;

        public WindowManager(ConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;

            windows = new Dictionary<nint, Window>();

            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Children, OnWindowOpened);

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
        }

        private void OnWindowOpened(object sender, AutomationEventArgs automationEventArgs)
        {
            if (!configurationManager.ConfigFile.AutoApplyProfiles)
            {
                return;
            }

            try
            {
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

                if (process == null || !process.ProcessName.Equals(profile.ProcessName))
                {
                    return;
                }

                ApplyWindowComposition(element.Current.NativeWindowHandle, profile.WindowComposition);
            }
            catch (ElementNotAvailableException)
            {
                return;
            }
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
}
