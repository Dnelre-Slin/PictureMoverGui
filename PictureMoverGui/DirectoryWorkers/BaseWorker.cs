using PictureMoverGui.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace PictureMoverGui.DirectoryWorkers
{
    abstract public class BaseWorker
    {
        protected BackgroundWorker _worker;
        protected WorkStatus _workStatus;

        public bool CancellationPending => _worker.CancellationPending;

        public BaseWorker()
        {
            _workStatus = WorkStatus.Unfinished;

            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_WorkDone;
        }

        public void StartWorker()
        {
            _worker.RunWorkerAsync(); // Start new worker
        }

        public void CancelWorker()
        {
            if (_worker != null) // Make sure it is running
            {
                _workStatus = WorkStatus.Cancelled;
                _worker.CancelAsync();
            }
        }

        public void InteruptWorker()
        {
            if (_worker != null) // Make sure it is running
            {
                _workStatus = WorkStatus.Interupted;
                _worker.CancelAsync();
            }
        }

        public bool IsRunning()
        {
            return _worker != null;
        }

        abstract protected void worker_DoWork(object sender, DoWorkEventArgs e);

        abstract protected void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e);
    }
}
