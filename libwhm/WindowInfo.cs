using System.Drawing;

namespace libwhm;

public class WindowInfo(IntPtr windowHandle)
{
    public static WindowInfo? ForegroundWindow => OpenWindows.Find(x => x.IsForeground);
    public static List<WindowInfo> OpenWindows => GetOpenWindows();

    private static List<WindowInfo> GetOpenWindows()
    {
        List<WindowInfo> windowList = [];
        _ = Native.EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
        {
            WindowInfo currentWindow = new(hWnd);
            if (!Native.IsWindowVisible(hWnd)) return true;
            if (currentWindow.ClassName == "Progman") return true;
            if (string.IsNullOrWhiteSpace(currentWindow.Title)) return true;
            if (currentWindow.ClassName == "Xaml_WindowedPopupClass") return true;
            if (currentWindow.ClassName == "Windows.UI.Core.CoreWindow") return true;

            windowList.Add(currentWindow);
            return true;
        }, IntPtr.Zero);

        return windowList;
    }

    public bool IsMaximized
    { get => Native.IsZoomed(windowHandle); set { if (value) Native.ShowWindow(windowHandle, (int)ShowWindowCommand.SW_SHOWMAXIMIZED); } }
    public bool IsMinimized
    { get => Native.IsIconic(windowHandle); set { if (value) Native.ShowWindow(windowHandle, (int)ShowWindowCommand.SW_SHOWMINIMIZED); } }
    public bool IsForeground
    { get => Native.GetForegroundWindow() == windowHandle; set { if (value) Native.SetForegroundWindow(windowHandle); } }
    public string ExecutablePath => Native.GetWindowExecutablePath(windowHandle);
    public Icon? Icon => Icon.ExtractAssociatedIcon(ExecutablePath);
    public string ClassName => Native.GetWindowClass(windowHandle);
    public string Title => Native.GetWindowTitle(windowHandle);

    public override string ToString() => ExecutablePath;
}