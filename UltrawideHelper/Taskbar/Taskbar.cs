using Microsoft.Windows.Sdk;

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
            PInvoke.ShowWindow(hwnd, visible ? Constants.SW_SHOW : Constants.SW_HIDE);
        }
    }
}
