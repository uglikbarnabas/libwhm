using System.Runtime.InteropServices;
using System.Drawing;

namespace libwhm;

public class IconExtractor
{
    private const int SHGFI_ICON = 0x100;
    private const int SHGFI_LARGEICON = 0x0;

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref Shfileinfo psfi, uint cbFileInfo, uint uFlags);

    [StructLayout(LayoutKind.Sequential)]
    private struct Shfileinfo
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    public static Icon? ExtractIcon(string filePath)
    {
        Shfileinfo shinfo = new();
        IntPtr hImg = SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);

        if (hImg != IntPtr.Zero)
        {
            Icon icon = Icon.FromHandle(shinfo.hIcon);
            return icon;
        }
        else
        {
            return null;
        }
    }
}