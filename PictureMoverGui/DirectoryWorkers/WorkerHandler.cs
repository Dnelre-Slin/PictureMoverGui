using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.DirectoryWorkers
{
    public class WorkerHandler
    {
        public event Action<RunStates> RunStateChanged;
        //private ExtensionCounterWorker _extensionCounterWorker;
        //private PictureMoverWorker _pictureMoverWorker;

        private Queue<BaseWorker> _workerQueue;
        //private Queue<BaseWorker> _workerSuspendedQueue;

        public RunStates RunState { get; private set; }

        public WorkerHandler()
        {
            //_extensionCounterWorker = new ExtensionCounterWorker();
            //_pictureMoverWorker = new PictureMoverWorker();

            _workerQueue = new Queue<BaseWorker>();

            RunState = RunStates.Idle;
        }

        public void Dispose()
        {
            if (_workerQueue.Count > 0)
            {
                _workerQueue.Peek().CancelWorker();
                _workerQueue.Clear();
            }
        }

        public void StartExtensionCounterWorker(ExtensionCounterArguments extensionCounterArguments)
        {
            _workerQueue.Enqueue(new ExtensionCounterWorker(extensionCounterArguments, OnWorkerDone));
            RunNextWorker();
        }

        public void CancelExtensionCounterWorker()
        {
            if (IsExtensionCounterRunning())
            {
                _workerQueue.Peek().CancelWorker();
            }
        }

        public void InteruptExtensionCounterWorker()
        {
            if (IsExtensionCounterRunning())
            {
                _workerQueue.Peek().InteruptWorker();
            }
        }

        //public void SuspendAndPushBackExtensionCounterWorker()
        //{
        //    if (IsExtensionCounterRunning())
        //    {
        //        BaseWorker currentWorker = _workerQueue.Dequeue();
        //        currentWorker.PauseWorker();
        //        _workerQueue.Enqueue(currentWorker);
        //        RunState = RunStates.Idle;
        //        RunNextWorker();
        //    }
        //}

        protected bool IsExtensionCounterRunning()
        {
            return (RunState != RunStates.Idle && _workerQueue.Peek() is ExtensionCounterWorker);
        }

        //public void SuspendExtensionCounterWorker()
        //{
        //    if (RunState != RunStates.Idle && _workerQueue.Peek() is ExtensionCounterWorker)
        //    {
        //        _workerQueue.Peek().CancelWorker();
        //    }
        //}

        public void StartPictureMoverWorker(PictureMoverArguments pictureMoverArguments)
        {
            _workerQueue.Enqueue(new PictureMoverWorker(pictureMoverArguments, UpdateRunState, OnWorkerDone));
            RunNextWorker();
        }

        public void CancelPictureMoverWorker()
        {
            if (IsPictureMoverRunning())
            {
                _workerQueue.Peek().CancelWorker();
            }
        }

        protected bool IsPictureMoverRunning()
        {
            return (RunState != RunStates.Idle && _workerQueue.Peek() is PictureMoverWorker);
        }

        protected void UpdateRunState(RunStates runState)
        {
            RunState = runState;
        }

        protected void RunNextWorker()
        {
            if (RunState == RunStates.Idle)
            {
                if (_workerQueue.Count > 0)
                {
                    //WorkerArgumentWrapper workerArgumentWrapper = _workerQueue.Dequeue();
                    BaseWorker worker = _workerQueue.Peek();
                    if (worker is ExtensionCounterWorker)
                    {
                        RunState = RunStates.DirectoryGathering;
                    }
                    else if (worker is PictureMoverWorker)
                    {
                        RunState = RunStates.DirectoryValidation;
                    }
                    else
                    {
                        throw new NotImplementedException($"RunNextWorker has unhandled type : {worker}");
                    }
                    worker.StartWorker();
                    RunStateChanged?.Invoke(RunState);
                }
            }
        }

        protected void OnWorkerDone(BaseWorker worker)
        {
            _workerQueue.Dequeue();
            RunState = RunStates.Idle;
            RunStateChanged?.Invoke(RunState);
            RunNextWorker();
        }
    }
}
