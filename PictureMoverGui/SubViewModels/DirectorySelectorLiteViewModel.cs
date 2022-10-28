using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.SubViewModels
{
    public class DirectorySelectorLiteViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        private SorterConfigurationModel SorterConfig => _masterStore.SorterConfigurationStore.SorterConfiguration;

        public string DestinationPath
        {
            get { return SorterConfig.DestinationPath; }
            set
            {
                if (value != SorterConfig.DestinationPath)
                {
                    _masterStore.SorterConfigurationStore.SetDestinationPath(value);
                }
            }
        }

        public bool CanOpenDialog => _masterStore.RunningStore.RunState != RunStates.RunningSorter;

        public ICommand OpenFolderBrowserDialog { get; }

        public DirectorySelectorLiteViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfiguration_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            OpenFolderBrowserDialog = new CallbackCommand(OnOpenFolderBrowserDialog);
        }

        public override void Dispose()
        {
            base.Dispose();
            _masterStore.SorterConfigurationStore.SorterConfigurationChanged -= SorterConfiguration_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
        }

        protected void SorterConfiguration_SorterConfigurationChanged(SorterConfigurationModel sorterConfigurationModel)
        {
            OnPropertyChanged(nameof(DestinationPath));
        }

        protected void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(CanOpenDialog));
        }

        protected void OnOpenFolderBrowserDialog(object parameter)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(openFileDlg.SelectedPath))
            {
                System.Diagnostics.Debug.WriteLine(openFileDlg.SelectedPath);
                DestinationPath = openFileDlg.SelectedPath;
            }
        }
    }
}
