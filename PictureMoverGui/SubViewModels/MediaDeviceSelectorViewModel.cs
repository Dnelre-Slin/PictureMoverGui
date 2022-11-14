using PictureMoverGui.Commands;
using PictureMoverGui.DeviceWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PictureMoverGui.SubViewModels
{
    public class MediaDeviceSelectorViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        private MediaDeviceUnlockWorker _usbMediaDeviceUnlockWorker;

        public IEnumerable<string> MediaDeviceChoices => _masterStore.UsbDeviceStore.MediaDeviceList.Select(md => md.Name);

        private bool _isLocked = true;

        public string MediaDeviceConnected => _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice != null ? "✔️" : "❌";
        public Brush MediaDeviceConnectedColor => _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice != null ? Brushes.Green : Brushes.Red;
     
        public string MediaDeviceUnlocked => _isLocked ? "🔒" : "🔓";
        public Brush MediaDeviceUnlockedColor => _isLocked ? Brushes.Red : Brushes.Green;
        public Visibility MediaDevicePickerVisibility => _masterStore.UsbDeviceStore.MediaDeviceList.Count() > 0 ? Visibility.Visible : Visibility.Hidden;
        public string MediaDeviceChosenName
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

        public int InfoFileCount => _masterStore.RunningStore.InfoFileCount;

        public ICommand RefreshUsbDevices { get; }
        public ICommand CancelGatherer { get; }

        public MediaDeviceSelectorViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _usbMediaDeviceUnlockWorker = new MediaDeviceUnlockWorker();
            //_extensionCounterWorker = new ExtensionCounterWorker();

            RefreshUsbDevices = new CallbackCommand(OnRefreshUsbDevices);
            CancelGatherer = new CallbackCommand(OnExtensionCounterWorkerCancel);

            _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            StartUnlockWorker();
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.UsbDeviceStore.DeviceInfoChanged -= UsbDeviceStore_DeviceInfoChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
        }

        private void UsbDeviceStore_DeviceInfoChanged(UsbDeviceStore usbDeviceStore)
        {
            System.Diagnostics.Debug.WriteLine($"Device change: MediaDeviceConnected: {MediaDeviceConnected}");
            System.Diagnostics.Debug.WriteLine($"Device change: MediaDeviceChosenName: {MediaDeviceChosenName}");
            //foreach (var ic in MediaDeviceChoices)
            //{
            //    System.Diagnostics.Debug.WriteLine($"Device change: ItemChoices: {ic}");
            //}

            if (_masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice == null)
            {
                OnExtensionCounterWorkerCancel(null);
                _isLocked = true;
                OnPropertyChanged(nameof(MediaDeviceUnlocked));
                OnPropertyChanged(nameof(MediaDeviceUnlockedColor));
            }

            StartUnlockWorker();
            StopUnlockWorker();

            OnPropertyChanged(nameof(MediaDeviceChoices));

            OnPropertyChanged(nameof(MediaDeviceConnected));
            OnPropertyChanged(nameof(MediaDeviceConnectedColor));
            OnPropertyChanged(nameof(MediaDevicePickerVisibility));
            OnPropertyChanged(nameof(MediaDeviceChosenName));
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
                OnPropertyChanged(nameof(MediaDeviceUnlocked));
                OnPropertyChanged(nameof(MediaDeviceUnlockedColor));
            }
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
                    //_masterStore.RunningStore.RunState,
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
