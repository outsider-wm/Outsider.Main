using System.Windows.Controls.Primitives;
using Outsider.Wrapper;

namespace Outsider.Main;
static class Program
{
    static bool IsCommon(Window window)
    {

        // https://github.com/glsorre/amethystwindows/blob/master/AmethystWindows/DesktopWindowsManager/DesktopWindow.cs#L46
        return window.WindowInfo.dwExStyle.HasFlag(NativeWin32UserApiStructs.WindowStylesEx.WS_EX_WINDOWEDGE) &&
               window.WindowInfo.dwStyle.HasFlag(NativeWin32UserApiStructs.WindowStyles.WS_MINIMIZEBOX) &&
               window.WindowInfo.dwStyle.HasFlag(NativeWin32UserApiStructs.WindowStyles.WS_MAXIMIZEBOX) &&
               !window.WindowInfo.dwExStyle.HasFlag(NativeWin32UserApiStructs.WindowStylesEx.WS_EX_TOPMOST) &&
               !window.WindowInfo.dwExStyle.HasFlag(NativeWin32UserApiStructs.WindowStylesEx.WS_EX_TOOLWINDOW);
    }
    static async Task Main()
    {
        List<Window> canTiledWindows = new();
        var windows = await WindowsParser.OnCurrentDesktop();
        windows.ForEach(win =>
        {
            if (IsCommon(win))
            {
                canTiledWindows.Add(win);
            }
        });

        canTiledWindows.ForEach(t =>
        {
            WrappedWin32UserApi.ShowWindow(t.Handle, NativeWin32UserApiStructs.ShowWindowMode.SW_NORMAL);
            WrappedWin32UserApi.MoveWindow(t.Handle, 0, 0, 200, 200, true);
        });
    }
}