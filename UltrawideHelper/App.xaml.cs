using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Windows.Sdk;
using System;
using System.Windows;
using System.Windows.Interop;

namespace UltrawideHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IntPtr handle;
        private HwndSource source;
        private HWND hwnd;

        private TaskbarIcon notifyIcon;

        private const int HOTKEY_ID = 9000;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();
            ((MainWindow)MainWindow).InitHwnd();

            // Create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
            notifyIcon = (TaskbarIcon) FindResource("NotifyIcon");

            handle = new WindowInteropHelper(MainWindow).Handle;
            source = HwndSource.FromHwnd(handle);
            hwnd = new HWND(handle);

            source.AddHook(HwndHook);
            PInvoke.RegisterHotKey(hwnd, HOTKEY_ID, Constants.MOD_ALT, Constants.VK_F11);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            PInvoke.UnregisterHotKey(hwnd, HOTKEY_ID);
            source.RemoveHook(HwndHook);            
            notifyIcon.Dispose();

            base.OnExit(e);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Constants.WM_HOTKEY:

                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:

                            int vkey = (((int)lParam >> 16) & 0xFFFF);

                            if (vkey == Constants.VK_F11)
                            {
                                OnHotKeyPressed();
                            }

                            handled = true;
                            break;
                    }

                    break;
            }

            return IntPtr.Zero;
        }

        private void OnHotKeyPressed()
        {
            var foregroundHwnd = PInvoke.GetForegroundWindow();

            PInvoke.SetWindowLong(foregroundHwnd, Constants.GWL_STYLE, (int)Constants.WS_VISIBLE);
            PInvoke.MoveWindow(foregroundHwnd, 0, -1, 5120, 1440, true);

            //PInvoke.SetWindowPos(foregroundHwnd, new HWND(0), 0, 0, 3440, 1440, 0);

            //var style = PInvoke.GetWindowLong(foregroundHwnd, Constants.GWL_STYLE);
            //MessageBox.Show($"style: {style}");

            //RECT rect;
            //PInvoke.GetWindowRect(foregroundHwnd, out rect);
            //MessageBox.Show($"bottom: {rect.bottom}, left: {rect.left}, right: {rect.right}, top: {rect.top}");
        }
    }
}
