using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace PictureMoverGui
{
    class DirectorySelector
    {
        private PictureMoverModel moverModel;
        //private Label labelSourceDir;
        //private Label labelDestinationDir;

        private BackgroundWorker worker;

        public DirectorySelector(PictureMoverModel moverModel)
        {
            this.moverModel = moverModel;
            this.moverModel.PropertyChanged += OnSourceDirChange;
            //this.labelSourceDir = labelSourceDir;
            //this.labelDestinationDir = labelDestinationDir;
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

        //public void StartUp()
        //{
        //    string start_source_dir = Properties.Settings.Default.UnsortedDir;
        //    string start_destination_dir = Properties.Settings.Default.SortedDir;
        //    if (!string.IsNullOrEmpty(start_source_dir) && new DirectoryInfo(start_source_dir).Exists)
        //    {
        //        SetSourceDir(start_source_dir);
        //    }
        //    if (!string.IsNullOrEmpty(start_destination_dir) && new DirectoryInfo(start_destination_dir).Exists)
        //    {
        //        SetDestinationDir(start_destination_dir);
        //    }
        //}

        public void ChooseSourceButtonClick()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
            {
                this.moverModel.labelSourceDirContent = openFileDlg.SelectedPath;
                //this.StartDirGathering();
                //this.SetSourceDir(openFileDlg.SelectedPath);
            }
        }

        public void ChooseSourceButtonCancelClick()
        {
            if (this.worker != null)
            {
                this.worker.CancelAsync();
                //this.moverModel.labelSourceDirContent = "";
                //this.UnsetSourceDir();
            }
        }

        public void ChooseDestinationButtonClick()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
            {
                this.moverModel.labelDestinationDirContent = openFileDlg.SelectedPath;
                //this.SetDestinationDir(openFileDlg.SelectedPath);
            }
        }

        public void SwapSourceDestinationButtonClick()
        {
            if (this.moverModel.AllowSwapOperation)
            {
                string temp_value = this.moverModel.labelDestinationDirContent;
                this.moverModel.labelDestinationDirContent = this.moverModel.labelSourceDirContent;
                this.moverModel.labelSourceDirContent = temp_value;
                //this.StartDirGathering();
                //string new_source_dir = this.labelDestinationDir.Content.ToString();
                //string new_destination_dir = this.labelSourceDir.Content.ToString();
                //SetSourceDir(new_source_dir);
                //SetDestinationDir(new_destination_dir);
            }
        }

        public void RefreshSourceDir(Action callback = null)
        {
            this.StartDirGathering(callback);
            //DirectoryInfo d = new DirectoryInfo(this.moverModel.labelSourceDirContent);
            //if (!d.Exists)
            //{
            //    System.Diagnostics.Trace.TraceWarning("Source dir no longer existed");
            //    this.moverModel.labelSourceDirContent = "";
            //    return true;
            //}
            //bool sourceDirChanged = DirSearcher.DirLastWriteCompare(d, this.moverModel.lastSourceInfoGatherTime);
            //if (sourceDirChanged)
            //{
            //    this.StartDirGathering();
            //    MessageBox.Show("The source dir has been changed. Please start again, so that the latest changes will be included", "Source dir change");
            //    return true;
            //}
            //return false;
        }

        //private void SetSourceDir(string path_to_dir)
        //{
        //    this.labelSourceDir.Content = path_to_dir;
        //    Properties.Settings.Default.UnsortedDir = path_to_dir;
        //    Properties.Settings.Default.Save();
        //    this.moverModel.sourceDirSat = true;
        //    this.StartDirGathering();
        //}

        //private void UnsetSourceDir()
        //{
        //    this.labelSourceDir.Content = App.Current.FindResource("DefaultEmptyPath");
        //    Properties.Settings.Default.UnsortedDir = "";
        //    Properties.Settings.Default.Save();
        //    this.moverModel.sourceDirSat = false;
        //}

        //private void SetDestinationDir(string path_to_dir)
        //{
        //    this.labelDestinationDir.Content = path_to_dir;
        //    Properties.Settings.Default.SortedDir = path_to_dir;
        //    Properties.Settings.Default.Save();
        //    this.moverModel.destinationDirSat = true;
        //}

        //private void UnsetDestinationDir()
        //{
        //    this.labelDestinationDir.Content = App.Current.FindResource("DefaultEmptyPath");
        //    Properties.Settings.Default.SortedDir = "";
        //    Properties.Settings.Default.Save();
        //    this.moverModel.destinationDirSat = false;
        //}

        private void StartDirGathering(Action callback = null)
        {
            if (this.moverModel.sourceDirSat && this.moverModel.runningState == PictureMoverModel.RunStates.Idle)
            {
                this.moverModel.runningState = PictureMoverModel.RunStates.DirectoryGathering;

                this.moverModel.lastSourceInfoGatherTime = DateTime.Now;

                string search_dir = Properties.Settings.Default.UnsortedDir;

                worker = new BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                //worker.WorkerReportsProgress = true;
                worker.DoWork += (obj, e) => worker_DirGathererDoWork(obj, e, search_dir);
                //worker.ProgressChanged += worker_DirGathererProgressChanged;
                worker.RunWorkerCompleted += (obj, e) => worker_DirGathererWorkDone(obj, e, callback);
                worker.RunWorkerAsync();
            }
        }

        private void worker_DirGathererDoWork(object sender, DoWorkEventArgs e, string search_dir)
        {
            //System.Threading.Thread.Sleep(4000);
            //e.Result = new Dictionary<string, int>();

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
                this.moverModel.extensionMapInCurrentDir = new Dictionary<string, int>();
                this.moverModel.nrOfFilesInCurrentDir = 0;
                this.moverModel.labelSourceDirContent = "";

                worker = null;
                this.moverModel.runningState = PictureMoverModel.RunStates.Idle;
            }
            else
            {
                int nrOfFilesInCurrentDir = 0;
                foreach (var item in extensionInfo)
                {
                    nrOfFilesInCurrentDir += item.Value;
                }
                this.moverModel.extensionMapInCurrentDir = extensionInfo;
                this.moverModel.nrOfFilesInCurrentDir = nrOfFilesInCurrentDir;

                worker = null;
                this.moverModel.runningState = PictureMoverModel.RunStates.Idle;

                if (callback != null)
                {
                    callback();
                }
            }
        }
    }
}
