using PictureMoverGui.DirectoryUtils;
using PictureMoverGui.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PictureMoverGui.DirectoryWorkers
{
    public class ExtensionCounterWorker
    {
        private BackgroundWorker _worker;
        private MediaTypeEnum _mediaType;
        private string _source;
        private DateTime _newerThan;
        private Action<RunStates> _updateRunState;
        private Action<Dictionary<string, int>> _workDone;

        public ExtensionCounterWorker(MediaTypeEnum mediaType, string source, DateTime newerThan, Action<RunStates> updateRunState, Action<Dictionary<string, int>> workDone)
        {
            _mediaType = mediaType;
            _source = source;
            _newerThan = newerThan;
            _updateRunState = updateRunState;
            _workDone = workDone;

            if (_updateRunState != null)
            {
                _updateRunState(RunStates.DirectoryGathering);
            }

            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_WorkDone;
            _worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, int> extensionInfo = PictureRetriever.GetExtensions(_mediaType, _source, sender as BackgroundWorker, _newerThan);
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

            if (e.Cancelled || extensionInfo == null)
            {
                // All did not work fine (Cancelled | Failed)
            }
            else
            {
                // All worked fine. (Success)
            }
            _worker = null;
            if (_updateRunState != null)
            {
                _updateRunState(RunStates.Idle);
            }
            if (_workDone != null)
            {
                _workDone(extensionInfo);
            }
        }
    }
}
