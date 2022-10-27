using PictureMoverGui.DirectoryUtils;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace PictureMoverGui.DirectoryWorkers
{
    public class PictureMoverWorker
    {
        private BackgroundWorker _worker;
        private PictureMoverArguments _pictureMoverArguments;
        private WorkStatus _workStatus;

        public PictureMoverWorker()
        {
            _worker = null;
        }

        public void StartWorker(PictureMoverArguments pictureMoverArguments)
        {
            if (_worker == null && pictureMoverArguments.RunState == RunStates.Idle) // Make sure it is not already running
            {
                _pictureMoverArguments = pictureMoverArguments;

                _pictureMoverArguments.UpdateRunState?.Invoke(RunStates.DirectoryValidation);

                _workStatus = WorkStatus.Unfinished;

                _worker = new BackgroundWorker();
                _worker.WorkerReportsProgress = true;
                _worker.WorkerSupportsCancellation = true;
                _worker.ProgressChanged += worker_PictureMoverProgressChanged;
                _worker.DoWork += worker_DoWork;
                _worker.RunWorkerCompleted += worker_WorkDone;
                _worker.RunWorkerAsync();
            }
        }

        public void CancelWorker()
        {
            if (_worker != null) // Make sure it is running
            {
                _worker.CancelAsync();
            }
        }

        private void worker_PictureMoverProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _pictureMoverArguments.UpdateRunPercentage?.Invoke(e.ProgressPercentage);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (PictureRetriever pictureRetriever = new PictureRetriever(_pictureMoverArguments.SorterMediaType, _pictureMoverArguments.PictureRetrieverSource))
                {
                    if (!pictureRetriever.IsValid)
                    {
                        _pictureMoverArguments.AddRunStatusLog?.Invoke("Source dir no longer exists");
                        _workStatus = WorkStatus.Invalid;
                        return;
                    }

                    List<GenericFileInfo> fileInfoList = pictureRetriever.EnumerateFiles("*", SearchOption.AllDirectories)
                        .Where(f => WorkerHelpers.IsValidFileExtension(f.Extension, _pictureMoverArguments.ValidExtensions) && 
                                    WorkerHelpers.IsFileNewerThan(f.LastWriteTime, _pictureMoverArguments.PictureRetrieverNewerThan))
                        .CancelWorker(_worker)
                        .CatchUnauthorizedAccessExceptions(WorkerHelpers.HandleFileAccessExceptions)
                        .ToList();

                    _pictureMoverArguments.UpdateRunState?.Invoke(RunStates.RunningSorter);

                    if (_worker.CancellationPending)
                    {
                        _pictureMoverArguments.AddRunStatusLog?.Invoke("Cancelled during preparation");
                        _workStatus = WorkStatus.Cancelled;
                        return;
                    }

                    PictureMover pictureMover = new PictureMover(_pictureMoverArguments, fileInfoList, sender as BackgroundWorker);
                    //List<string> infoStatusMessages = pictureMover.Mover();
                    //e.Result = infoStatusMessages;
                    int nrOfErrors = pictureMover.Mover();
                    _workStatus = WorkStatus.Success;
                    e.Result = nrOfErrors;
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"PictureMoverWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
            }
        }

        private void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            int nrOfErrors = (int)e.Result;

            //if (_workStatus == WorkStatus.Invalid)
            //{
            //    MessageBox.Show("The source dir no longer exists. Please start select source again", "Source dir no longer exists");
            //    //this.moverModel.labelSourceDirContent = "";
            //}

            //this.moverModel.infoStatusMessagesLastRun = infoStatusMessages;
            //this.moverModel.statusPercentage = 0;

            _worker = null;
            _pictureMoverArguments.UpdateRunPercentage(0);
            _pictureMoverArguments.UpdateRunState(RunStates.Idle);
            _pictureMoverArguments.WorkDone(_workStatus, nrOfErrors);
        }
    }
}
