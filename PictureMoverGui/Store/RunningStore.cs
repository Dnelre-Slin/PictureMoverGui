using PictureMoverGui.Helpers;
using System;
using System.Collections.Generic;

namespace PictureMoverGui.Store
{
    public class RunningStore
    {
        public event Action<RunningStore> RunningStoreChanged;
        public event Action<string> StatusMessageLogAdded;

        public RunStates RunState { get; private set; }
        public double StatusPercentage { get; private set; }
        public string StatusMessage { get; private set; }
        public int InfoFileCount { get; private set; }

        private List<string> _statusMessageLogList;
        public IEnumerable<string> StatusMessageLogList => _statusMessageLogList;

        public RunningStore()
        {
            RunState = RunStates.Idle;
            StatusPercentage = 0.0;
            StatusMessage = "";
            InfoFileCount = 0;
            _statusMessageLogList = new List<string>();
        }

        public void SetRunState(RunStates runState)
        {
            RunState = runState;
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
            RunningStoreChanged?.Invoke(this);
        }

        public void ResetInfoFileCount()
        {
            InfoFileCount = 0;
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
                    return App.Current.FindResource("DirValidateStatusMessage").ToString();
                case RunStates.DirectoryGathering:
                    return App.Current.FindResource("DirGatherStatusMessage").ToString();
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
