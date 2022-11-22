using PictureMoverGui.DirectoryUtils;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PictureMoverGui.DirectoryWorkers
{
    public class ExtensionCounterWorker : BaseWorker
    {
        protected ExtensionCounterArguments _extensionCounterArguments;
        protected Action<BaseWorker> _workDoneCallback;

        public ExtensionCounterWorker(ExtensionCounterArguments extensionCounterArguments, Action<BaseWorker> workDoneCallback) : base()
        {
            _extensionCounterArguments = extensionCounterArguments;
            _workDoneCallback = workDoneCallback;

            _workStatus = WorkStatus.Unfinished;
        }

        protected override void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, int> extensionInfo = PictureRetriever.GetExtensions(
                    _extensionCounterArguments.MediaType,
                    _extensionCounterArguments.Source, 
                    _extensionCounterArguments.SelectedMediaDevice,
                    this, 
                    _extensionCounterArguments.IncrementInfoFileCount,
                    _extensionCounterArguments.NewerThan);

                if (_worker.CancellationPending)
                {
                    //_workStatus = WorkStatus.Cancelled;
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

        protected override void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Dictionary<string, int> extensionInfo = e.Result as Dictionary<string, int>;

            _worker = null;
            _workDoneCallback?.Invoke(this);
            _extensionCounterArguments.WorkDone?.Invoke(_workStatus, extensionInfo);
        }
    }
}
