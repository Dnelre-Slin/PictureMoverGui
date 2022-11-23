using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PictureMoverGui.Store
{
    public class RunningStore
    {
        public event Action<RunningStore> RunningStoreChanged;
        public event Action<string> StatusMessageLogAdded;

        public WorkerHandler _workerHandler;
        public WorkerHandler WorkerHandler => _workerHandler;

        public RunStates RunState => _workerHandler.RunState;
        public double StatusPercentage { get; private set; }
        public string StatusMessage { get; private set; }
        public int InfoFileCount { get; private set; }

        private ObservableCollection<string> _statusMessageLogList;
        public IEnumerable<string> StatusMessageLogList => _statusMessageLogList;

        public bool IsMediaLocked { get; private set; }

        public RunningStore()
        {
            _workerHandler = new WorkerHandler();
            StatusPercentage = 0.0;
            StatusMessage = "";
            InfoFileCount = 0;
            _statusMessageLogList = new ObservableCollection<string>();
            IsMediaLocked = true;

            _workerHandler.RunStateChanged += WorkerHandler_RunStateChanged;
        }

        public void Dispose()
        {
            _workerHandler.RunStateChanged -= WorkerHandler_RunStateChanged;
            _workerHandler.Dispose();
        }

        protected void WorkerHandler_RunStateChanged(RunStates runState)
        {
            SetStatusMessage();
            RunningStoreChanged?.Invoke(this);
        }

        public void SetStatusPercentage(double statusPercentage)
        {
            StatusPercentage = statusPercentage;
            SetStatusMessage();
            RunningStoreChanged?.Invoke(this);
        }

        public void AddStatusLog(string statusLog)
        {
            _statusMessageLogList.Add(statusLog);
            StatusMessageLogAdded?.Invoke(statusLog);
        }

        public void IncrementInfoFileCount(int incrementAmount = 1)
        {
            InfoFileCount += incrementAmount;
            StatusMessage = CalculateStatusMessage();
            RunningStoreChanged?.Invoke(this);
        }

        public void ResetInfoFileCount()
        {
            InfoFileCount = 0;
            StatusMessage = CalculateStatusMessage();
            RunningStoreChanged?.Invoke(this);
        }

        public void SetMediaLockedState(bool locked)
        {
            IsMediaLocked = locked;
            RunningStoreChanged?.Invoke(this);
        }

        protected void SetStatusMessage()
        {
            StatusMessage = CalculateStatusMessage();
        }

        protected string CalculateStatusMessage()
        {
            switch (RunState)
            {
                case RunStates.DirectoryValidation:
                    //return App.Current.FindResource("DirValidateStatusMessage").ToString();
                    return $"{App.Current.FindResource("FileCountStatusMessage")} {InfoFileCount}";
                case RunStates.DirectoryGathering:
                    return App.Current?.FindResource("ReadyStatusMessage").ToString() ?? "";
                    //return App.Current.FindResource("DirGatherStatusMessage").ToString();
                case RunStates.RunningSorter:
                    return $"{StatusPercentage}%";
                case RunStates.Idle:
                    return App.Current.FindResource("ReadyStatusMessage").ToString();
                    //if (AllowStartingMover)
                    //{
                    //    return App.Current.FindResource("ReadyStatusMessage").ToString();
                    //}
                    //else
                    //{
                    //    return App.Current.FindResource("NotReadyStatusMessage").ToString();
                    //}
                default:
                    throw new NotImplementedException($"RunningStore.CalculateStatusMessage switch is not accoutning for RunState: {RunState}");
            }
        }
    }
}
