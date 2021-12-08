using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Threading;

namespace PictureMoverGui
{
    class PictureMoverUiHandler
    {
        private PictureMoverModel moverModel;
        private CheckBox chkboxMakeCopies;
        private CheckBox chkboxFolderStructure;
        private CheckBox chkboxRename;
        private Label labelSourceDir;
        private Label labelDestinationDir;
        private Label labelStatusMessage;
        private Microsoft.Expression.Shapes.Arc arcProgressBar;

        private DispatcherTimer statusMessageTimer = new DispatcherTimer();

        public PictureMoverUiHandler(PictureMoverModel moverModel, CheckBox chkboxMakeCopies, CheckBox chkboxFolderStructure, CheckBox chkboxRename, Label labelSourceDir,
                                     Label labelDestinationDir, Label labelStatusMessage, Microsoft.Expression.Shapes.Arc arcProgressBar)
        {
            this.moverModel = moverModel;
            this.chkboxMakeCopies = chkboxMakeCopies;
            this.chkboxFolderStructure = chkboxFolderStructure;
            this.chkboxRename = chkboxRename;
            this.labelSourceDir = labelSourceDir;
            this.labelDestinationDir = labelDestinationDir;
            this.labelStatusMessage = labelStatusMessage;
            this.arcProgressBar = arcProgressBar;

            statusMessageTimer.Tick += UpdateStatusMessage;
        }

        public void StartSorterButtonClick()
        {
            //if (!this.moverModel.sourceDirSat || !this.moverModel.destinationDirSat) // Dont allow run
            //{
            //    return;
            //}
            if (!this.moverModel.AllowStartingMover)
            {
                this.moverModel.pictureMoverRunning = true;

                bool doCopy = this.chkboxMakeCopies.IsChecked.HasValue && chkboxMakeCopies.IsChecked.Value;
                bool doMakeStructures = this.chkboxFolderStructure.IsChecked.HasValue && chkboxFolderStructure.IsChecked.Value;
                bool doRename = this.chkboxRename.IsChecked.HasValue && chkboxRename.IsChecked.Value;
                string path_to_source= this.labelSourceDir.Content.ToString();
                string path_to_destination = this.labelDestinationDir.Content.ToString();

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += (obj, e) => worker_PictureMoverDoWork(obj, e, doCopy, doMakeStructures, doRename, path_to_source, path_to_destination);
                worker.ProgressChanged += worker_PictureMoverProgressChanged;
                worker.RunWorkerCompleted += worker_PictureMoverWorkDone;

                worker.RunWorkerAsync();

                labelStatusMessage.Content = "0%";
            }
        }

        private void worker_PictureMoverDoWork(object sender, DoWorkEventArgs e, bool doCopy, bool doMakeStructures, bool doRename, string path_to_source, string path_to_destination)
        {
            PictureMover pictureMover = new PictureMover(path_to_source, path_to_destination, doCopy, sender as BackgroundWorker, this.moverModel.nrOfFilesInCurrentDir, doMakeStructures, doRename);
            pictureMover.Mover();
            int nrOfErrors = pictureMover.GetNrOfErrors();
            e.Result = nrOfErrors;
        }

        private void worker_PictureMoverProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            labelStatusMessage.Content = $"{progress}%";
            arcProgressBar.EndAngle = progress * 3.6;
        }

        private void worker_PictureMoverWorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            int nrOfErrors = (int)e.Result;
            if (nrOfErrors > 0)
            {
                labelStatusMessage.Content = App.Current.FindResource("ErrorStatusMessage").ToString() + " " + nrOfErrors;
                labelStatusMessage.Foreground = Brushes.Red;
            }
            else
            {
                labelStatusMessage.Content = App.Current.FindResource("DoneStatusMessage");
                labelStatusMessage.Foreground = Brushes.Green;
            }

            arcProgressBar.EndAngle = 0;

            statusMessageTimer.Interval = TimeSpan.FromSeconds((double)App.Current.FindResource("DoneStatusMessageTime"));
            statusMessageTimer.Start();

            this.moverModel.pictureMoverRunning = false;
        }

        private void UpdateStatusMessage(object sender, EventArgs e)
        {
            labelStatusMessage.Content = App.Current.FindResource("ReadyStatusMessage");
            labelStatusMessage.Foreground = Brushes.Black;

            statusMessageTimer.Stop();
        }
    }
}
