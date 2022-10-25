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

        static private Dictionary<string, int> GetExtensions(string search_dir, BackgroundWorker sender_worker)
        {
            DirectoryInfo d = new DirectoryInfo(search_dir);
            Dictionary<string, int> extensionMap = new Dictionary<string, int>();

            foreach (FileInfo file in d.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                if (string.IsNullOrEmpty(file.Extension))
                {
                    continue; // Do not add extension, if file has no extension.
                }

                string ext = file.Extension.ToLower(); // To lower case. Example .JPEG -> .jpeg
                ext = ext.Substring(1); // Remove leading '.'. Example: .jpeg -> jpeg
                if (extensionMap.ContainsKey(ext))
                {
                    extensionMap[ext] += 1;
                }
                else
                {
                    extensionMap[ext] = 1;
                }

                if (sender_worker.CancellationPending)
                {
                    break;
                }
            }

            return extensionMap;
        }

        private void worker_DirGathererDoWork(object sender, DoWorkEventArgs e, string search_dir)
        {
            //System.Threading.Thread.Sleep(4000);
            //e.Result = new Dictionary<string, int>();

            try
            {
                //DirectoryInfoGatherer directoryInfoGatherer = new DirectoryInfoGatherer(search_dir, sender as BackgroundWorker);
                //Dictionary<string, int> extensionInfo = directoryInfoGatherer.GatherInfo();
                Dictionary<string, int> extensionInfo = GetExtensions(search_dir, sender as BackgroundWorker);
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
                //this.moverModel.extensionMapInCurrentDir = new Dictionary<string, int>();
                //this.moverModel.nrOfFilesInCurrentDir = 0;
                this.moverModel.extensionInfoList = new ObservableCollection<ExtensionInfo>();
                this.moverModel.labelSourceDirContent = "";

                worker = null;
                this.moverModel.runningState = RunStates.Idle;
            }
            else
            {
                ObservableCollection<ExtensionInfo> newExtensionInfoList = new ObservableCollection<ExtensionInfo>();
                //int nrOfFilesInCurrentDir = 0;
                foreach (var item in extensionInfo)
                {
                    newExtensionInfoList.Add(new ExtensionInfo(item.Key, item.Value, ExtensionLookup.imageAndVideoExtensions.Contains(item.Key)));
                    //nrOfFilesInCurrentDir += item.Value;
                }
                this.moverModel.extensionInfoList = newExtensionInfoList;
                //this.moverModel.extensionMapInCurrentDir = extensionInfo;
                //this.moverModel.nrOfFilesInCurrentDir = nrOfFilesInCurrentDir;

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
