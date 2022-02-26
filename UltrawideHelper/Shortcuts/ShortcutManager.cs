using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Interop;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using UltrawideHelper.Configuration;
using UltrawideHelper.Data;
using UltrawideHelper.Windows;

namespace UltrawideHelper.Shortcuts
{
    public partial class ShortcutManager : System.Windows.Window, IDisposable
    {
        private HwndSource source;
        private HWND hwnd;

        private List<int> registeredHotKeys;

        private ConfigurationManager configurationManager;
        private WindowManager windowManager;

        public ShortcutManager(ConfigurationManager configurationManager, WindowManager windowManager)
        {
            this.configurationManager = configurationManager;
            configurationManager.Changed += ConfigurationManager_Changed;

            this.windowManager = windowManager;

            var helper = new WindowInteropHelper(this);
            helper.EnsureHandle();

            source = HwndSource.FromHwnd(helper.Handle);
            hwnd = new HWND(helper.Handle);

            source.AddHook(OnHotKeyPressed);

            registeredHotKeys = new List<int>();
            ConfigurationManager_Changed(configurationManager.ConfigFile);
        }

        public void Dispose()
        {
            UnregisterAllHotKeys();
            source.RemoveHook(OnHotKeyPressed);
        }

        private void ConfigurationManager_Changed(ConfigurationFile newConfiguration)
        {
            UnregisterAllHotKeys();

            foreach (var shortcut in newConfiguration.ShortcutProfiles)
            {
                PInvoke.RegisterHotKey(hwnd, shortcut.Id, (HOT_KEY_MODIFIERS) shortcut.GetModifier(), shortcut.GetKey());
                registeredHotKeys.Add(shortcut.Id);
            }
        }

        private void UnregisterAllHotKeys()
        {
            foreach (var hotKey in registeredHotKeys)
            {
                PInvoke.UnregisterHotKey(hwnd, hotKey);
            }

            registeredHotKeys.Clear();
        }

        private IntPtr OnHotKeyPressed(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case (int)PInvoke.WM_HOTKEY:
                    var shortcut =
                        configurationManager.ConfigFile.ShortcutProfiles.SingleOrDefault(s => s.Id == wParam.ToInt32());

                    if (shortcut == null)
                    {
                        return IntPtr.Zero;
                    }

                    int modifier = (int) lParam & 0x0000FFFF;
                    int key = (int) ((int) lParam & 0xFFFF0000) >> 16;

                    if (modifier != shortcut.GetModifier() || key != shortcut.GetKey())
                    {
                        return IntPtr.Zero;
                    }

                    var foregroundHwnd = PInvoke.GetForegroundWindow();
                    windowManager.ApplyWindowComposition(foregroundHwnd.Value, shortcut.WindowComposition);

                    handled = true;

                    break;
            }

            return IntPtr.Zero;
        }
    }
}