using PictureMoverGui.Commands;
using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
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
                _extensionCounterWorker.StartWorker(Helpers.MediaTypeEnum.NormalDirectory, SourcePath, DateTime.MinValue, _masterStore.RunningStore.SetRunState, OnExtensionCounterWorkerDone);
            }
        }

        protected void OnExtensionCounterWorkerDone(Dictionary<string, int> extensionInfo)
        {
            System.Diagnostics.Debug.WriteLine("Worker done!");
            _masterStore.FileExtensionStore.Set(extensionInfo);
        }

        protected void OnCancelGatherer(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnCancelGatherer");
            _extensionCounterWorker.CancelWorker();
        }
    }
}
