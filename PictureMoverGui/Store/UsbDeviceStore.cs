using MediaDevices;
using PictureMoverGui.DeviceWorkers;
using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.StoreHelpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public event Action<UsbDeviceStore> DeviceInfoChanged;

        private MediaDeviceChangeWorker _mediaDeviceChangeWorker;
        private RemovableDeviceChangeWorker _removableDeviceChangeWorker;

        private List<MediaDeviceStorage> _mediaDeviceStorageList;
        private List<MediaDeviceModel> _mediaDeviceList;
        public IEnumerable<MediaDeviceModel> MediaDeviceList => _mediaDeviceList;
        public MediaDeviceModel SelectedMediaDevice { get; private set; }

        private List<RemovableDeviceStorage> _removableDeviceStorageList;
        private List<RemovableDeviceModel> _removableDeviceList;
        public IEnumerable<RemovableDeviceModel> RemovableDeviceList => _removableDeviceList;
        public RemovableDeviceModel SelectedRemovableDevice { get; private set; }

        public UsbDeviceStore()
        {
            _mediaDeviceChangeWorker = new MediaDeviceChangeWorker();
            _removableDeviceChangeWorker = new RemovableDeviceChangeWorker();

            _mediaDeviceStorageList = new List<MediaDeviceStorage>();
            _mediaDeviceList = new List<MediaDeviceModel>();
            _removableDeviceStorageList = new List<RemovableDeviceStorage>();
            _removableDeviceList = new List<RemovableDeviceModel>();

            SelectedMediaDevice = new MediaDeviceModel(null, null, DateTime.MinValue, null);
            SelectedRemovableDevice = new RemovableDeviceModel(null, null, null, false);

            LoadFromStore();

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

            _mediaDeviceChangeWorker.CancelWorker();
            _removableDeviceChangeWorker.CancelWorker();
        }

        public void RefreshUsbDevices()
        {
            UpdateUsbInfoLists();
        }

        private void UsbDeviceNotifier_UsbRemovableDeviceAdded()
        {
            Debug.WriteLine("Removable device added");
            _removableDeviceChangeWorker.StartWorker(DeviceChangeType.Added, _removableDeviceList.Count, RemovableDeviceChangeWorkDone);
        }

        private void UsbDeviceNotifier_UsbRemovableDeviceRemoved()
        {
            Debug.WriteLine("Removable device removed");
            SelectedRemovableDevice = new RemovableDeviceModel(SelectedRemovableDevice.Name, SelectedRemovableDevice.SerialId, SelectedRemovableDevice.Path, false);
            int currentCount = _removableDeviceList.Count;
            _removableDeviceList.Clear();
            DeviceInfoChanged?.Invoke(this);
            _removableDeviceChangeWorker.StartWorker(DeviceChangeType.Removed, currentCount, RemovableDeviceChangeWorkDone);
        }

        private void UsbDeviceNotifier_UsbMediaDeviceAdded()
        {
            Debug.WriteLine("Media device added");
            _mediaDeviceChangeWorker.StartWorker(DeviceChangeType.Added, _mediaDeviceList.Count, MediaDeviceChangeWorkDone);
        }

        private void UsbDeviceNotifier_UsbMediaDeviceRemoved()
        {
            Debug.WriteLine("Media device removed");
            SelectedMediaDevice = new MediaDeviceModel(SelectedMediaDevice.Name, SelectedMediaDevice.SerialId, SelectedMediaDevice.LastRun, null);
            int currentCount = _mediaDeviceList.Count;
            _mediaDeviceList.Clear();
            DeviceInfoChanged?.Invoke(this);
            _mediaDeviceChangeWorker.StartWorker(DeviceChangeType.Removed, currentCount, MediaDeviceChangeWorkDone);
        }

        public void SetNewSelectedMediaDevice(string newSelectedMediaName)
        {
            foreach (MediaDeviceModel mediaDevice in MediaDeviceList)
            {
                if (mediaDevice.Name == newSelectedMediaName)
                {
                    SelectedMediaDevice = mediaDevice;
                    if (_mediaDeviceStorageList.Find(mds => mds.SerialId == SelectedMediaDevice.SerialId) == null)
                    {
                        _mediaDeviceStorageList.Add(new MediaDeviceStorage(SelectedMediaDevice.SerialId, SelectedMediaDevice.LastRun));
                    }
                    SaveToStore();
                }
            }
            DeviceInfoChanged?.Invoke(this);
        }

        public void SetSelectedMediaDeviceDateTime(DateTime dateTime)
        {
            if (SelectedMediaDevice != null)
            {
                SelectedMediaDevice = new MediaDeviceModel(SelectedMediaDevice.Name, SelectedMediaDevice.SerialId, dateTime, SelectedMediaDevice.MediaDevice);
                _mediaDeviceStorageList.Find(mds => mds.SerialId == SelectedMediaDevice.SerialId).LastRun = dateTime;
                SaveToStore();
                DeviceInfoChanged?.Invoke(this);
            }
        }

        public void SetNewSelectedRemovableDevice(string newSelectedRemovableName)
        {
            foreach (RemovableDeviceModel removableDevice in RemovableDeviceList)
            {
                if (removableDevice.Name == newSelectedRemovableName)
                {
                    SelectedRemovableDevice = removableDevice;
                    if (_removableDeviceStorageList.Find(rds => rds.SerialId == SelectedRemovableDevice.SerialId) == null)
                    {
                        _removableDeviceStorageList.Add(new RemovableDeviceStorage(SelectedRemovableDevice.SerialId, SelectedRemovableDevice.Path));
                    }
                    SaveToStore();
                }
            }
            DeviceInfoChanged?.Invoke(this);
        }

        public void SetSelectedRemovableDevicePath(string path)
        {
            if (SelectedRemovableDevice != null)
            {
                SelectedRemovableDevice = new RemovableDeviceModel(SelectedRemovableDevice.Name, SelectedRemovableDevice.SerialId, path, SelectedRemovableDevice.IsConnected);
                _removableDeviceStorageList.Find(rds => rds.SerialId == SelectedRemovableDevice.SerialId).Path = path;
                SaveToStore();
                DeviceInfoChanged?.Invoke(this);
            }
        }

        private void UpdateUsbInfoLists()
        {
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
                    _mediaDeviceList.Add(new MediaDeviceModel(kv.Value.FriendlyName, kv.Key, _mediaDeviceStorageList.Find(md => md.SerialId == kv.Key)?.LastRun ?? DateTime.MinValue, kv.Value));
                }

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
                    _removableDeviceList.Add(new RemovableDeviceModel(kv.Value, kv.Key, _removableDeviceStorageList.Find(rd => rd.SerialId == kv.Key)?.Path ?? "\\", true));
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

        private void SaveToStore()
        {
            StringCollection mList = new StringCollection();
            foreach (MediaDeviceStorage mds in _mediaDeviceStorageList)
            {
                mList.Add(mds.SerialId);
                mList.Add(mds.LastRun.ToString());
            }
            Properties.Datastore.Default.MediaDeviceStorageList = mList;

            StringCollection rList = new StringCollection();
            foreach (RemovableDeviceStorage rds in _removableDeviceStorageList)
            {
                rList.Add(rds.SerialId);
                rList.Add(rds.Path);
            }
            Properties.Datastore.Default.RemovableDeviceStorageList = rList;
            Properties.Datastore.Default.Save();

            Properties.Persistant.Default.MediaDeviceName = SelectedMediaDevice.Name;
            Properties.Persistant.Default.MediaDeviceSerialNr = SelectedMediaDevice.SerialId;
            Properties.Persistant.Default.RemovableDeviceName = SelectedRemovableDevice.Name;
            Properties.Persistant.Default.RemovableDeviceSerialNr = SelectedRemovableDevice.SerialId;
            Properties.Persistant.Default.Save();
        }

        private void LoadFromStore()
        {
            _mediaDeviceStorageList.Clear();
            StringCollection mList = Properties.Datastore.Default.MediaDeviceStorageList;
            if (mList.Count % 2 != 0)
            {
                throw new ArrayTypeMismatchException("MediaDeviceStorageList should be stored in 2s. The current MediaDeviceStorageList has not been, so it cannot be loaded");
            }
            for (int i = 0; i < mList.Count; i += 2)
            {
                _mediaDeviceStorageList.Add(new MediaDeviceStorage(mList[i], DateTime.Parse(mList[i + 1])));
            }

            _removableDeviceStorageList.Clear();
            StringCollection rList = Properties.Datastore.Default.RemovableDeviceStorageList;
            if (rList.Count % 2 != 0)
            {
                throw new ArrayTypeMismatchException("RemovableDeviceStorageList should be stored in 2s. The current RemovableDeviceStorageList has not been, so it cannot be loaded");
            }
            for (int i = 0; i < rList.Count; i += 2)
            {
                _removableDeviceStorageList.Add(new RemovableDeviceStorage(rList[i], rList[i + 1]));
            }

            string mediaDeviceName = Properties.Persistant.Default.MediaDeviceName;
            string mediaDeviceSerialNr = Properties.Persistant.Default.MediaDeviceSerialNr;

            SelectedMediaDevice = new MediaDeviceModel(mediaDeviceName, mediaDeviceSerialNr, _mediaDeviceStorageList.Find(md => md.SerialId == mediaDeviceSerialNr)?.LastRun??DateTime.MinValue, null);

            string removableDeviceName = Properties.Persistant.Default.RemovableDeviceName;
            string removableDeviceSerialNr = Properties.Persistant.Default.RemovableDeviceSerialNr;

            SelectedRemovableDevice = new RemovableDeviceModel(removableDeviceName, removableDeviceSerialNr, _removableDeviceStorageList.Find(rd => rd.SerialId == removableDeviceSerialNr)?.Path??"\\", false);
        }
    }
}
