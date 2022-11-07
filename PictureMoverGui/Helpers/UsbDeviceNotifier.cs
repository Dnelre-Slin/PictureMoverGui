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
                        Debug.WriteLine($"{Added} : {deviceType}");
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

    //public static class UsbDeviceNotifier
    //{
    //    public const int DbtDevicearrival = 0x8000; // system detected a new device        
    //    public const int DbtDeviceremovecomplete = 0x8004; // device is gone      
    //    public const int WmDevicechange = 0x0219; // device change event      
    //    private const int DbtDevtypDeviceinterface = 5;
    //    private static readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
    //    private static IntPtr notificationHandle = IntPtr.Zero;

    //    /// <summary>
    //    /// Registers a window to receive notifications when USB devices are plugged or unplugged.
    //    /// </summary>
    //    /// <param name="windowHandle">Handle to the window receiving notifications.</param>
    //    public static void RegisterUsbDeviceNotification(IntPtr windowHandle)
    //    {
    //        if (notificationHandle == IntPtr.Zero)
    //        {
    //            DevBroadcastDeviceinterface dbi = new DevBroadcastDeviceinterface
    //            {
    //                DeviceType = DbtDevtypDeviceinterface,
    //                Reserved = 0,
    //                ClassGuid = GuidDevinterfaceUSBDevice,
    //                Name = 0
    //            };

    //            dbi.Size = Marshal.SizeOf(dbi);
    //            IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
    //            Marshal.StructureToPtr(dbi, buffer, true);

    //            notificationHandle = RegisterDeviceNotification(windowHandle, buffer, 0);
    //        }
    //    }

    //    /// <summary>
    //    /// Unregisters the window for USB device notifications
    //    /// </summary>
    //    public static void UnregisterUsbDeviceNotification()
    //    {
    //        if (notificationHandle != IntPtr.Zero)
    //        {
    //            UnregisterDeviceNotification(notificationHandle);
    //            notificationHandle = IntPtr.Zero;
    //        }
    //    }

    //    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //    private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

    //    [DllImport("user32.dll")]
    //    private static extern bool UnregisterDeviceNotification(IntPtr handle);

    //    [StructLayout(LayoutKind.Sequential)]
    //    private struct DevBroadcastDeviceinterface
    //    {
    //        internal int Size;
    //        internal int DeviceType;
    //        internal int Reserved;
    //        internal Guid ClassGuid;
    //        internal short Name;
    //    }
    //}
}
