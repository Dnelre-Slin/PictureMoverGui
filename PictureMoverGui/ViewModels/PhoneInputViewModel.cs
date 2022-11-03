using MediaDevices;
using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
//using System.Management;
using System.Windows.Input;
using System.Windows.Interop;

namespace PictureMoverGui.ViewModels
{
    public class PhoneInputViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        //private MediaDevice _mediaDevice;
        //private Timer _checkLockTimer;
        private PhoneUnlockPoller _phoneUnlockPoller;

        public IEnumerable<string> ItemChoices => _masterStore.UsbDeviceStore.DriveInfoList.Select(di => di.DriveId);
        public string ChosenString { get; set; }

        //public string PhoneStuff => _masterStore.UsbDeviceStore.MediaDeviceList.Select(md => md.FriendlyName).ToList().Contains("Nils sin S20+") ? "Connected" : "Not connected";
        public string PhoneStuff => _masterStore.UsbDeviceStore.SelectedMediaDevice != null ? "Connected" : "Not connected";

        private bool _isLocked = true;
        public string PhoneStuff2 => _isLocked ? "Locked" : "Unlocked";

        public ICommand RefreshDevices { get; }
        public ICommand ConnectDevice { get; }

        public PhoneInputViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            ChosenString = "hello";

            RefreshDevices = new CallbackCommand(OnRefreshDevices);
            ConnectDevice = new CallbackCommand(OnConnectDevice);

            _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;

            //_checkLockTimer = new Timer(CheckIfLocked, null, 1000, 1000);
            _phoneUnlockPoller = new PhoneUnlockPoller(_masterStore, OnPhoneUnlockChange);

            //foreach (MediaDevice mediaDevice in MediaDevice.GetDevices())
            //{
            //    System.Diagnostics.Debug.WriteLine($"|{mediaDevice.FriendlyName}|");
            //    if (mediaDevice.FriendlyName == "Nils sin S20+")
            //    {
            //        System.Diagnostics.Debug.WriteLine(mediaDevice.FriendlyName);
            //        System.Diagnostics.Debug.WriteLine(mediaDevice.DeviceId);
            //        //System.Diagnostics.Debug.WriteLine(mediaDevice.Protocol);
            //        //System.Diagnostics.Debug.WriteLine(mediaDevice.SerialNumber);                
            //        System.Diagnostics.Debug.WriteLine(mediaDevice.IsConnected);
            //        mediaDevice.Connect();
            //        System.Diagnostics.Debug.WriteLine(mediaDevice.Protocol);
            //        System.Diagnostics.Debug.WriteLine(mediaDevice.SerialNumber);
            //        System.Diagnostics.Debug.WriteLine(mediaDevice.Model);
            //        System.Diagnostics.Debug.WriteLine(mediaDevice.Manufacturer);

            //        mediaDevice.Disconnect();
            //        System.Diagnostics.Debug.WriteLine("------------------------------");

            //        mediaDevice.DeviceCapabilitiesUpdated += MediaDevice_DeviceCapabilitiesUpdated;
            //        mediaDevice.DeviceRemoved += MediaDevice_DeviceRemoved;
            //        mediaDevice.DeviceReset += MediaDevice_DeviceReset;
            //        mediaDevice.ObjectAdded += MediaDevice_ObjectAdded;
            //        mediaDevice.ObjectRemoved += MediaDevice_ObjectRemoved;
            //        mediaDevice.ObjectTransferRequest += MediaDevice_ObjectTransferRequest;
            //        mediaDevice.ObjectUpdated += MediaDevice_ObjectUpdated;
            //        mediaDevice.ServiceMethodComplete += MediaDevice_ServiceMethodComplete;
            //        mediaDevice.StorageFormat += MediaDevice_StorageFormat;
            //        _mediaDevice = mediaDevice;
            //    }
            //}
        }

        //private void CheckIfLocked(object stateInfo)
        //{
        //    int count = 0;
        //    if (_masterStore.UsbDeviceStore.SelectedMediaDevice != null && !_masterStore.UsbDeviceStore.IsResolving)
        //    {
        //        try
        //        {
        //            _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice.Connect();
        //            count = _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice.GetDrives().Count();
        //            _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice.Disconnect();
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine(ex.Message);
        //        }
        //    }
        //    //foreach (MediaDeviceModel mediaDeviceModel in _masterStore.UsbDeviceStore.MediaDeviceList)
        //    //{
        //    //    try
        //    //    {
        //    //        mediaDeviceModel.MediaDevice.Connect();
        //    //        count = mediaDeviceModel.MediaDevice.GetDrives().Count();
        //    //        mediaDeviceModel.MediaDevice.Disconnect();
        //    //    }
        //    //    catch(Exception ex)
        //    //    {
        //    //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //    //    }
        //    //}
        //    bool locked = count == 0;
        //    if (_isLocked != locked)
        //    {
        //        if (!locked)
        //        {
        //            _checkLockTimer.Dispose();
        //            _checkLockTimer = null;
        //        }
        //        _isLocked = locked;
        //        OnPropertyChanged(nameof(PhoneStuff2));
        //    }
        //}

        private void UsbDeviceStore_DeviceInfoChanged(CollectiveDeviceInfoModel collectiveDeviceInfo)
        {
            //CheckIfLocked(null);
            OnPropertyChanged(nameof(ItemChoices));
            OnPropertyChanged(nameof(PhoneStuff));
            foreach (var drive in collectiveDeviceInfo.DriveInfoList)
            {
                System.Diagnostics.Debug.WriteLine($"{drive.DriveId} : {drive.SerialId}");
            }
            foreach (var media in collectiveDeviceInfo.MediaDeviceList)
            {
                System.Diagnostics.Debug.WriteLine($"{media.FriendlyName} : {media.SerialId}");
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            _phoneUnlockPoller.Dispose();

            //_masterStore.UsbDeviceStore.DeviceInfoChanged -= UsbDeviceStore_DeviceInfoChanged;

            //if (_checkLockTimer != null)
            //{
            //    _checkLockTimer.Dispose();
            //}

            //if (_mediaDevice != null)
            //{
            //    _mediaDevice.DeviceCapabilitiesUpdated -= MediaDevice_DeviceCapabilitiesUpdated;
            //    _mediaDevice.DeviceRemoved -= MediaDevice_DeviceRemoved;
            //    _mediaDevice.DeviceReset -= MediaDevice_DeviceReset;
            //    _mediaDevice.ObjectAdded -= MediaDevice_ObjectAdded;
            //    _mediaDevice.ObjectRemoved -= MediaDevice_ObjectRemoved;
            //    _mediaDevice.ObjectTransferRequest -= MediaDevice_ObjectTransferRequest;
            //    _mediaDevice.ObjectUpdated -= MediaDevice_ObjectUpdated;
            //    _mediaDevice.ServiceMethodComplete -= MediaDevice_ServiceMethodComplete;
            //    _mediaDevice.StorageFormat -= MediaDevice_StorageFormat;
            //}
        }

        private void OnPhoneUnlockChange(bool phoneLocked)
        {
            _isLocked = phoneLocked;
            OnPropertyChanged(nameof(PhoneStuff2));
        }

        //private void MediaDevice_StorageFormat(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_DeviceCapabilitiesUpdated");
        //}

        //private void MediaDevice_ServiceMethodComplete(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_ServiceMethodComplete");
        //}

        //private void MediaDevice_ObjectUpdated(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_ObjectUpdated");
        //}

        //private void MediaDevice_ObjectTransferRequest(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_ObjectTransferRequest");
        //}

        //private void MediaDevice_ObjectRemoved(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_ObjectRemoved");
        //}

        //private void MediaDevice_ObjectAdded(object sender, ObjectAddedEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_ObjectAdded");
        //}

        //private void MediaDevice_DeviceReset(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_DeviceReset");
        //}

        //private void MediaDevice_DeviceRemoved(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_DeviceRemoved");
        //}

        //private void MediaDevice_DeviceCapabilitiesUpdated(object sender, MediaDeviceEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("MediaDevice_DeviceCapabilitiesUpdated");
        //}

        protected void OnRefreshDevices(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnRefreshDevices");
            OnPropertyChanged(nameof(PhoneStuff2));
            //ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            //ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            //ManagementClass logicalDisks = new ManagementClass("Win32_LogicalDisk");
            //{
            //    foreach (ManagementObject ld in logicalDisks.GetInstances())
            //    {
            //        System.Diagnostics.Debug.WriteLine(ld["DeviceId"]?.ToString());
            //        //if (mo["SerialNumber"]?.ToString() == "AA00000000000489")
            //        {
            //            foreach (ManagementObject dp in ld.GetRelated("Win32_DiskPartition"))
            //            {
            //                foreach (ManagementObject dd in dp.GetRelated("Win32_Diskdrive"))
            //                {
            //                    System.Diagnostics.Debug.WriteLine(dd["SerialNumber"]?.ToString());
            //                }
            //            }
            //        }
            //        System.Diagnostics.Debug.WriteLine("------------------------------");
            //    }
            //}
            //ManagementClass devs = new ManagementClass("Win32_Diskdrive");
            //{
            //    foreach(ManagementObject mo in devs.GetInstances())
            //    {
            //        System.Diagnostics.Debug.WriteLine(mo["SerialNumber"]?.ToString());
            //        //if (mo["SerialNumber"]?.ToString() == "AA00000000000489")
            //        {
            //            foreach (ManagementObject b in mo.GetRelated("Win32_DiskPartition"))
            //            {
            //                foreach (ManagementObject c in b.GetRelated("Win32_LogicalDisk"))
            //                {
            //                    System.Diagnostics.Debug.WriteLine(c["VolumeName"]?.ToString());
            //                    System.Diagnostics.Debug.WriteLine(c["DeviceId"]?.ToString());
            //                }
            //            }
            //        }
            //        System.Diagnostics.Debug.WriteLine("------------------------------");
            //    }
            //}
            //foreach (ManagementObject wmi_HD in moSearcher.Get())
            //{
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["Model"].ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["InterfaceType"].ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["SerialNumber"].ToString());
            //    System.Diagnostics.Debug.WriteLine(wmi_HD["Name"].ToString());
            //    System.Diagnostics.Debug.WriteLine(wmi_HD["DeviceID"].ToString());
            //    System.Diagnostics.Debug.WriteLine(wmi_HD["PNPDeviceID"]?.ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["Description"].ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["SystemName"].ToString());
            //    System.Diagnostics.Debug.WriteLine("------------------------------");

            //}
            //foreach (var v in System.IO.DriveInfo.GetDrives())
            //{
            //    System.Diagnostics.Debug.WriteLine(v.Name);
            //    System.Diagnostics.Debug.WriteLine(v.DriveType);
            //    //System.Diagnostics.Debug.WriteLine(v.DriveFormat);
            //    System.Diagnostics.Debug.WriteLine(v.IsReady);
            //    System.Diagnostics.Debug.WriteLine("------------------------------");
            //}
            //foreach (MediaDevice mediaDevice in MediaDevice.GetDevices())
            //{
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.FriendlyName);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.DeviceId);
            //    //System.Diagnostics.Debug.WriteLine(mediaDevice.Protocol);
            //    //System.Diagnostics.Debug.WriteLine(mediaDevice.SerialNumber);                
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.IsConnected);
            //    mediaDevice.Connect();
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.Protocol);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.SerialNumber);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.Model);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.Manufacturer);

            //    mediaDevice.Disconnect();
            //    System.Diagnostics.Debug.WriteLine("------------------------------");
            //}
        }
        protected void OnConnectDevice(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnConnectDevice");
        }
    }
}
