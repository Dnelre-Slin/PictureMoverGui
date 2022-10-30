using PictureMoverGui.DirectoryUtils;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PictureMoverGui.DirectoryWorkers
{
    public class ExtensionCounterWorker
    {
        private BackgroundWorker _worker;
        private ExtensionCounterArguments _extensionCounterArguments;
        private WorkStatus _workStatus;

        public ExtensionCounterWorker()
        {
            _worker = null;
        }

        public void StartWorker(ExtensionCounterArguments extensionCounterArguments)
        {
            if (_worker == null && extensionCounterArguments.RunState == RunStates.Idle) // Make sure it is not already running
            {
                _extensionCounterArguments = extensionCounterArguments;
                _extensionCounterArguments.UpdateRunState?.Invoke(RunStates.DirectoryGathering);

                _workStatus = WorkStatus.Unfinished;

                _worker = new BackgroundWorker();
                _worker.WorkerSupportsCancellation = true;
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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, int> extensionInfo = PictureRetriever.GetExtensions(
                    _extensionCounterArguments.MediaType, 
                    _extensionCounterArguments.Source, 
                    sender as BackgroundWorker, 
                    _extensionCounterArguments.NewerThan);

                if (_worker.CancellationPending)
                {
                    _workStatus = WorkStatus.Cancelled;
                }
                else if (extensionInfo == null)
                {
                    _workStatus = WorkStatus.Invalid;
                }
                else
                {
                    _workStatus = WorkStatus.Success;
                }
                e.Result = extensionInfo;
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"ExtensionCounterWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
            }
        }

        private void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Dictionary<string, int> extensionInfo = e.Result as Dictionary<string, int>;

            _worker = null;
            _extensionCounterArguments.UpdateRunState?.Invoke(RunStates.Idle);
            _extensionCounterArguments.WorkDone?.Invoke(_workStatus, extensionInfo);
        }
    }
}
