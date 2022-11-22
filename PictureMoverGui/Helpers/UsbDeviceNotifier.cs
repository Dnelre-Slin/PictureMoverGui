using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace PictureMoverGui.Helpers
{
    public static class UsbDeviceNotifier
    {
        public static event Action UsbRemovableDeviceAdded;
        public static event Action UsbRemovableDeviceRemoved;
        public static event Action UsbMediaDeviceAdded;
        public static event Action UsbMediaDeviceRemoved;

        public static void Setup(Window MainWindow)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(MainWindow).Handle);
            if (source != null)
            {
                source.AddHook(HwndHandler);
            }
        }

        private static IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            const int WmDevicechange = 0x0219;
            const int DbtDevicearrival = 0x8000;
            const int DbtDeviceremovecomplete = 0x8004;
            const int DbtDevtypRemovableDevice = 0x00000002;
            const int DbtDevtypMediaDevice = 0x00000003;

            if (msg == WmDevicechange)
            {
                switch ((int)wparam)
                {
                    case DbtDeviceremovecomplete:
                    case DbtDevicearrival:
                        bool Added = (int)wparam == DbtDevicearrival;
                        var deviceTypeBytes = new byte[4];
                        Marshal.Copy(lparam + 4, deviceTypeBytes, 0, 4); // Get deviceType part of lparam
                        int deviceType = BitConverter.ToInt32(deviceTypeBytes);
                        if (deviceType == DbtDevtypRemovableDevice)
                        {
                            if (Added)
                            {
                                UsbRemovableDeviceAdded?.Invoke();
                            }
                            else
                            {
                                UsbRemovableDeviceRemoved?.Invoke();
                            }
                        }
                        else if (deviceType == DbtDevtypMediaDevice)
                        {
                            if (Added)
                            {
                                UsbMediaDeviceAdded?.Invoke();
                            }
                            else
                            {
                                UsbMediaDeviceRemoved?.Invoke();
                            }
                        }
                        break;
                }
            }
            handled = false;
            return IntPtr.Zero;
        }
    }
}
