using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.SubViewModels
{
    public class PhoneInterfaceViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        
        private MediaDeviceSelectorViewModel _mediaDeviceSelector;

        private SorterConfigurationModel SorterConfig => _masterStore.SorterConfigurationStore.SorterConfiguration;

        public bool DoStructured
        {
            get { return SorterConfig.DoStructured; }
            set
            {
                if (value != SorterConfig.DoStructured)
                {
                    _masterStore.SorterConfigurationStore.SetDoStructured(value);
                }
            }
        }

        public bool DoRename
        {
            get { return SorterConfig.DoRename; }
            set
            {
                if (value != SorterConfig.DoRename)
                {
                    _masterStore.SorterConfigurationStore.SetDoRename(value);
                }
            }
        }

        private bool _startActivated;
        private bool StartActivated
        {
            get => _startActivated;
            set
            {
                if (_startActivated != value)
                {
                    _startActivated = value;
                    OnPropertyChanged(nameof(StartActivated));
                    OnPropertyChanged(nameof(AllowStartSorting));
                }
            }
        }

        private bool _cancelActivated;
        private bool CancelActivated
        {
            get => _cancelActivated;
            set
            {
                if (_cancelActivated != value)
                {
                    _cancelActivated = value;
                    OnPropertyChanged(nameof(CancelActivated));
                    OnPropertyChanged(nameof(AllowCancel));
                }
            }
        }

        public bool AllowConfiguration => _masterStore.RunningStore.RunState == RunStates.Idle || _masterStore.RunningStore.RunState == RunStates.DirectoryGathering;
        public bool AllowStartSorting
        {
            get
            {
                return ((_masterStore.RunningStore.RunState == RunStates.Idle || _masterStore.RunningStore.RunState == RunStates.DirectoryGathering) &&
                    (!StartActivated) && (!_masterStore.RunningStore.IsMediaLocked) &&
                    (_masterStore.UsbDeviceStore.SelectedRemovableDevice.IsConnected));
            }
        }

        public Visibility CancelVisibility => (_masterStore.RunningStore.RunState == RunStates.RunningSorter || _masterStore.RunningStore.RunState == RunStates.DirectoryValidation) ? Visibility.Visible : Visibility.Hidden;
        public bool AllowCancel => !CancelActivated;

        public string StatusMessage => _masterStore.RunningStore.StatusMessage;
        public double StatusProgressDegrees => _masterStore.RunningStore.StatusPercentage * 3.6; // Percentage times 3.6 to get in degrees [0-360]

        public ICommand StartSorting { get; }
        public ICommand CancelSorting { get; }

        public PhoneInterfaceViewModel(MasterStore masterStore, MediaDeviceSelectorViewModel mediaDeviceSelector)
        {
            _masterStore = masterStore;
            _startActivated = false;
            _cancelActivated = false;

            _mediaDeviceSelector = mediaDeviceSelector;

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfigurationStore_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;
            _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;

            StartSorting = new CallbackCommand(OnStartSorting);
            CancelSorting = new CallbackCommand(OnCancelSorting);
        }


        public override void Dispose()
        {
            base.Dispose();

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged -= SorterConfigurationStore_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
            _masterStore.UsbDeviceStore.DeviceInfoChanged -= UsbDeviceStore_DeviceInfoChanged;
        }

        protected void SorterConfigurationStore_SorterConfigurationChanged(SorterConfigurationModel sorterConfigurationModel)
        {
            OnPropertyChanged(nameof(DoStructured));
            OnPropertyChanged(nameof(DoRename));
        }

        protected void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(AllowConfiguration));
            OnPropertyChanged(nameof(AllowStartSorting));
            OnPropertyChanged(nameof(CancelVisibility));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(StatusProgressDegrees));
        }

        protected void UsbDeviceStore_DeviceInfoChanged(UsbDeviceStore usbDeviceStore)
        {
            OnPropertyChanged(nameof(AllowStartSorting));
        }

        protected void OnStartSorting(object parameter)
        {
            if (_masterStore.RunningStore.RunState == RunStates.Idle || _masterStore.RunningStore.RunState == RunStates.DirectoryGathering)
            {
                StartActivated = true;
                _masterStore.RunningStore.WorkerHandler.InteruptExtensionCounterWorker();
                _masterStore.RunningStore.ResetInfoFileCount();
                _masterStore.RunningStore.WorkerHandler.StartPictureMoverWorker(new PictureMoverArguments(
                    new List<string> { _masterStore.SorterConfigurationStore.SorterConfiguration.DestinationPath, _masterStore.UsbDeviceStore.SelectedRemovableDevice.Name + _masterStore.UsbDeviceStore.SelectedRemovableDevice.Path },
                    true,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.DoStructured,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.DoRename,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.NameCollisionAction,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.CompareFilesAction,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.HashType,
                    _masterStore.FileExtensionStore.GetListOfValidExtension(),
                    new List<EventDataModel>(_masterStore.EventDataStore.EventDataValues),
                    MediaTypeEnum.MediaDevice,
                    null,
                    _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice,
                    _masterStore.UsbDeviceStore.SelectedMediaDevice.LastRun,
                    _masterStore.RunningStore.SetStatusPercentage,
                    _masterStore.RunningStore.AddStatusLog,
                    _masterStore.RunningStore.IncrementInfoFileCount,
                    OnPictureMoverWorkerDone
                ));
            }
        }

        protected void OnPictureMoverWorkerDone(WorkStatus workStatus, int nrOfErrors)
        {
            switch (workStatus)
            {
                case WorkStatus.Unfinished:
                    break;
                case WorkStatus.Success:
                    _masterStore.UsbDeviceStore.SetSelectedMediaDeviceDateTime(DateTime.Now);
                    break;
                case WorkStatus.Invalid:
                    MessageBox.Show("The media device not loaded. Please reload and try again", "Media device not loaded");
                    break;
                case WorkStatus.Cancelled:
                    break;
                case WorkStatus.Interupted:
                    break;
                default:
                    throw new NotImplementedException("Switch case in OnExtensionCounterWorkerDone does not handle all cases");
            }
            StartActivated = false;
            CancelActivated = false;
        }

        protected void OnCancelSorting(object parameter)
        {
            CancelActivated = true;
            _masterStore.RunningStore.WorkerHandler.CancelPictureMoverWorker();
        }
    }
}
