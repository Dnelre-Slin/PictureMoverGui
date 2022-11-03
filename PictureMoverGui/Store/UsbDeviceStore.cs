using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

namespace PictureMoverGui.Store
{
    public class UsbDeviceStore
    {
        public event Action<CollectiveDeviceInfoModel> DeviceInfoChanged;

        private UsbDeviceLookupWorker _usbDeviceLookupWorker;

        private CollectiveDeviceInfoModel _collectiveDeviceInfo;

        public IEnumerable<DriveInfoModel> DriveInfoList => _collectiveDeviceInfo.DriveInfoList;
        public IEnumerable<MediaDeviceModel> MediaDeviceList => _collectiveDeviceInfo.MediaDeviceList;
        public MediaDeviceModel SelectedMediaDevice => _collectiveDeviceInfo.SelectedMediaDevice;

        public string ChosenMediaDeviceName { get; private set; }

        public bool IsResolving { get; private set; }

        public UsbDeviceStore()
        {
            _usbDeviceLookupWorker = new UsbDeviceLookupWorker();
            _collectiveDeviceInfo = new CollectiveDeviceInfoModel(new List<DriveInfoModel>(), new List<MediaDeviceModel>(), null);

            ChosenMediaDeviceName = "Nils sin S20+";

            IsResolving = false;

            UpdateUsbInfoLists();
        }
        public void Setup(Window MainWindow)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(MainWindow).Handle);
            if (source != null)
            {
                source.AddHook(HwndHandler);
                Debug.WriteLine("Registrering");
                UsbDeviceNotifier.RegisterUsbDeviceNotification(source.Handle);
            }
        }

        public void Dispose()
        {
            Debug.WriteLine("Unregistrering");
            UsbDeviceNotifier.UnregisterUsbDeviceNotification();
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == UsbDeviceNotifier.WmDevicechange)
            {
                switch ((int)wparam)
                {
                    case UsbDeviceNotifier.DbtDeviceremovecomplete:
                        UpdateUsbInfoLists();
                        break;
                    case UsbDeviceNotifier.DbtDevicearrival:
                        UpdateUsbInfoLists();
                        break;
                }
            }
            handled = false;
            return IntPtr.Zero;
        }

        private void UpdateUsbInfoLists()
        {
            IsResolving = true;
            _usbDeviceLookupWorker.StartWorker(ChosenMediaDeviceName, UpdateUsbInfoListsWorkDone);
        }

        private void UpdateUsbInfoListsWorkDone(WorkStatus workStatus, CollectiveDeviceInfoModel collectiveDeviceInfo)
        {
            if (workStatus == WorkStatus.Success)
            {
                _collectiveDeviceInfo = collectiveDeviceInfo;
            }
            else
            {
                _collectiveDeviceInfo = new CollectiveDeviceInfoModel(new List<DriveInfoModel>(), new List<MediaDeviceModel>(), null);
            }
            IsResolving = false;
            DeviceInfoChanged?.Invoke(_collectiveDeviceInfo);
        }
    }
}
