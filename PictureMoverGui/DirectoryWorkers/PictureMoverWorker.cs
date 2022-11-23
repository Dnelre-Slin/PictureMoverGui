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
        protected List<string> _statusLogList;

        public PictureMoverWorker(PictureMoverArguments pictureMoverArguments, Action<RunStates> updateRunState, Action<BaseWorker> workDoneCallback) : base ()
        {
            _worker.WorkerReportsProgress = true;
            _worker.ProgressChanged += worker_PictureMoverProgressChanged;

            _pictureMoverArguments = pictureMoverArguments;
            _updateRunState = updateRunState;
            _workDoneCallback = workDoneCallback;
            _statusLogList = new List<string>();

            _workStatus = WorkStatus.Unfinished;
        }

        protected void AddStatusLog(string statusLog)
        {
            _statusLogList.Add(statusLog);
        }

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
                        //_pictureMoverArguments.AddRunStatusLog?.Invoke("Source dir no longer exists");
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
                        //_pictureMoverArguments.AddRunStatusLog?.Invoke("Cancelled during preparation");
                        return;
                    }

                    _updateRunState?.Invoke(RunStates.RunningSorter);

                    PictureMover pictureMover = new PictureMover(_pictureMoverArguments, fileInfoList, sender as BackgroundWorker, AddStatusLog);
                    int nrOfErrors = pictureMover.Mover();

                    if (_worker.CancellationPending)
                    {
                        //_pictureMoverArguments.AddRunStatusLog?.Invoke("Cancelled during running");
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
            
            foreach (string log in _statusLogList)
            {
                _pictureMoverArguments.AddRunStatusLog?.Invoke(log);
            }

            _worker = null;
            _pictureMoverArguments.UpdateRunPercentage(0);
            _workDoneCallback?.Invoke(this);
            _pictureMoverArguments.WorkDone(_workStatus, nrOfErrors);
        }
    }
}
