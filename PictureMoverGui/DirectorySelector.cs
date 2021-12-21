using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;

namespace PictureMoverGui
{
    class DirectorySelector
    {
        private PictureMoverModel moverModel;

        private BackgroundWorker worker;

        public DirectorySelector(PictureMoverModel moverModel)
        {
            this.moverModel = moverModel;
            this.moverModel.PropertyChanged += OnSourceDirChange;
            this.worker = null;

            this.StartDirGathering();
        }

        private void OnSourceDirChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "labelSourceDirContent")
            {
                this.StartDirGathering();
            }
        }

        public void ChooseSourceButtonClick()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
            {
                this.moverModel.labelSourceDirContent = openFileDlg.SelectedPath;
            }
        }

        public void ChooseSourceButtonCancelClick()
        {
            if (this.worker != null)
            {
                this.worker.CancelAsync();
            }
        }

        public void ChooseDestinationButtonClick()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
            {
                this.moverModel.labelDestinationDirContent = openFileDlg.SelectedPath;
            }
        }

        public void SwapSourceDestinationButtonClick()
        {
            if (this.moverModel.AllowSwapOperation)
            {
                string temp_value = this.moverModel.labelDestinationDirContent;
                this.moverModel.labelDestinationDirContent = this.moverModel.labelSourceDirContent;
                this.moverModel.labelSourceDirContent = temp_value;
            }
        }

        public void RefreshSourceDir(Action callback = null)
        {
            this.StartDirGathering(callback);
        }

        private void StartDirGathering(Action callback = null)
        {
            if (this.moverModel.sourceDirSat && this.moverModel.runningState == RunStates.Idle)
            {
                this.moverModel.runningState = RunStates.DirectoryGathering;

                this.moverModel.lastSourceInfoGatherTime = DateTime.Now;

                string search_dir = Properties.Settings.Default.SourceDir;

                worker = new BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += (obj, e) => worker_DirGathererDoWork(obj, e, search_dir);
                worker.RunWorkerCompleted += (obj, e) => worker_DirGathererWorkDone(obj, e, callback);
                worker.RunWorkerAsync();
            }
        }

        private void worker_DirGathererDoWork(object sender, DoWorkEventArgs e, string search_dir)
        {
            try
            {
                DirectoryInfoGatherer directoryInfoGatherer = new DirectoryInfoGatherer(search_dir, sender as BackgroundWorker);
                Dictionary<string, int> extensionInfo = directoryInfoGatherer.GatherInfo();
                e.Result = extensionInfo;
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"DirectorySelector crashed unexpectedly. Message: {err.Message}");
            }
        }

        private void worker_DirGathererWorkDone(object sender, RunWorkerCompletedEventArgs e, Action callback)
        {
            Dictionary<string, int> extensionInfo = e.Result as Dictionary<string, int>;

            if (e.Cancelled || extensionInfo == null)
            {
                this.moverModel.extensionInfoList = new ObservableCollection<ExtensionInfo>();
                this.moverModel.labelSourceDirContent = "";

                worker = null;
                this.moverModel.runningState = RunStates.Idle;
            }
            else
            {
                ObservableCollection<ExtensionInfo> newExtensionInfoList = new ObservableCollection<ExtensionInfo>();
                foreach (var item in extensionInfo)
                {
                    newExtensionInfoList.Add(new ExtensionInfo(item.Key, item.Value, ExtensionLookup.imageAndVideoExtensions.Contains(item.Key)));
                }
                this.moverModel.extensionInfoList = newExtensionInfoList;

                worker = null;
                this.moverModel.runningState = RunStates.Idle;

                if (callback != null)
                {
                    callback();
                }
            }
        }
    }
}
