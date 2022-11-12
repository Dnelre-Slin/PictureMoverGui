using MediaDevices;
using PictureMoverGui.Commands;
using PictureMoverGui.DeviceWorkers;
using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
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
        //private PhoneUnlockPoller _phoneUnlockPoller;
        private MediaDeviceUnlockWorker _usbMediaDeviceUnlockWorker;
        //private ExtensionCounterWorker _extensionCounterWorker;

        public DirectorySelectorLiteViewModel DestinationDirectorySelector { get; }
        public SorterInterfaceViewModel SorterInterface { get; }

        //public IEnumerable<string> ItemChoices => _masterStore.UsbDeviceStore.DriveInfoList.Select(di => di.DriveId);
        public IEnumerable<string> MediaDeviceChoices => _masterStore.UsbDeviceStore.MediaDeviceList.Select(md => md.Name);
        public IEnumerable<string> RemovableDeviceChoices => _masterStore.UsbDeviceStore.RemovableDeviceList.Select(rd => rd.Name);
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

        public string PhoneConnected => _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice != null ? "✔️" : "❌";
        public Brush PhoneConnectedColor => _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice != null ? Brushes.Green : Brushes.Red;
        //public string PhoneUnlocked => _phoneUnlockPoller.IsLocked ? "🔒" : "🔓";
        //public Brush PhoneUnlockedColor => _phoneUnlockPoller.IsLocked ? Brushes.Red : Brushes.Green;       
        public string PhoneUnlocked => _isLocked ? "🔒" : "🔓";
        public Brush PhoneUnlockedColor => _isLocked ? Brushes.Red : Brushes.Green;
        public Visibility PhonePickerVisibility => _masterStore.UsbDeviceStore.MediaDeviceList.Count() > 0 ? Visibility.Visible : Visibility.Hidden;
        public string PhoneChosenName
        {
            get => _masterStore.UsbDeviceStore.SelectedMediaDevice.Name;
            set
            {
                if (value != null && value != _masterStore.UsbDeviceStore.SelectedMediaDevice.Name)
                {
                    _masterStore.UsbDeviceStore.SetNewSelectedMediaDevice(value);
                }
            }
        }
        public string LastRunDateTime => _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.ToString();

        //=> _masterStore.UsbDeviceStore.ChosenMediaDeviceName;

        public string RemovableDeviceConnected => _masterStore.UsbDeviceStore.SelectedRemovableDevice.IsConnected ? "✔️" : "❌";
        public Brush RemovableDeviceConnectedColor => _masterStore.UsbDeviceStore.SelectedRemovableDevice.IsConnected ? Brushes.Green : Brushes.Red;
        public Visibility RemovableDevicePickerVisibility => _masterStore.UsbDeviceStore.RemovableDeviceList.Count() > 0 ? Visibility.Visible : Visibility.Hidden;
        public string RemovableDeviceChosenName
        {
            get => _masterStore.UsbDeviceStore.SelectedRemovableDevice.Name;
            set
            {
                if (value != null && value != _masterStore.UsbDeviceStore.SelectedRemovableDevice.Name)
                {
                    _masterStore.UsbDeviceStore.SetNewSelectedRemovableDevice(value);
                }
            }
        }

        public int InfoFileCount => _masterStore.RunningStore.InfoFileCount;

        public ICommand RefreshDevices { get; }
        public ICommand ConnectDevice { get; }
        public ICommand RefreshUsbDevices { get; }
        public ICommand CancelGatherer { get; }

        public PhoneInputViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _usbMediaDeviceUnlockWorker = new MediaDeviceUnlockWorker();
            //_extensionCounterWorker = new ExtensionCounterWorker();

            RefreshDevices = new CallbackCommand(OnRefreshDevices);
            ConnectDevice = new CallbackCommand(OnConnectDevice);
            RefreshUsbDevices = new CallbackCommand(OnRefreshUsbDevices);
            CancelGatherer = new CallbackCommand(OnExtensionCounterWorkerCancel);

            DestinationDirectorySelector = new DirectorySelectorLiteViewModel(masterStore);
            SorterInterface = new SorterInterfaceViewModel(masterStore);

            //ChosenString = "hello";

            _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            StartUnlockWorker();

            //_phoneUnlockPoller = new PhoneUnlockPoller(_masterStore, OnPhoneUnlockChange);
        }

        public override void Dispose()
        {
            base.Dispose();

            DestinationDirectorySelector.Dispose();
            SorterInterface.Dispose();

            _masterStore.UsbDeviceStore.DeviceInfoChanged -= UsbDeviceStore_DeviceInfoChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;

            //_phoneUnlockPoller.Dispose();
        }

        private void UsbDeviceStore_DeviceInfoChanged(UsbDeviceStore usbDeviceStore)
        {
            System.Diagnostics.Debug.WriteLine($"Device change: PhoneConnected: {PhoneConnected}");
            System.Diagnostics.Debug.WriteLine($"Device change: PhoneChosenName: {PhoneChosenName}");
            System.Diagnostics.Debug.WriteLine($"Device change: RemovableDeviceConnected: {RemovableDeviceConnected}");
            foreach (var ic in MediaDeviceChoices)
            {
                System.Diagnostics.Debug.WriteLine($"Device change: ItemChoices: {ic}");
            }

            if (_masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice == null)
            {
                OnExtensionCounterWorkerCancel(null);
                _isLocked = true;
                OnPropertyChanged(nameof(PhoneUnlocked));
                OnPropertyChanged(nameof(PhoneUnlockedColor));
            }

            StartUnlockWorker();
            StopUnlockWorker();

            OnPropertyChanged(nameof(MediaDeviceChoices));
            OnPropertyChanged(nameof(RemovableDeviceChoices));

            OnPropertyChanged(nameof(PhoneConnected));
            OnPropertyChanged(nameof(PhoneConnectedColor));
            OnPropertyChanged(nameof(PhonePickerVisibility));
            OnPropertyChanged(nameof(PhoneChosenName));

            OnPropertyChanged(nameof(RemovableDeviceConnected));
            OnPropertyChanged(nameof(RemovableDeviceConnectedColor));
            OnPropertyChanged(nameof(RemovableDevicePickerVisibility));
            OnPropertyChanged(nameof(RemovableDeviceChosenName));
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

        private void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(InfoFileCount));
        }

        private void StartUnlockWorker()
        {
            if (_masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice != null && _isLocked)
            {
                _usbMediaDeviceUnlockWorker.StartWorker(_masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice, OnUnlockWorkerDone);
            }
        }

        private void StopUnlockWorker()
        {
            if (_masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice == null)
            {
                _usbMediaDeviceUnlockWorker.CancelWorker();
            }
        }

        private void OnUnlockWorkerDone(WorkStatus workStatus)
        {
            System.Diagnostics.Debug.WriteLine($"OnUnlockWorkerDone : {workStatus}");
            if (workStatus == WorkStatus.Success)
            {
                _isLocked = false;
                StartExtensionCountnerWorker();
                OnPropertyChanged(nameof(PhoneUnlocked));
                OnPropertyChanged(nameof(PhoneUnlockedColor));
            }
        }

        //private void OnPhoneUnlockChange(bool phoneLocked)
        //{
        //    System.Diagnostics.Debug.WriteLine($"OnPhoneUnlockChange : {phoneLocked}");
        //    _isLocked = phoneLocked;
        //    OnPropertyChanged(nameof(PhoneUnlocked));
        //    OnPropertyChanged(nameof(PhoneUnlockedColor));
        //}

        protected void OnRefreshDevices(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnRefreshDevices");
            //OnPropertyChanged(nameof(PhoneStuff2));
        }
        protected void OnConnectDevice(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnConnectDevice");
        }
        protected void OnRefreshUsbDevices(object parameter)
        {
            _masterStore.UsbDeviceStore.RefreshUsbDevices();
        }

        protected void StartExtensionCountnerWorker()
        {
            if (!_isLocked && _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType == MediaTypeEnum.MediaDevice)
            {
                _masterStore.RunningStore.WorkerHandler.CancelExtensionCounterWorker();
                _masterStore.RunningStore.ResetInfoFileCount();
                _masterStore.FileExtensionStore.Clear(); // Clear old extensions
                _masterStore.RunningStore.WorkerHandler.StartExtensionCounterWorker(new ExtensionCounterArguments(
                    _masterStore.RunningStore.RunState,
                    MediaTypeEnum.MediaDevice,
                    null,
                    _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice,
                    _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun,
                    //DateTime.MinValue,
                    //_masterStore.RunningStore.SetGathererState,
                    _masterStore.RunningStore.IncrementInfoFileCount,
                    OnExtensionCounterWorkerDone
                ));
            }
        }

        protected void OnExtensionCounterWorkerDone(WorkStatus workStatus, Dictionary<string, int> extensionInfo)
        {
            System.Diagnostics.Debug.WriteLine("Worker done!");
            System.Diagnostics.Debug.WriteLine(workStatus);
            switch (workStatus)
            {
                case WorkStatus.Unfinished:
                    _masterStore.FileExtensionStore.Clear();
                    //_masterStore.SorterConfigurationStore.SetSourcePath("");
                    System.Diagnostics.Debug.WriteLine("Work status unfinished!");
                    break;
                case WorkStatus.Success:
                    _masterStore.FileExtensionStore.Set(extensionInfo);
                    break;
                case WorkStatus.Invalid:
                    _masterStore.FileExtensionStore.Clear();
                    //_masterStore.SorterConfigurationStore.SetSourcePath("");
                    System.Diagnostics.Debug.WriteLine("The source was invald");
                    break;
                case WorkStatus.Cancelled:
                    _masterStore.FileExtensionStore.Clear();
                    //_masterStore.SorterConfigurationStore.SetSourcePath("");
                    break;
                case WorkStatus.Interupted:
                    _masterStore.FileExtensionStore.Clear();
                    StartExtensionCountnerWorker(); // Rerun, as this is what should be done on interupts
                    break;
                default:
                    throw new NotImplementedException("Switch case in OnExtensionCounterWorkerDone does not handle all cases");
            }
        }

        protected void OnExtensionCounterWorkerCancel(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnCancelGatherer");
            //_extensionCounterWorker.CancelWorker();
            _masterStore.RunningStore.WorkerHandler.CancelExtensionCounterWorker();
        }
    }
}
