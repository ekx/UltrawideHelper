using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace UltrawideHelper.Taskbar
{
    public class Taskbar
    {
        private HWND hwnd;

        public Taskbar(nint windowHandle)
        {
            hwnd = new HWND(windowHandle);
        }

        public void SetVisibility(bool visible)
        {
            PInvoke.ShowWindow(hwnd, visible ? SHOW_WINDOW_CMD.SW_SHOW : SHOW_WINDOW_CMD.SW_HIDE);
        }
    }
}
