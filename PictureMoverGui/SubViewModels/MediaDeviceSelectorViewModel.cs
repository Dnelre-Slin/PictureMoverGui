using PictureMoverGui.Commands;
using PictureMoverGui.DeviceWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PictureMoverGui.SubViewModels
{
    public class MediaDeviceSelectorViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        private MediaDeviceUnlockWorker _usbMediaDeviceUnlockWorker;
        private MediaTypeEnum _mediaType;

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
        public string LastRunDate
        {
            get => _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Date.ToString("dd.MM.yyyy");
            set
            {
                if (_masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Date.ToString("dd.MM.yyyy") != value)
                {
                    string[] dateValues = value.Split('.');
                    int hour = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Hour;
                    int minute = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Minute;
                    int second = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Second;
                    _masterStore.UsbDeviceStore.SetSelectedMediaDeviceDateTime(new DateTime(
                        int.Parse(dateValues[2]),
                        int.Parse(dateValues[1]),
                        int.Parse(dateValues[0]),
                        hour,
                        minute,
                        second));
                    //OnPropertyChanged(nameof(LastRunDateTime));
                    //OnPropertyChanged(nameof(LastRunDate));
                }
            }
        }

        public string LastRunHour
        {
            get => _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Hour.ToString("00");
            set
            {
                if (_masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Hour.ToString("00") != value)
                {
                    int year = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Year;
                    int month = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Month;
                    int day = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Day;
                    int minute = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Minute;
                    int second = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Second;
                    _masterStore.UsbDeviceStore.SetSelectedMediaDeviceDateTime(new DateTime(
                        year,
                        month,
                        day,
                        int.Parse(value),
                        minute,
                        second));
                    //OnPropertyChanged(nameof(LastRunDateTime));
                    //OnPropertyChanged(nameof(LastRunHour));
                }
            }
        }

        public string LastRunMinute
        {
            get => _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Minute.ToString("00");
            set
            {
                if (_masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Minute.ToString("00") != value)
                {
                    int year = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Year;
                    int month = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Month;
                    int day = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Day;
                    int hour = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Hour;
                    int second = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Second;
                    _masterStore.UsbDeviceStore.SetSelectedMediaDeviceDateTime(new DateTime(
                        year,
                        month,
                        day,
                        hour,
                        int.Parse(value),
                        second));
                    //OnPropertyChanged(nameof(LastRunDateTime));
                    //OnPropertyChanged(nameof(LastRunMinute));
                }
            }
        }

        public string LastRunSecond
        {
            get => _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Second.ToString("00");
            set
            {
                if (_masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Second.ToString("00") != value)
                {
                    int year = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Year;
                    int month = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Month;
                    int day = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Day;
                    int hour = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Hour;
                    int minute = _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun.Minute;
                    _masterStore.UsbDeviceStore.SetSelectedMediaDeviceDateTime(new DateTime(
                        year,
                        month,
                        day,
                        hour,
                        minute,
                        int.Parse(value)));
                    //OnPropertyChanged(nameof(LastRunDateTime));
                    //OnPropertyChanged(nameof(LastRunSecond));
                }
            }
        }

        public IEnumerable<string> ValidHours => Enumerable.Range(0, 24).Select(i => i.ToString("00"));
        public IEnumerable<string> ValidMinutesAndSeconds => Enumerable.Range(0, 60).Select(i => i.ToString("00"));

        public int InfoFileCount => _masterStore.RunningStore.InfoFileCount;

        public bool Editing { get; private set; }
        public Visibility EditPanelVisibility => Editing ? Visibility.Visible : Visibility.Hidden;
        public Brush EditColor => Editing ? Brushes.LightBlue : Brushes.LightGray;

        public Visibility WorkerRunningVisibility => _masterStore.RunningStore.RunState == RunStates.DirectoryGathering ? Visibility.Visible : Visibility.Hidden;

        public ICommand RefreshUsbDevices { get; }
        public ICommand CancelGatherer { get; }
        public ICommand Edit { get; }

        public MediaDeviceSelectorViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _usbMediaDeviceUnlockWorker = new MediaDeviceUnlockWorker();
            //_extensionCounterWorker = new ExtensionCounterWorker();
            _mediaType = _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType;

            RefreshUsbDevices = new CallbackCommand(OnRefreshUsbDevices);
            CancelGatherer = new CallbackCommand(OnExtensionCounterWorkerCancel);
            Edit = new CallbackCommand(OnEdit);

            _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;
            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfigurationStore_SorterConfigurationChanged;

            StartUnlockWorker();
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.UsbDeviceStore.DeviceInfoChanged -= UsbDeviceStore_DeviceInfoChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
            _masterStore.SorterConfigurationStore.SorterConfigurationChanged -= SorterConfigurationStore_SorterConfigurationChanged;

            _usbMediaDeviceUnlockWorker.CancelWorker();
        }

        private void UsbDeviceStore_DeviceInfoChanged(UsbDeviceStore usbDeviceStore)
        {
            System.Diagnostics.Debug.WriteLine($"Device change: MediaDeviceConnected: {MediaDeviceConnected}");
            System.Diagnostics.Debug.WriteLine($"Device change: MediaDeviceChosenName: {MediaDeviceChosenName}");

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

            OnPropertyChanged(nameof(LastRunDateTime));
            OnPropertyChanged(nameof(LastRunDate));
            OnPropertyChanged(nameof(LastRunHour));
            OnPropertyChanged(nameof(LastRunMinute));
            OnPropertyChanged(nameof(LastRunSecond));
        }

        private void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(InfoFileCount));
            OnPropertyChanged(nameof(WorkerRunningVisibility));
        }

        private void SorterConfigurationStore_SorterConfigurationChanged(Models.SorterConfigurationModel sorterConfig)
        {
            if (_mediaType != _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType)
            {
                _mediaType = _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType;
                StartExtensionCountnerWorker();
            }
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
            if (_masterStore.SorterConfigurationStore.SorterConfiguration.MediaType == MediaTypeEnum.MediaDevice)
            {
                _masterStore.FileExtensionStore.Clear(); // Clear old extensions
                if (!_isLocked)
                {
                    _masterStore.RunningStore.WorkerHandler.CancelExtensionCounterWorker();
                    _masterStore.RunningStore.ResetInfoFileCount();
                    _masterStore.RunningStore.WorkerHandler.StartExtensionCounterWorker(new ExtensionCounterArguments(
                        MediaTypeEnum.MediaDevice,
                        null,
                        _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice,
                        _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun,
                        _masterStore.RunningStore.IncrementInfoFileCount,
                        OnExtensionCounterWorkerDone
                    ));
                }
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

        protected void OnEdit(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnEdit");
            Editing = !Editing;
            OnPropertyChanged(nameof(Editing));
            OnPropertyChanged(nameof(EditPanelVisibility));
            OnPropertyChanged(nameof(EditColor));
        }
    }
}
