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
        private MediaTypeEnum _mediaType;

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

        public Visibility CancelVisibility => _masterStore.RunningStore.RunState == RunStates.DirectoryGathering ? Visibility.Visible : Visibility.Hidden;

        public ICommand OpenFolderBrowserDialog { get; }
        public ICommand CancelGatherer { get; }

        public DirectorySelectorViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _mediaType = _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType;

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
            if (_mediaType != _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType)
            {
                _mediaType = _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType;
                StartExtensionCountnerWorker();
            }
        }

        protected void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
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
            if (_masterStore.SorterConfigurationStore.SorterConfiguration.MediaType == MediaTypeEnum.NormalDirectory)
            {              
                _masterStore.RunningStore.WorkerHandler.CancelExtensionCounterWorker();
                _masterStore.RunningStore.ResetInfoFileCount();
                _masterStore.FileExtensionStore.Clear(); // Clear old extensions
                _masterStore.RunningStore.WorkerHandler.StartExtensionCounterWorker(new ExtensionCounterArguments(
                    MediaTypeEnum.NormalDirectory, 
                    _masterStore.SorterConfigurationStore.SorterConfiguration.SourcePath,
                    null,
                    DateTime.MinValue,
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
                    break;
                case WorkStatus.Interupted:
                    _masterStore.FileExtensionStore.Clear();
                    StartExtensionCountnerWorker(); // Rerun, as this is what should be done on interupts
                    break;
                default:
                    throw new NotImplementedException("Switch case in OnExtensionCounterWorkerDone does not handle all cases");
            }
        }

        protected void OnCancelGatherer(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnCancelGatherer");
            _masterStore.RunningStore.WorkerHandler.CancelExtensionCounterWorker();
        }
    }
}
