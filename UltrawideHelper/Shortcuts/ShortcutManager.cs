using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Interop;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using UltrawideHelper.Audio;
using UltrawideHelper.Configuration;
using UltrawideHelper.Data;
using UltrawideHelper.Suspend;
using UltrawideHelper.Windows;

namespace UltrawideHelper.Shortcuts;

public class ShortcutManager : System.Windows.Window, IDisposable
{
    private readonly HwndSource source;
    private readonly HWND hwnd;

    private readonly List<int> registeredHotKeys;

    private readonly ConfigurationManager configurationManager;
    private readonly WindowManager windowManager;
    private readonly SuspendManager suspendManager;
    
    private const int ToggleMuteHotkeyId = 2141314714;
    private const int TogglePauseHotkeyId = 2141314715;

    public ShortcutManager(
        ConfigurationManager configurationManager, 
        WindowManager windowManager,
        SuspendManager suspendManager)
    {
        this.configurationManager = configurationManager;
        configurationManager.Changed += ConfigurationManager_Changed;

        this.windowManager = windowManager;
        this.suspendManager = suspendManager;

        var helper = new WindowInteropHelper(this);
        helper.EnsureHandle();

        source = HwndSource.FromHwnd(helper.Handle);
        hwnd = new HWND(helper.Handle);

        source?.AddHook(OnHotKeyPressed);

        registeredHotKeys = new List<int>();
        ConfigurationManager_Changed(configurationManager.ConfigFile);
    }

    public void Dispose()
    {
        UnregisterAllHotKeys();
        source.RemoveHook(OnHotKeyPressed);
        
        GC.SuppressFinalize(this);
    }

    private void ConfigurationManager_Changed(ConfigurationFile newConfiguration)
    {
        UnregisterAllHotKeys();

        if (!string.IsNullOrWhiteSpace(newConfiguration.MuteFocusedApplicationShortcut))
        {
            PInvoke.RegisterHotKey(
                hwnd, 
                ToggleMuteHotkeyId,
                (HOT_KEY_MODIFIERS) ShortcutHelper.GetModifier(newConfiguration.MuteFocusedApplicationShortcut),
                ShortcutHelper.GetKey(newConfiguration.MuteFocusedApplicationShortcut));
            registeredHotKeys.Add(ToggleMuteHotkeyId);
        }

        if (!string.IsNullOrWhiteSpace(newConfiguration.PauseFocusedApplicationShortcut))
        {
            PInvoke.RegisterHotKey(
                hwnd,
                TogglePauseHotkeyId,
                (HOT_KEY_MODIFIERS)ShortcutHelper.GetModifier(newConfiguration.PauseFocusedApplicationShortcut),
                ShortcutHelper.GetKey(newConfiguration.PauseFocusedApplicationShortcut));
            registeredHotKeys.Add(TogglePauseHotkeyId);
        }
        
        foreach (var shortcut in newConfiguration.ShortcutProfiles)
        {
            PInvoke.RegisterHotKey(hwnd, shortcut.Id, (HOT_KEY_MODIFIERS) ShortcutHelper.GetModifier(shortcut.KeyCombination), ShortcutHelper.GetKey(shortcut.KeyCombination));
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

    private IntPtr OnHotKeyPressed(IntPtr hwndIn, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case (int)PInvoke.WM_HOTKEY:
                var modifier = (int) lParam & 0x0000FFFF;
                var key = (int) ((int) lParam & 0xFFFF0000) >> 16;
                var foregroundHwnd = PInvoke.GetForegroundWindow();

                if (wParam.ToInt32() == ToggleMuteHotkeyId)
                {
                    unsafe
                    {
                        var keyCombination = configurationManager.ConfigFile.MuteFocusedApplicationShortcut;
                    
                        if (modifier != ShortcutHelper.GetModifier(keyCombination) || key != ShortcutHelper.GetKey(keyCombination))
                        {
                            return IntPtr.Zero;
                        }
                    
                        var pid = 0U;
                        PInvoke.GetWindowThreadProcessId(foregroundHwnd, &pid);
                        VolumeMixer.ToggleApplicationMute(pid);
                    
                        handled = true;
                        break;
                    }
                }

                if (wParam.ToInt32() == TogglePauseHotkeyId)
                {
                    unsafe
                    {
                        var keyCombination = configurationManager.ConfigFile.PauseFocusedApplicationShortcut;
                    
                        if (modifier != ShortcutHelper.GetModifier(keyCombination) || key != ShortcutHelper.GetKey(keyCombination))
                        {
                            return IntPtr.Zero;
                        }
                    
                        var pid = 0U;
                        PInvoke.GetWindowThreadProcessId(foregroundHwnd, &pid);
                        suspendManager.ToggleProcessSuspended(pid);
                    
                        handled = true;
                        break;
                    }
                }
                
                var shortcut = configurationManager.ConfigFile.ShortcutProfiles.SingleOrDefault(s => s.Id == wParam.ToInt32());

                if (shortcut == null)
                {
                    return IntPtr.Zero;
                }
                
                if (modifier != ShortcutHelper.GetModifier(shortcut.KeyCombination) || key != ShortcutHelper.GetKey(shortcut.KeyCombination))
                {
                    return IntPtr.Zero;
                }
                
                windowManager.ApplyWindowComposition(foregroundHwnd.Value, shortcut.WindowComposition);

                handled = true;
                break;
        }

        return IntPtr.Zero;
    }
}