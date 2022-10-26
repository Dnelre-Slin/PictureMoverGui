using PictureMoverGui.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class RunningStore
    {
        public event Action<RunningStore> RunningStoreChanged;

        public RunStates RunState { get; private set; }
        public double StatusPercentage { get; private set; }
        public string StatusMessage { get; private set; }

        public RunningStore()
        {
            RunState = RunStates.Idle;
            StatusPercentage = 0.0;
            StatusMessage = "";
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
