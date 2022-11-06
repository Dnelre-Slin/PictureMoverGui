using MediaDevices;
using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.SubViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
//using System.Management;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace PictureMoverGui.ViewModels
{
    public class PhoneInputViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        private PhoneUnlockPoller _phoneUnlockPoller;

        public DirectorySelectorLiteViewModel DestinationDirectorySelector { get; }
        public SorterInterfaceViewModel SorterInterface { get; }

        //public IEnumerable<string> ItemChoices => _masterStore.UsbDeviceStore.DriveInfoList.Select(di => di.DriveId);
        public IEnumerable<string> ItemChoices => _masterStore.UsbDeviceStore.MediaDeviceList.Select(md => md.FriendlyName);
        //private string _chosenString;
        //public string ChosenString
        //{
        //    get => String.IsNullOrEmpty(_chosenString) ? _masterStore.UsbDeviceStore.ChosenMediaDeviceName : _chosenString;
        //    set
        //    {
        //        System.Diagnostics.Debug.WriteLine("HELOO???");
        //        if (value != null && value != _masterStore.UsbDeviceStore.ChosenMediaDeviceName)
        //        {
        //            _masterStore.UsbDeviceStore.SetNewChosenMediaName(value);
        //        }
        //        if (_chosenString != value)
        //        {
        //            _chosenString = value;
        //            OnPropertyChanged(nameof(ChosenString));
        //        }
        //    }
        //}        
        //public string ChosenString
        //{
        //    get => _masterStore.UsbDeviceStore.ChosenMediaDeviceName;
        //    set
        //    {
        //        if (value != null && value != _masterStore.UsbDeviceStore.ChosenMediaDeviceName)
        //        {
        //            _masterStore.UsbDeviceStore.SetNewChosenMediaName(value);
        //        }
        //        //if (_chosenString != value)
        //        //{
        //        //    _chosenString = value;
        //        //    OnPropertyChanged(nameof(ChosenString));
        //        //}
        //    }
        //}

        //public string PhoneStuff => _masterStore.UsbDeviceStore.SelectedMediaDevice != null ? "Connected" : "Not connected";

        private bool _isLocked = true;
        //public string PhoneStuff2 => _phoneUnlockPoller.IsLocked ? "Locked" : "Unlocked";

        //public string PhoneStuff
        //{
        //    get
        //    {
        //        string s1 = _masterStore.UsbDeviceStore.SelectedMediaDevice != null ? "✔️" : "❌";
        //        string s2 = _phoneUnlockPoller.IsLocked ? "🔒" : "🔓";
        //        return $"{s1} {s2}";
        //    }
        //}

        public string PhoneConnected => _masterStore.UsbDeviceStore.SelectedMediaDevice != null ? "✔️" : "❌";
        public Brush PhoneConnectedColor => _masterStore.UsbDeviceStore.SelectedMediaDevice != null ? Brushes.Green : Brushes.Red;
        //public string PhoneUnlocked => _phoneUnlockPoller.IsLocked ? "🔒" : "🔓";
        //public Brush PhoneUnlockedColor => _phoneUnlockPoller.IsLocked ? Brushes.Red : Brushes.Green;       
        public string PhoneUnlocked => _isLocked ? "🔒" : "🔓";
        public Brush PhoneUnlockedColor => _isLocked ? Brushes.Red : Brushes.Green;
        public Visibility PhonePickerVisibility => _masterStore.UsbDeviceStore.MediaDeviceList.Count() > 0 ? Visibility.Visible : Visibility.Hidden;
        public string PhoneChosenName
        {
            get => _masterStore.UsbDeviceStore.ChosenMediaDeviceName;
            set
            {
                if (value != null && value != _masterStore.UsbDeviceStore.ChosenMediaDeviceName)
                {
                    _masterStore.UsbDeviceStore.SetNewChosenMediaName(value);
                }
            }
        }
        public string LastRunDateTime => _masterStore.UsbDeviceStore.ChosenMediaLastTime.ToString();
            
            //=> _masterStore.UsbDeviceStore.ChosenMediaDeviceName;

        public ICommand RefreshDevices { get; }
        public ICommand ConnectDevice { get; }

        public PhoneInputViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            DestinationDirectorySelector = new DirectorySelectorLiteViewModel(masterStore);
            SorterInterface = new SorterInterfaceViewModel(masterStore);

            //ChosenString = "hello";

            RefreshDevices = new CallbackCommand(OnRefreshDevices);
            ConnectDevice = new CallbackCommand(OnConnectDevice);

            _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;

            //_phoneUnlockPoller = new PhoneUnlockPoller(_masterStore, OnPhoneUnlockChange);
        }

        public override void Dispose()
        {
            base.Dispose();

            DestinationDirectorySelector.Dispose();
            SorterInterface.Dispose();

            //_phoneUnlockPoller.Dispose();
        }

        private void UsbDeviceStore_DeviceInfoChanged(CollectiveDeviceInfoModel collectiveDeviceInfo)
        {
            System.Diagnostics.Debug.WriteLine($"Device change: PhoneConnected: {PhoneConnected}");
            System.Diagnostics.Debug.WriteLine($"Device change: PhoneChosenName: {PhoneChosenName}");
            foreach (var ic in ItemChoices)
            {
                System.Diagnostics.Debug.WriteLine($"Device change: ItemChoices: {ic}");
            }
            OnPropertyChanged(nameof(ItemChoices));
            OnPropertyChanged(nameof(PhoneConnected));
            OnPropertyChanged(nameof(PhoneConnectedColor));
            OnPropertyChanged(nameof(PhonePickerVisibility));
            OnPropertyChanged(nameof(PhoneChosenName));
            //OnPropertyChanged(nameof(ChosenString));
            //foreach (var drive in collectiveDeviceInfo.DriveInfoList)
            //{
            //    System.Diagnostics.Debug.WriteLine($"{drive.DriveId} : {drive.SerialId}");
            //}
            //foreach (var media in collectiveDeviceInfo.MediaDeviceList)
            //{
            //    System.Diagnostics.Debug.WriteLine($"{media.FriendlyName} : {media.SerialId}");
            //}
        }

        private void OnPhoneUnlockChange(bool phoneLocked)
        {
            System.Diagnostics.Debug.WriteLine($"OnPhoneUnlockChange : {phoneLocked}");
            _isLocked = phoneLocked;
            OnPropertyChanged(nameof(PhoneUnlocked));
            OnPropertyChanged(nameof(PhoneUnlockedColor));
        }

        protected void OnRefreshDevices(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnRefreshDevices");
            //OnPropertyChanged(nameof(PhoneStuff2));
        }
        protected void OnConnectDevice(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnConnectDevice");
        }
    }
}
