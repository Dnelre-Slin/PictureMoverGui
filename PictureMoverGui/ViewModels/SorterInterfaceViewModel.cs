using PictureMoverGui.Commands;
using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class SorterInterfaceViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        private PictureMoverWorker _pictureMoverWorker;

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

        public bool AllowConfiguration => _masterStore.RunningStore.RunState != RunStates.RunningSorter;
        public bool AllowStartSorting => _masterStore.RunningStore.RunState == RunStates.Idle;

        public Visibility CancelVisibility => _masterStore.RunningStore.RunState == RunStates.RunningSorter ? Visibility.Visible : Visibility.Hidden;

        public string StatusMessage => _masterStore.RunningStore.StatusMessage;
        public double StatusProgressDegrees => _masterStore.RunningStore.StatusPercentage * 3.6; // Time 3.6 to get in degrees [0-360]

        public ICommand StartSorting { get; }
        public ICommand CancelSorting { get; }

        public SorterInterfaceViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _pictureMoverWorker = new PictureMoverWorker();

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfigurationStore_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            StartSorting = new CallbackCommand(OnStartSorting);
            CancelSorting = new CallbackCommand(OnCancelSorting);
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
                System.Diagnostics.Debug.WriteLine("OnStartSorting");
                _pictureMoverWorker.StartWorker(new PictureMoverArguments(
                    _masterStore.RunningStore.RunState,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.DestinationPath,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.DoCopy,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.DoStructured,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.DoRename,
                    NameCollisionActionEnum.CompareFiles,
                    CompareFilesActionEnum.NameAndDateOnly,
                    HashTypeEnum.MD5,
                    _masterStore.FileExtensionStore.GetListOfValidExtension(),
                    new List<SimpleEventData>(),
                    MediaTypeEnum.NormalDirectory,
                    _masterStore.SorterConfigurationStore.SorterConfiguration.SourcePath,
                    DateTime.MinValue,
                    _masterStore.RunningStore.SetRunState,
                    _masterStore.RunningStore.SetStatusPercentage,
                    null,
                    OnPictureMoverWorkerDone
                ));
                //_masterStore.RunningStore.SetRunState(RunStates.RunningSorter);
            }
        }

        protected void OnPictureMoverWorkerDone(WorkStatus workStatus, int nrOfErrors)
        {
            System.Diagnostics.Debug.WriteLine("Worker done!");
            System.Diagnostics.Debug.WriteLine(workStatus);
            System.Diagnostics.Debug.WriteLine($"Number of errors: {nrOfErrors}");
        }

        protected void OnCancelSorting(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnCancelSorting");
            _pictureMoverWorker.CancelWorker();
            //_masterStore.RunningStore.SetRunState(RunStates.Idle);
        }
    }
}
