using PictureMoverGui.DirectoryUtils;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace PictureMoverGui.DirectoryWorkers
{
    public class PictureMoverWorker : BaseWorker
    {
        protected PictureMoverArguments _pictureMoverArguments;
        protected Action<RunStates> _updateRunState;
        protected Action<BaseWorker> _workDoneCallback;

        public PictureMoverWorker(PictureMoverArguments pictureMoverArguments, Action<RunStates> updateRunState, Action<BaseWorker> workDoneCallback) : base ()
        {
            _worker.WorkerReportsProgress = true;
            _worker.ProgressChanged += worker_PictureMoverProgressChanged;


            _pictureMoverArguments = pictureMoverArguments;
            _updateRunState = updateRunState;
            _workDoneCallback = workDoneCallback;

            //_pictureMoverArguments.UpdateRunState?.Invoke(RunStates.DirectoryValidation);

            _workStatus = WorkStatus.Unfinished;
        }

        //public void SetupWorker(PictureMoverArguments pictureMoverArguments)
        //{
        //    if (_worker == null && pictureMoverArguments.RunState == RunStates.Idle) // Make sure it is not already running
        //    {
        //        BaseSetupWorker();
        //        _worker.WorkerReportsProgress = true;
        //        _worker.ProgressChanged += worker_PictureMoverProgressChanged;


        //        _pictureMoverArguments = pictureMoverArguments;

        //        _pictureMoverArguments.UpdateRunState?.Invoke(RunStates.DirectoryValidation);

        //        _workStatus = WorkStatus.Unfinished;
        //    }
        //}

        //public void CancelWorker()
        //{
        //    if (_worker != null) // Make sure it is running
        //    {
        //        _worker.CancelAsync();
        //    }
        //}

        protected void worker_PictureMoverProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _pictureMoverArguments.UpdateRunPercentage?.Invoke(e.ProgressPercentage);
        }

        protected override void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (PictureRetriever pictureRetriever = new PictureRetriever(_pictureMoverArguments.SorterMediaType, _pictureMoverArguments.PictureRetrieverSource, _pictureMoverArguments.SelectedMediaDevice))
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
                        .IncrementInfoFileCount(_pictureMoverArguments.IncrementInfoFileCount)
                        .ToList();

                    if (_worker.CancellationPending)
                    {
                        _pictureMoverArguments.AddRunStatusLog?.Invoke("Cancelled during preparation");
                        //_workStatus = WorkStatus.Cancelled;
                        return;
                    }

                    //_pictureMoverArguments.UpdateRunState?.Invoke(RunStates.RunningSorter);
                    _updateRunState?.Invoke(RunStates.RunningSorter);

                    PictureMover pictureMover = new PictureMover(_pictureMoverArguments, fileInfoList, sender as BackgroundWorker);
                    int nrOfErrors = pictureMover.Mover();

                    if (_worker.CancellationPending)
                    {
                        _pictureMoverArguments.AddRunStatusLog?.Invoke("Cancelled during running");
                        //_workStatus = WorkStatus.Cancelled;
                        return;
                    }

                    _workStatus = WorkStatus.Success;
                    e.Result = nrOfErrors;
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"PictureMoverWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
            }
        }

        protected override void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            int nrOfErrors = e.Result != null ? (int)e.Result : -1;

            _worker = null;
            _pictureMoverArguments.UpdateRunPercentage(0);
            //_pictureMoverArguments.UpdateRunState(RunStates.Idle);
            _workDoneCallback?.Invoke(this);
            _pictureMoverArguments.WorkDone(_workStatus, nrOfErrors);
        }
    }
}
