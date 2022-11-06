using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        public MediaDeviceModel SelectedMediaDevice { get; private set; }

        public string ChosenMediaDeviceName { get; private set; }
        public DateTime ChosenMediaLastTime { get; private set; }

        public bool IsResolving { get; private set; }

        public UsbDeviceStore()
        {
            _usbDeviceLookupWorker = new UsbDeviceLookupWorker();
            _collectiveDeviceInfo = new CollectiveDeviceInfoModel(new List<DriveInfoModel>(), new List<MediaDeviceModel>());

            ChosenMediaDeviceName = "Nils sin S20+";
            ChosenMediaLastTime = new DateTime(2019, 01, 01);
            //ChosenMediaDeviceName = "Test Device 123";

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
                //UsbDeviceNotifier.RegisterUsbDeviceNotification(source.Handle);
            }
        }

        public void Dispose()
        {
            Debug.WriteLine("Unregistrering");
            //UsbDeviceNotifier.UnregisterUsbDeviceNotification();
        }

        public void SetNewChosenMediaName(string chosenMediaName)
        {
            foreach (MediaDeviceModel mediaDevice in MediaDeviceList)
            {
                if (mediaDevice.FriendlyName == chosenMediaName)
                {
                    ChosenMediaDeviceName = chosenMediaName;
                    SelectedMediaDevice = _collectiveDeviceInfo.MediaDeviceList.Find(md => md.FriendlyName == ChosenMediaDeviceName);
                }
            }
            DeviceInfoChanged?.Invoke(_collectiveDeviceInfo);
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == UsbDeviceNotifier.WmDevicechange)
            {
                DEV_BROADCAST_HDR dbh;
                switch ((int)wparam)
                {
                    case UsbDeviceNotifier.DbtDeviceremovecomplete:
                    case UsbDeviceNotifier.DbtDevicearrival:
                        var tb = new byte[(int)4];
                        Marshal.Copy(lparam + (int)4, tb, 0, 4);
                        int res = BitConverter.ToInt32(tb);
                        Debug.WriteLine(res);
                        dbh = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(lparam, typeof(DEV_BROADCAST_HDR));
                        Debug.WriteLine(dbh.dbch_devicetype);
                        if (dbh.dbch_devicetype == 0x00000002)
                        {
                            var portNameBytes = new byte[dbh.dbch_size - (int)12];
                            Marshal.Copy(lparam + (int)12, portNameBytes, 0, portNameBytes.Length);
                            string portName = Encoding.Unicode.GetString(portNameBytes).TrimEnd('\0');
                            Debug.WriteLine(portName);
                        }
                        //UpdateUsbInfoLists();
                        break;
                    //case UsbDeviceNotifier.DbtDeviceremovecomplete:
                    //    UpdateUsbInfoLists();
                    //    break;
                    //case UsbDeviceNotifier.DbtDevicearrival:
                    //    UpdateUsbInfoLists();
                    //    break;
                }
            }
            handled = false;
            return IntPtr.Zero;
        }

        public struct DEV_BROADCAST_HDR
        {
            internal UInt32 dbch_size;
            internal UInt32 dbch_devicetype;
            internal UInt32 dbch_reserved;
        };

        private void UpdateUsbInfoLists()
        {
            IsResolving = true;
            _usbDeviceLookupWorker.StartWorker(UpdateUsbInfoListsWorkDone);
        }

        private void UpdateUsbInfoListsWorkDone(WorkStatus workStatus, CollectiveDeviceInfoModel collectiveDeviceInfo)
        {
            if (workStatus == WorkStatus.Success)
            {
                _collectiveDeviceInfo = collectiveDeviceInfo;
                _collectiveDeviceInfo.MediaDeviceList.Add(new MediaDeviceModel("Test Device 123", "1234567890", null));
                SelectedMediaDevice = collectiveDeviceInfo.MediaDeviceList.Find(md => md.FriendlyName == ChosenMediaDeviceName);
            }
            else
            {
                _collectiveDeviceInfo = new CollectiveDeviceInfoModel(new List<DriveInfoModel>(), new List<MediaDeviceModel>());
            }
            IsResolving = false;
            DeviceInfoChanged?.Invoke(_collectiveDeviceInfo);
        }
    }
}
