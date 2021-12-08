using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace PictureMoverGui
{
    class DirectorySelector
    {
        private PictureMoverModel moverModel;
        private Label labelSourceDir;
        private Label labelDestinationDir;

        public DirectorySelector(PictureMoverModel moverModel, Label labelSourceDir, Label labelDestinationDir)
        {
            this.moverModel = moverModel;
            this.labelSourceDir = labelSourceDir;
            this.labelDestinationDir = labelDestinationDir;
        }

        public void StartUp()
        {
            string start_source_dir = Properties.Settings.Default.UnsortedDir;
            string start_destination_dir = Properties.Settings.Default.SortedDir;
            if (!string.IsNullOrEmpty(start_source_dir) && new DirectoryInfo(start_source_dir).Exists)
            {
                SetSourceDir(start_source_dir);
            }
            if (!string.IsNullOrEmpty(start_destination_dir) && new DirectoryInfo(start_destination_dir).Exists)
            {
                SetDestinationDir(start_destination_dir);
            }
        }

        public void ChooseSourceButtonClick()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
            {
                this.SetSourceDir(openFileDlg.SelectedPath);
            }
        }

        public void ChooseDestinationButtonClick()
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
            {
                this.SetDestinationDir(openFileDlg.SelectedPath);
            }
        }

        public void SwapSourceDestinationButtonClick()
        {
            if (this.moverModel.AllowSwapOperation)
            {
                string new_source_dir = this.labelDestinationDir.Content.ToString();
                string new_destination_dir = this.labelSourceDir.Content.ToString();
                SetSourceDir(new_source_dir);
                SetDestinationDir(new_destination_dir);
            }
        }

        private void SetSourceDir(string path_to_dir)
        {
            this.labelSourceDir.Content = path_to_dir;
            Properties.Settings.Default.UnsortedDir = path_to_dir;
            Properties.Settings.Default.Save();
            this.moverModel.sourceDirSat = true;
            this.StartDirGathering();
        }

        private void SetDestinationDir(string path_to_dir)
        {
            this.labelDestinationDir.Content = path_to_dir;
            Properties.Settings.Default.SortedDir = path_to_dir;
            Properties.Settings.Default.Save();
            this.moverModel.destinationDirSat = true;
        }

        private void StartDirGathering()
        {
            if (this.moverModel.GatherInfoDirNotRunning)
            {
                this.moverModel.gatherDirInfoRunning = true;

                string search_dir = Properties.Settings.Default.UnsortedDir;

                BackgroundWorker worker = new BackgroundWorker();
                //worker.WorkerReportsProgress = true;
                worker.DoWork += (obj, e) => worker_DirGathererDoWork(obj, e, search_dir);
                //worker.ProgressChanged += worker_DirGathererProgressChanged;
                worker.RunWorkerCompleted += worker_DirGathererWorkDone;
                worker.RunWorkerAsync();
            }
        }

        private void worker_DirGathererDoWork(object sender, DoWorkEventArgs e, string search_dir)
        {
            DirectoryInfoGatherer directoryInfoGatherer = new DirectoryInfoGatherer(search_dir);
            Dictionary<string, int> extensionInfo = directoryInfoGatherer.GatherInfo();
            e.Result = extensionInfo;
        }

        private void worker_DirGathererWorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Dictionary<string, int> extensionInfo = e.Result as Dictionary<string, int>;

            int nrOfFileInCurrentDir = 0;
            foreach (var item in extensionInfo)
            {
                nrOfFileInCurrentDir += item.Value;
            }
            this.moverModel.extensionMapInCurrentDir = extensionInfo;
            this.moverModel.nrOfFilesInCurrentDir = nrOfFileInCurrentDir;
            
            this.moverModel.gatherDirInfoRunning = false;
        }
    }
}
