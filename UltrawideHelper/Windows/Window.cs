using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using UltrawideHelper.Audio;
using UltrawideHelper.Data;

namespace UltrawideHelper.Windows;

public class Window
{
    private readonly HWND hwnd;

    private readonly WindowComposition originalComposition;
    private WindowComposition appliedComposition;

    private readonly uint processId;
    private readonly string processName;

    public Window(nint windowHandle)
    {
        hwnd = new HWND(windowHandle);
        originalComposition = GetCurrentComposition();

        unsafe
        {
            var pid = 0U;
            PInvoke.GetWindowThreadProcessId(hwnd, &pid);
            processId = pid;
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
        
    internal void OnFocusChanged(HWND currentFocus)
    {
        if (appliedComposition == null) return;

        if (appliedComposition.MuteWhileNotFocused)
        {
            VolumeMixer.SetApplicationMute(processId, hwnd.Value != currentFocus.Value);
        }
    }

    private WindowComposition GetCurrentComposition()
    {
        var result = new WindowComposition();

        PInvoke.GetWindowRect(hwnd, out var rect);

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
            const SET_WINDOW_POS_FLAGS uFlags = SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOOWNERZORDER | SET_WINDOW_POS_FLAGS.SWP_NOSENDCHANGING | SET_WINDOW_POS_FLAGS.SWP_FRAMECHANGED;

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
    }
}