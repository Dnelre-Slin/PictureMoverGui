using MediaDevices;
using PictureMoverGui.DeviceWorkers;
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
        //public event Action<CollectiveDeviceInfoModel> DeviceInfoChanged;
        public event Action<UsbDeviceStore> DeviceInfoChanged;
        //public static event Action<MediaDeviceModel> UsbRemovableDeviceAdded;
        //public static event Action UsbRemovableDeviceAdded;
        //public static event Action UsbRemovableDeviceRemoved;
        ////public static event Action<MediaDeviceModel> UsbMediaDeviceAdded;
        //public static event Action UsbMediaDeviceAdded;
        //public static event Action UsbMediaDeviceRemoved;

        //private UsbDeviceLookupWorker _usbDeviceLookupWorker;
        private MediaDeviceChangeWorker _mediaDeviceChangeWorker;
        private RemovableDeviceChangeWorker _removableDeviceChangeWorker;

        //private CollectiveDeviceInfoModel _collectiveDeviceInfo;
        private List<MediaDeviceModel> _mediaDeviceList;

        //public IEnumerable<DriveInfoModel> DriveInfoList => _collectiveDeviceInfo.DriveInfoList;
        //public IEnumerable<MediaDeviceModel> MediaDeviceList => _collectiveDeviceInfo.MediaDeviceList;
        public IEnumerable<MediaDeviceModel> MediaDeviceList => _mediaDeviceList;

        public MediaDeviceModel SelectedMediaDevice { get; private set; }
        //public MediaDeviceModel SelectedMediaDevice { get; private set; }

        //public string ChosenMediaDeviceName { get; private set; }
        //public string ChosenMediaDeviceSerialId { get; private set; }
        //public DateTime ChosenMediaLastTime { get; private set; }
        private List<RemovableDeviceModel> _removableDeviceList;
        public IEnumerable<RemovableDeviceModel> RemovableDeviceList => _removableDeviceList;
        public RemovableDeviceModel SelectedRemovableDevice { get; private set; }

        //public bool IsResolving { get; private set; }

        public UsbDeviceStore()
        {
            //_usbDeviceLookupWorker = new UsbDeviceLookupWorker();
            _mediaDeviceChangeWorker = new MediaDeviceChangeWorker();
            _removableDeviceChangeWorker = new RemovableDeviceChangeWorker();
            //_collectiveDeviceInfo = new CollectiveDeviceInfoModel(new List<DriveInfoModel>(), new List<MediaDeviceModel>());
            _mediaDeviceList = new List<MediaDeviceModel>();
            _removableDeviceList = new List<RemovableDeviceModel>();

            SelectedMediaDevice = new MediaDeviceModel("Nils sin S20+", "75903ADBED212FFCAF7789C73E685D3D", new DateTime(2019, 01, 01), null);
            //SelectedRemovableDevice = new RemovableDeviceModel("I:", "AA00000000000489", "\\", false);
            SelectedRemovableDevice = new RemovableDeviceModel("I:", "507653BC", "\\", false);
            //ChosenMediaDeviceName = "Nils sin S20+";
            //ChosenMediaDeviceSerialId = "75903ADBED212FFCAF7789C73E685D3D";
            //ChosenMediaLastTime = new DateTime(2019, 01, 01);
            //ChosenMediaDeviceName = "Test Device 123";

            //IsResolving = false;

            UsbDeviceNotifier.UsbRemovableDeviceAdded += UsbDeviceNotifier_UsbRemovableDeviceAdded;
            UsbDeviceNotifier.UsbRemovableDeviceRemoved += UsbDeviceNotifier_UsbRemovableDeviceRemoved;
            UsbDeviceNotifier.UsbMediaDeviceAdded += UsbDeviceNotifier_UsbMediaDeviceAdded;
            UsbDeviceNotifier.UsbMediaDeviceRemoved += UsbDeviceNotifier_UsbMediaDeviceRemoved;

            UpdateUsbInfoLists();
        }

        public void Dispose()
        {
            UsbDeviceNotifier.UsbRemovableDeviceAdded -= UsbDeviceNotifier_UsbRemovableDeviceAdded;
            UsbDeviceNotifier.UsbRemovableDeviceRemoved -= UsbDeviceNotifier_UsbRemovableDeviceRemoved;
            UsbDeviceNotifier.UsbMediaDeviceAdded -= UsbDeviceNotifier_UsbMediaDeviceAdded;
            UsbDeviceNotifier.UsbMediaDeviceRemoved -= UsbDeviceNotifier_UsbMediaDeviceRemoved;
        }

        public void RefreshUsbDevices()
        {
            UpdateUsbInfoLists();
        }

        private void UsbDeviceNotifier_UsbRemovableDeviceAdded()
        {
            Debug.WriteLine("Removable device added");
            //UpdateUsbInfoLists();
            _removableDeviceChangeWorker.StartWorker(DeviceChangeType.Added, _removableDeviceList.Count, RemovableDeviceChangeWorkDone);
        }

        private void UsbDeviceNotifier_UsbRemovableDeviceRemoved()
        {
            Debug.WriteLine("Removable device removed");
            SelectedRemovableDevice = new RemovableDeviceModel(SelectedRemovableDevice.Name, SelectedRemovableDevice.SerialId, SelectedRemovableDevice.Path, false);
            int currentCount = _removableDeviceList.Count;
            _removableDeviceList.Clear();
            DeviceInfoChanged?.Invoke(this);
            //UpdateUsbInfoLists();
            _removableDeviceChangeWorker.StartWorker(DeviceChangeType.Removed, currentCount, RemovableDeviceChangeWorkDone);
        }

        private void UsbDeviceNotifier_UsbMediaDeviceAdded()
        {
            Debug.WriteLine("Media device added");
            _mediaDeviceChangeWorker.StartWorker(DeviceChangeType.Added, _mediaDeviceList.Count, MediaDeviceChangeWorkDone);
            //UpdateUsbInfoLists();
            //UsbMediaDeviceAdded?.Invoke(null);
        }

        private void UsbDeviceNotifier_UsbMediaDeviceRemoved()
        {
            Debug.WriteLine("Media device removed");
            SelectedMediaDevice = new MediaDeviceModel(SelectedMediaDevice.Name, SelectedMediaDevice.SerialId, SelectedMediaDevice.LastRun, null);
            //SelectedMediaDevice = null;
            //_mediaDeviceList = new List<MediaDeviceModel>();
            int currentCount = _mediaDeviceList.Count;
            _mediaDeviceList.Clear();
            DeviceInfoChanged?.Invoke(this);
            //UsbMediaDeviceRemoved?.Invoke();
            //UpdateUsbInfoLists();
            _mediaDeviceChangeWorker.StartWorker(DeviceChangeType.Removed, currentCount, MediaDeviceChangeWorkDone);
        }

        //public void Setup(Window MainWindow)
        //{
        //    HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(MainWindow).Handle);
        //    if (source != null)
        //    {
        //        source.AddHook(HwndHandler);
        //        Debug.WriteLine("Registrering");
        //        //UsbDeviceNotifier.RegisterUsbDeviceNotification(source.Handle);
        //    }
        //}

        //public void Dispose()
        //{
        //    //Debug.WriteLine("Unregistrering");
        //    //UsbDeviceNotifier.UnregisterUsbDeviceNotification();
        //}

        public void SetNewSelectedMediaDevice(string newSelectedMediaName)
        {
            foreach (MediaDeviceModel mediaDevice in MediaDeviceList)
            {
                if (mediaDevice.Name == newSelectedMediaName)
                {
                    SelectedMediaDevice = mediaDevice;
                    //SelectedMediaDevice.Name = chosenMediaName;
                    //ChosenMediaDeviceSerialId = mediaDevice.SerialId;
                    //SelectedMediaDevice = _mediaDeviceList.Find(md => md.SerialId == mediaDevice.SerialId);
                    //Get ChosenDateTime from store
                    // Save new Chosen name and serial id to store
                }
            }
            DeviceInfoChanged?.Invoke(this);
        }

        public void SetNewSelectedRemovableDevice(string newSelectedRemovableName)
        {
            foreach (RemovableDeviceModel removableDevice in RemovableDeviceList)
            {
                if (removableDevice.Name == newSelectedRemovableName)
                {
                    SelectedRemovableDevice = removableDevice;
                    //SelectedMediaDevice.Name = chosenMediaName;
                    //ChosenMediaDeviceSerialId = mediaDevice.SerialId;
                    //SelectedMediaDevice = _mediaDeviceList.Find(md => md.SerialId == mediaDevice.SerialId);
                    //Get ChosenDateTime from store
                    // Save new Chosen name and serial id to store
                }
            }
            DeviceInfoChanged?.Invoke(this);
        }

        //private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        //{
        //    const int WmDevicechange = 0x0219;
        //    const int DbtDevicearrival = 0x8000;
        //    const int DbtDeviceremovecomplete = 0x8004;

        //    if (msg == WmDevicechange)
        //    {
        //        switch ((int)wparam)
        //        {
        //            case DbtDeviceremovecomplete:
        //            case DbtDevicearrival:
        //                string sa = (int)wparam == DbtDevicearrival ? "Arrival" : "Removal";
        //                var deviceTypeBytes = new byte[4];
        //                Marshal.Copy(lparam + 4, deviceTypeBytes, 0, 4); // Get deviceType part of lparam
        //                int deviceType = BitConverter.ToInt32(deviceTypeBytes);
        //                Debug.WriteLine($"{sa} : {deviceType}");
        //                break;
        //        }
        //    }
        //    handled = false;
        //    return IntPtr.Zero;
        //}

        private void UpdateUsbInfoLists()
        {
            //IsResolving = true;
            //_usbDeviceLookupWorker.StartWorker(UpdateUsbInfoListsWorkDone);
            _mediaDeviceChangeWorker.StartWorker(DeviceChangeType.None, 0, MediaDeviceChangeWorkDone);
            _removableDeviceChangeWorker.StartWorker(DeviceChangeType.None, 0, RemovableDeviceChangeWorkDone);
        }

        private void MediaDeviceChangeWorkDone(WorkStatus workStatus, Dictionary<string, MediaDevice> mediaDeviceDict)
        {
            _mediaDeviceList.Clear();
            if (workStatus == WorkStatus.Success)
            {
                foreach (var kv in mediaDeviceDict)
                {
                    _mediaDeviceList.Add(new MediaDeviceModel(kv.Value.FriendlyName, kv.Key, DateTime.MinValue, kv.Value));
                }
                //_mediaDeviceList.Add(new MediaDeviceModel("Test Device 123", "1234567890", DateTime.MinValue, null));

                MediaDeviceModel newSelectedMediaDevice = _mediaDeviceList.Find(md => md.SerialId == SelectedMediaDevice.SerialId);
                if (newSelectedMediaDevice != null)
                {
                    SelectedMediaDevice = newSelectedMediaDevice;
                }
            }
            else
            {
                SelectedMediaDevice = new MediaDeviceModel(SelectedMediaDevice.Name, SelectedMediaDevice.SerialId, SelectedMediaDevice.LastRun, null);
            }
            DeviceInfoChanged?.Invoke(this);
        }

        private void RemovableDeviceChangeWorkDone(WorkStatus workStatus, Dictionary<string, string> removableDeviceDict)
        {
            _removableDeviceList.Clear();
            if (workStatus == WorkStatus.Success)
            {
                foreach (var kv in removableDeviceDict)
                {
                    _removableDeviceList.Add(new RemovableDeviceModel(kv.Value, kv.Key, "\\", true));
                }

                RemovableDeviceModel newSelectedRemovableDevice = _removableDeviceList.Find(rd => rd.SerialId == SelectedRemovableDevice.SerialId);
                if (newSelectedRemovableDevice != null)
                {
                    SelectedRemovableDevice = _removableDeviceList.Find(rd => rd.SerialId == SelectedRemovableDevice.SerialId);
                }
            }
            else
            {
                SelectedRemovableDevice = new RemovableDeviceModel(SelectedRemovableDevice.Name, SelectedRemovableDevice.SerialId, SelectedRemovableDevice.Path, false);
            }
            DeviceInfoChanged?.Invoke(this);
        }

        ////private void UpdateUsbInfoListsWorkDone(WorkStatus workStatus, CollectiveDeviceInfoModel collectiveDeviceInfo)
        //private void UpdateUsbInfoListsWorkDone(WorkStatus workStatus, Dictionary<string, MediaDevice> mediaDeviceDict, Dictionary<string, string> removableDeviceDict)
        //{
        //    _mediaDeviceList.Clear();
        //    _removableDeviceList.Clear();
        //    if (workStatus == WorkStatus.Success)
        //    {
        //        //_collectiveDeviceInfo = collectiveDeviceInfo;
        //        //_mediaDeviceList = mediaDeviceList;
        //        //_mediaDeviceList.Clear();
        //        foreach (var kv in mediaDeviceDict)
        //        {
        //            _mediaDeviceList.Add(new MediaDeviceModel(kv.Value.FriendlyName, kv.Key, DateTime.MinValue, kv.Value));
        //        }
        //        _mediaDeviceList.Add(new MediaDeviceModel("Test Device 123", "1234567890", DateTime.MinValue, null));

        //        MediaDeviceModel newSelectedMediaDevice = _mediaDeviceList.Find(md => md.SerialId == SelectedMediaDevice.SerialId);
        //        if (newSelectedMediaDevice != null)
        //        {
        //            SelectedMediaDevice = newSelectedMediaDevice;
        //        }

        //        foreach (var kv in removableDeviceDict)
        //        {
        //            _removableDeviceList.Add(new RemovableDeviceModel(kv.Value, kv.Key, "\\", true));
        //        }

        //        RemovableDeviceModel newSelectedRemovableDevice = _removableDeviceList.Find(rd => rd.SerialId == SelectedRemovableDevice.SerialId);
        //        if (newSelectedRemovableDevice != null)
        //        {
        //            SelectedRemovableDevice = _removableDeviceList.Find(rd => rd.SerialId == SelectedRemovableDevice.SerialId);
        //        }
        //        //SelectedMediaDevice = _mediaDeviceList.Find(md => md.SerialId == );
        //        //if (SelectedMediaDevice != null)
        //        //{
        //        //    Debug.WriteLine($"SerialId: {SelectedMediaDevice.SerialId}");
        //        //}
        //    }
        //    else
        //    {
        //        //_collectiveDeviceInfo = new CollectiveDeviceInfoModel(new List<DriveInfoModel>(), new List<MediaDeviceModel>());
        //        //_mediaDeviceList.Clear();
        //        //SelectedMediaDevice = null;
        //        SelectedMediaDevice = new MediaDeviceModel(SelectedMediaDevice.Name, SelectedMediaDevice.SerialId, SelectedMediaDevice.LastRun, null);
        //        SelectedRemovableDevice = new RemovableDeviceModel(SelectedRemovableDevice.Name, SelectedRemovableDevice.SerialId, SelectedRemovableDevice.Path, false);
        //    }
        //    //IsResolving = false;
        //    DeviceInfoChanged?.Invoke(this);
        //    //if (eventType == 2)
        //    //{
        //    //    UsbMediaDeviceAdded?.Invoke();
        //    //}
        //}
    }
}
