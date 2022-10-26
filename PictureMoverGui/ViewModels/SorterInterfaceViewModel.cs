using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
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
            System.Diagnostics.Debug.WriteLine("OnStartSorting");
            _masterStore.RunningStore.SetRunState(RunStates.RunningSorter);
        }

        protected void OnCancelSorting(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnCancelSorting");
            _masterStore.RunningStore.SetRunState(RunStates.Idle);
        }
    }
}
