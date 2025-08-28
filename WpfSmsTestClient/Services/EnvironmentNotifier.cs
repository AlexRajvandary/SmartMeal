using System.Runtime.InteropServices;

namespace WpfSmsTestClient.Services
{
    public class EnvironmentNotifier : IEnvironmentNotifier
    {
        private const uint WM_SETTINGCHANGE = 0x001A;
        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam,
            uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        public void NotifyEnvironmentChanged()
        {
            UIntPtr result;
            SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, UIntPtr.Zero, "Environment", 0, 1000, out result);
        }
    }

}
