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
    public class DirectorySelectorViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        private ExtensionCounterWorker _extensionCounterWorker;

        private SorterConfigurationModel SorterConfig => _masterStore.SorterConfigurationStore.SorterConfiguration;
   
        public string SourcePath
        {
            get { return SorterConfig.SourcePath; }
            set
            {
                if (value != SorterConfig.SourcePath)
                {
                    _masterStore.SorterConfigurationStore.SetSourcePath(value);
                    StartExtensionCountnerWorker();
                }
            }
        }

        public bool CanOpenDialog => _masterStore.RunningStore.RunState == RunStates.Idle;
 
        public Visibility CancelVisibility => _masterStore.RunningStore.RunState == RunStates.DirectoryGathering ? Visibility.Visible : Visibility.Hidden;

        public ICommand OpenFolderBrowserDialog { get; }
        public ICommand CancelGatherer { get; }

        public DirectorySelectorViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _extensionCounterWorker = new ExtensionCounterWorker();

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfiguration_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            OpenFolderBrowserDialog = new CallbackCommand(OnOpenFolderBrowserDialog);
            CancelGatherer = new CallbackCommand(OnCancelGatherer);

            StartExtensionCountnerWorker(); // Makes sure extension are loaded on startup
        }

        public override void Dispose()
        {
            base.Dispose();
            _masterStore.SorterConfigurationStore.SorterConfigurationChanged -= SorterConfiguration_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
        }

        protected void SorterConfiguration_SorterConfigurationChanged(SorterConfigurationModel sorterConfigurationModel)
        {
            OnPropertyChanged(nameof(SourcePath));
        }

        protected void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(CanOpenDialog));
            OnPropertyChanged(nameof(CancelVisibility));
        }

        protected void OnOpenFolderBrowserDialog(object parameter)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(openFileDlg.SelectedPath))
            {
                System.Diagnostics.Debug.WriteLine(openFileDlg.SelectedPath);
                SourcePath = openFileDlg.SelectedPath;
            }
        }

        protected void StartExtensionCountnerWorker()
        {
            if (CanOpenDialog)
            {
                _masterStore.FileExtensionStore.Clear(); // Clear old extensions
                _extensionCounterWorker.StartWorker(new ExtensionCounterArguments(
                    _masterStore.RunningStore.RunState,
                    MediaTypeEnum.NormalDirectory, 
                    _masterStore.SorterConfigurationStore.SorterConfiguration.SourcePath,
                    null,
                    //_masterStore.UsbDeviceStore.SelectedMediaDevice,
                    //_masterStore.UsbDeviceStore.ChosenMediaLastTime,
                    DateTime.MinValue,
                    _masterStore.RunningStore.SetRunState, 
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
                    _masterStore.SorterConfigurationStore.SetSourcePath("");
                    System.Diagnostics.Debug.WriteLine("Work status unfinished!");
                    break;
                case WorkStatus.Success:
                    _masterStore.FileExtensionStore.Set(extensionInfo);
                    break;
                case WorkStatus.Invalid:
                    _masterStore.FileExtensionStore.Clear();
                    _masterStore.SorterConfigurationStore.SetSourcePath("");
                    System.Diagnostics.Debug.WriteLine("The source was invald");
                    break;
                case WorkStatus.Cancelled:
                    _masterStore.FileExtensionStore.Clear();
                    _masterStore.SorterConfigurationStore.SetSourcePath("");
                    break;
                default:
                    throw new NotImplementedException("Switch case in OnExtensionCounterWorkerDone does not handle all cases");
            }
        }

        protected void OnCancelGatherer(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnCancelGatherer");
            _extensionCounterWorker.CancelWorker();
        }
    }
}
