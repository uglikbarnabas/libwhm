using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows;
using libwhm;

namespace test;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        void UpdateDebugFg(WindowInfo _, uint _1)
        {
            debugDisplay.Children.Clear();
            int len = WindowInfo.OpenWindows.Count;
            for (int i = 0; i < len; i++)
            {
                debugDisplay.Children.Add(new Image() { Width = 100, Height = 100, Source = Imaging.CreateBitmapSourceFromHIcon(WindowInfo.OpenWindows[i].Icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) });
            }
            int len2 = debugDisplay.Children.Count;
            debug.Content = "WindowInfo.OpenWindows.Count = " + len + "\ndebugDisplay.Children.Count (StackPanel) = " + len2 + "\nThey should always be equal. But when explorer starts it mismatches sometimes.";
        }
        Hooks.Add((uint)EVENT.WINDOW_OPENED, (uint)EVENT.WINDOW_CLOSED, UpdateDebugFg);
        Hooks.Add((uint)EVENT.WINDOW_TITLE_CHANGED, UpdateDebugFg);
        Hooks.Add((uint)EVENT.FOREGROUND_CHANGED, UpdateDebugFg);

        Closing += (_, _) => Hooks.Remove((uint)EVENT.WINDOW_OPENED, (uint)EVENT.WINDOW_CLOSED);
        Closing += (_, _) => Hooks.Remove((uint)EVENT.WINDOW_TITLE_CHANGED);
        Closing += (_, _) => Hooks.Remove((uint)EVENT.FOREGROUND_CHANGED);
    }
}
