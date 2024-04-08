using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace libwhm;

internal partial class Native
{
    [LibraryImport("user32.dll")] internal static partial IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
    internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, uint idObject, uint idChild, uint dwEventThread, uint dwmsEventTime);
    [LibraryImport("user32.dll")] internal static partial void UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    internal static string GetWindowTitle(IntPtr hWnd)
    { StringBuilder tb = new(1024); _ = GetWindowText(hWnd, tb, 1024); return tb.ToString(); }
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern int GetClassName(IntPtr hwnd, StringBuilder className, int maxLength);
    internal static string GetWindowClass(IntPtr hWnd)
    { StringBuilder cb = new(1024); _ = GetClassName(hWnd, cb, 1024); return cb.ToString(); }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)] internal static extern bool IsZoomed(IntPtr hwnd);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] internal static extern bool IsIconic(IntPtr hwnd);

    [DllImport("user32.dll")] internal static extern void SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")] internal static extern IntPtr GetForegroundWindow();

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
    internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")] internal static extern void ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern void GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    public static string GetWindowExecutablePath(IntPtr hWnd)
    {
        GetWindowThreadProcessId(hWnd, out int processId);
        return Process.GetProcessById(processId).MainModule?.FileName ?? "Failed";
    }
}

internal enum ShowWindowCommand
{
    SW_HIDE = 0,
    SW_SHOWNORMAL = 1,
    SW_SHOWMINIMIZED = 2,
    SW_SHOWMAXIMIZED = 3,
    SW_SHOWNOACTIVATE = 4,
    SW_SHOW = 5,
    SW_MINIMIZE = 6,
    SW_SHOWMINNOACTIVE = 7,
    SW_SHOWNA = 8,
    SW_RESTORE = 9,
    SW_SHOWDEFAULT = 10,
    SW_FORCEMINIMIZE = 11
}