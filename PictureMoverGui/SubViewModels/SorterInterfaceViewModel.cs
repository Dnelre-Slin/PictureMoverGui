using PictureMoverGui.Commands;
using PictureMoverGui.DirectoryWorkers;
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
    public class SorterInterfaceViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private SorterConfigurationModel SorterConfig => _masterStore.SorterConfigurationStore.SorterConfiguration;

        public bool DoCopy
        {
            get { return SorterConfig.DoCopy; }
            set
            {
                if (value != SorterConfig.DoCopy)
                {
                    _masterStore.SorterConfigurationStore.SetDoCopy(value);
                }
            }
        }

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

        private Visibility _copyVisibility;
        public Visibility CopyVisibility
        {
            get => _copyVisibility;
            set
            {
                if (_copyVisibility != value)
                {
                    _copyVisibility = value;
                    OnPropertyChanged(nameof(CopyVisibility));
                }
            }
        }

        public bool AllowConfiguration => _masterStore.RunningStore.RunState != RunStates.RunningSorter;
        public bool AllowStartSorting => _masterStore.RunningStore.RunState == RunStates.Idle || _masterStore.RunningStore.RunState == RunStates.DirectoryGathering;

        public Visibility CancelVisibility => (_masterStore.RunningStore.RunState == RunStates.RunningSorter || _masterStore.RunningStore.RunState == RunStates.DirectoryValidation) ? Visibility.Visible : Visibility.Hidden;

        public string StatusMessage => _masterStore.RunningStore.StatusMessage;
        public double StatusProgressDegrees => _masterStore.RunningStore.StatusPercentage * 3.6; // Percentage times 3.6 to get in degrees [0-360]

        public ICommand StartSorting { get; }
        public ICommand CancelSorting { get; }

        public SorterInterfaceViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfigurationStore_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            StartSorting = new CallbackCommand(OnStartSorting);
            CancelSorting = new CallbackCommand(OnCancelSorting);

            _copyVisibility = Visibility.Visible;
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged -= SorterConfigurationStore_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
        }

        protected void SorterConfigurationStore_SorterConfigurationChanged(SorterConfigurationModel sorterConfigurationModel)
        {
            OnPropertyChanged(nameof(DoCopy));
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

        protected void OnStartSorting(object parameter)
        {
            if (AllowStartSorting)
            {
                _masterStore.RunningStore.WorkerHandler.InteruptExtensionCounterWorker();
                System.Diagnostics.Debug.WriteLine("OnStartSorting");
                if (_masterStore.SorterConfigurationStore.SorterConfiguration.MediaType == MediaTypeEnum.NormalDirectory)
                {
                    _masterStore.RunningStore.ResetInfoFileCount();
                    _masterStore.RunningStore.WorkerHandler.StartPictureMoverWorker(new PictureMoverArguments(
                        new List<string> { _masterStore.SorterConfigurationStore.SorterConfiguration.DestinationPath },
                        _masterStore.SorterConfigurationStore.SorterConfiguration.DoCopy,
                        _masterStore.SorterConfigurationStore.SorterConfiguration.DoStructured,
                        _masterStore.SorterConfigurationStore.SorterConfiguration.DoRename,
                        _masterStore.SorterConfigurationStore.SorterConfiguration.NameCollisionAction,
                        _masterStore.SorterConfigurationStore.SorterConfiguration.CompareFilesAction,
                        _masterStore.SorterConfigurationStore.SorterConfiguration.HashType,
                        _masterStore.FileExtensionStore.GetListOfValidExtension(),
                        new List<EventDataModel>(_masterStore.EventDataStore.EventDataValues),
                        MediaTypeEnum.NormalDirectory,
                        _masterStore.SorterConfigurationStore.SorterConfiguration.SourcePath,
                        null,
                        DateTime.MinValue,
                        _masterStore.RunningStore.SetStatusPercentage,
                        _masterStore.RunningStore.AddStatusLog,
                        _masterStore.RunningStore.IncrementInfoFileCount,
                        OnPictureMoverWorkerDone
                    ));                    
                }
                else if (_masterStore.SorterConfigurationStore.SorterConfiguration.MediaType == MediaTypeEnum.MediaDevice)
                {
                    _masterStore.RunningStore.ResetInfoFileCount();
                    _masterStore.RunningStore.WorkerHandler.StartPictureMoverWorker(new PictureMoverArguments(
                        new List<string> { _masterStore.SorterConfigurationStore.SorterConfiguration.DestinationPath },
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
                else
                {
                    throw new NotImplementedException($"SorterInterfaceViewModel has not implemented supported for this media type: {_masterStore.SorterConfigurationStore.SorterConfiguration.MediaType}");
                }
            }
        }

        protected void OnPictureMoverWorkerDone(WorkStatus workStatus, int nrOfErrors)
        {
            System.Diagnostics.Debug.WriteLine("Worker done!");
            System.Diagnostics.Debug.WriteLine(workStatus);
            System.Diagnostics.Debug.WriteLine($"Number of errors: {nrOfErrors}");
            switch (workStatus)
            {
                case WorkStatus.Unfinished:
                    System.Diagnostics.Debug.WriteLine("Work status unfinished!");
                    break;
                case WorkStatus.Success:
                    if (_masterStore.SorterConfigurationStore.SorterConfiguration.MediaType == MediaTypeEnum.MediaDevice)
                    {
                        _masterStore.UsbDeviceStore.SetSelectedMediaDeviceDateTime(DateTime.Now);
                    }
                    break;
                case WorkStatus.Invalid:
                    _masterStore.RunningStore.AddStatusLog("Source dir no longer exists");
                    _masterStore.SorterConfigurationStore.SetSourcePath("");
                    MessageBox.Show("The source dir no longer exists. Please start select source again", "Source dir no longer exists");
                    break;
                case WorkStatus.Cancelled:
                    break;
                case WorkStatus.Interupted:
                    break;
                default:
                    throw new NotImplementedException("Switch case in OnExtensionCounterWorkerDone does not handle all cases");
            }
        }

        protected void OnCancelSorting(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnCancelSorting");
            //_pictureMoverWorker.CancelWorker();
            _masterStore.RunningStore.WorkerHandler.CancelPictureMoverWorker();
        }
    }
}
