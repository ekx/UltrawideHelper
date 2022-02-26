using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using UltrawideHelper.Data;

namespace UltrawideHelper.Windows
{
    public class Window
    {
        public bool IsModified { get { return appliedComposition != null; } }

        private HWND hwnd;

        private WindowComposition originalComposition;
        private WindowComposition appliedComposition;

        private string processName;

        public Window(nint windowHandle)
        {
            hwnd = new HWND(windowHandle);
            originalComposition = GetCurrentComposition();

            unsafe
            {
                var processId = 0U;
                PInvoke.GetWindowThreadProcessId(hwnd, &processId);
                processName = Process.GetProcessById((int)processId).ProcessName;
            }
        }

        public void ApplyWindowComposition(WindowComposition windowComposition)
        {           
            if (windowComposition.Equals(appliedComposition))
            {
                RevertWindowComposition();
                return;
            }

            SetCurrentComposition(windowComposition);
            appliedComposition = windowComposition;
        }

        public void RevertWindowComposition()
        {
            SetCurrentComposition(originalComposition);
            appliedComposition = null;
        }

        private WindowComposition GetCurrentComposition()
        {
            var result = new WindowComposition();

            RECT rect;
            PInvoke.GetWindowRect(hwnd, out rect);

            result.PositionX = rect.left;
            result.Width = rect.right - rect.left;
            result.PositionY = rect.top;
            result.Height = rect.bottom - rect.top;

            result.SetWindowStyle((uint)PInvoke.GetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE));
            result.SetExtendedWindowStyle((uint)PInvoke.GetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE));

            return result;
        }

        private async void SetCurrentComposition(WindowComposition windowComposition)
        {
            await Task.Delay(windowComposition.DelayMilliseconds);

            if (processName == "mstsc")
            {
                SET_WINDOW_POS_FLAGS uFlags = SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOOWNERZORDER | SET_WINDOW_POS_FLAGS.SWP_NOSENDCHANGING | SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED;

                PInvoke.SetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE, (int)WINDOW_STYLE.WS_VISIBLE | (int)WINDOW_STYLE.WS_THICKFRAME);
                PInvoke.SetWindowPos(hwnd, new HWND(0), windowComposition.PositionX, windowComposition.PositionY, windowComposition.Width, windowComposition.Height, uFlags);
                PInvoke.SendMessage(hwnd, PInvoke.WM_EXITSIZEMOVE, new WPARAM(0), new LPARAM(0));

                PInvoke.SetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE, (int)windowComposition.GetWindowStyle());
                PInvoke.SetWindowPos(hwnd, new HWND(0), windowComposition.PositionX, windowComposition.PositionY, windowComposition.Width, windowComposition.Height, uFlags);
                PInvoke.SendMessage(hwnd, PInvoke.WM_EXITSIZEMOVE, new WPARAM(0), new LPARAM(0));

                PInvoke.MoveWindow(hwnd, windowComposition.PositionX, windowComposition.PositionY, windowComposition.Width, windowComposition.Height, true);
            }
            else
            {
                if (windowComposition.HasWindowStyle)
                {
                    PInvoke.SetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE, (int)windowComposition.GetWindowStyle());
                }

                if (windowComposition.HasExtendedWindowStyle)
                {
                    PInvoke.SetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, (int)windowComposition.GetExtendedWindowStyle());
                }

                PInvoke.MoveWindow(hwnd, windowComposition.PositionX, windowComposition.PositionY, windowComposition.Width, windowComposition.Height, true);
                PInvoke.SendMessage(hwnd, PInvoke.WM_EXITSIZEMOVE, new WPARAM(0), new LPARAM(0));
            }

            // attempt at persona 4 workaround
            /*
            unsafe
            {
                RECT newSize = new RECT();
                newSize.left = windowComposition.PositionX;
                newSize.right = windowComposition.PositionX + windowComposition.Width;
                newSize.top = windowComposition.PositionY;
                newSize.bottom = windowComposition.PositionY + windowComposition.Height;

                nint newDim = 0;
                newDim = (newDim & 0xFFFF0000) + (windowComposition.Width & 0x0000FFFF);
                newDim = (newDim & 0x0000FFFF) + (windowComposition.Height << 16);

                PInvoke.SendMessage(hwnd, Constants.WM_ENTERSIZEMOVE, new WPARAM(0), new LPARAM(0));
                PInvoke.SendMessage(hwnd, Constants.WM_SIZING, new WPARAM(8), new LPARAM((int)&newSize));

                PInvoke.MoveWindow(hwnd, windowComposition.PositionX, windowComposition.PositionY, windowComposition.Width, windowComposition.Height, true);

                PInvoke.SetWindowLong(hwnd, Constants.GWL_STYLE, windowComposition.GetWindowStyle());
                PInvoke.SetWindowLong(hwnd, Constants.GWL_EXSTYLE, windowComposition.GetExtendedWindowStyle());
               
                PInvoke.SendMessage(hwnd, Constants.WM_SIZE, new WPARAM(0), new LPARAM(newDim));
                PInvoke.SendMessage(hwnd, Constants.WM_EXITSIZEMOVE, new WPARAM(0), new LPARAM(0));
            }
            */
        }
    }
}
