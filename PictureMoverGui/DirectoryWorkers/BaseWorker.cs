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
        //protected ManualResetEvent _workSuspender;
        //protected bool _isSuspended;

        public bool CancellationPending => _worker.CancellationPending;
        //public ManualResetEvent WorkSuspender => _workSuspender;

        public BaseWorker()
        {
            _workStatus = WorkStatus.Unfinished;
            //_workSuspender = new ManualResetEvent(true);
            //_isSuspended = false;

            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_WorkDone;
        }

        //public void BaseSetupWorker()
        //{
        //    if (_worker == null)
        //    {
        //        _workStatus = WorkStatus.Unfinished;

        //        _worker = new BackgroundWorker();
        //        _worker.WorkerSupportsCancellation = true;
        //        _worker.DoWork += worker_DoWork;
        //        _worker.RunWorkerCompleted += worker_WorkDone;
        //    }
        //}

        public void StartWorker()
        {
            _worker.RunWorkerAsync(); // Start new worker
            //if (_isSuspended)
            //{
            //    ResumeWorker();
            //}
            //else
            //{
            //    _worker.RunWorkerAsync(); // Start new worker
            //}
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

        //public void PauseWorker()
        //{
        //    if (_worker != null)
        //    {
        //        _workSuspender.Reset(); // Suspend worker
        //        _isSuspended = true;
        //    }
        //}

        //public void ResumeWorker()
        //{
        //    if (_worker != null)
        //    {
        //        _workSuspender.Set(); // Resume worker
        //        _isSuspended = false;
        //    }
        //}

        public bool IsRunning()
        {
            return _worker != null;
        }

        abstract protected void worker_DoWork(object sender, DoWorkEventArgs e);

        abstract protected void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e);
    }
}
