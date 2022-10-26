using PictureMoverGui.Commands;
using PictureMoverGui.DirectoryWorkers;
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
        //private SorterConfigurationModel _sorterConfigurationModel;
        private SorterConfigurationModel SorterConfig => _masterStore.SorterConfigurationStore.SorterConfiguration;
        //private string SourceStr => _masterStore.SorterConfigurationStore.SorterConfiguration.SourcePath;

        //public string FolderPath
        //{
        //    get { return _sorterConfigurationModel.SourcePath; }
        //    set
        //    {
        //        if (value != _sorterConfigurationModel.SourcePath)
        //        {
        //            _masterStore.SorterConfiguration.SetSourcePath(value);
        //            OnPropertyChanged(nameof(FolderPath));
        //        }
        //    }
        //}
        //public string FolderPath
        //{
        //    get { return SortMod.SourcePath; }
        //    set
        //    {
        //        if (value != SortMod.SourcePath)
        //        {
        //            _masterStore.SorterConfiguration.SetSourcePath(value);
        //            OnPropertyChanged(nameof(FolderPath));
        //        }
        //    }
        //}        

        public string SourcePath
        {
            get { return SorterConfig.SourcePath; }
            set
            {
                if (value != SorterConfig.SourcePath)
                {
                    _masterStore.SorterConfigurationStore.SetSourcePath(value);
                    //OnPropertyChanged(nameof(FolderPath));
                }
            }
        }


        private bool _isNotRunningGatherer;
        public bool IsNotRunningGatherer
        {
            get { return _isNotRunningGatherer; }
            set
            {
                if (_isNotRunningGatherer != value)
                {
                    _isNotRunningGatherer = value;
                    OnPropertyChanged(nameof(IsNotRunningGatherer));
                    OnPropertyChanged(nameof(CancelVisibility));
                }
            }
        }

        public Visibility CancelVisibility => IsNotRunningGatherer ? Visibility.Hidden : Visibility.Visible;

        public ICommand OpenFolderBrowserDialog { get; }
        public ICommand CancelGatherer { get; }

        public DirectorySelectorViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            //_sorterConfigurationModel = _masterStore.SorterConfiguration.SorterConfiguration;

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfiguration_SorterConfigurationChanged;

            _isNotRunningGatherer = true;

            OpenFolderBrowserDialog = new CallbackCommand(OnOpenFolderBrowserDialog);
            CancelGatherer = new CallbackCommand(OnCancelGatherer);
        }

        public override void Dispose()
        {
            base.Dispose();
            _masterStore.SorterConfigurationStore.SorterConfigurationChanged -= SorterConfiguration_SorterConfigurationChanged;
        }

        protected void SorterConfiguration_SorterConfigurationChanged(SorterConfigurationModel sorterConfigurationModel)
        {
            //_sorterConfigurationModel = sorterConfigurationModel;
            OnPropertyChanged(nameof(SourcePath));
        }

        protected void OnOpenFolderBrowserDialog(object parameter)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(openFileDlg.SelectedPath))
            {
                System.Diagnostics.Debug.WriteLine(openFileDlg.SelectedPath);
                SourcePath = openFileDlg.SelectedPath;
                ExtensionCounterWorker worker = new ExtensionCounterWorker(Helpers.MediaTypeEnum.NormalDirectory, SourcePath, DateTime.MinValue, null, OnExtensionCounterWorkerDone);
                //OnPropertyChanged(nameof(SourcePath));
                //IsNotRunningGatherer = !IsNotRunningGatherer;
                //this.moverModel.labelSourceDirContent = openFileDlg.SelectedPath;
            }
        }

        protected void OnExtensionCounterWorkerDone(Dictionary<string, int> extensionInfo)
        {
            System.Diagnostics.Debug.WriteLine("Worker done!");
            foreach (var extension in extensionInfo)
            {
                System.Diagnostics.Debug.WriteLine($"{extension.Key} : {extension.Value}");
            }
        }

        protected void OnCancelGatherer(object parameter)
        {
            //List<string> ls = new List<string>(Properties.Persistant.Default.FancyListy);
            string[] list = Properties.Persistant.Default.FancyListy.Cast<string>().ToArray();
            System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
            sc.AddRange(list);
            sc.Add(SourcePath);
            //foreach (var s in Properties.Persistant.Default.FancyListy)
            foreach (var s in sc)
            {
                System.Diagnostics.Debug.WriteLine(s);
            }
            IsNotRunningGatherer = !IsNotRunningGatherer;
        }
    }
}
