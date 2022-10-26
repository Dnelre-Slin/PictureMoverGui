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
            StatusMessage = "Klor";
        }

        public void SetRunState(RunStates runState)
        {
            RunState = runState;
            RunningStoreChanged?.Invoke(this);
        }

        public void SetStatusPercentage(double statusPercentage)
        {
            StatusPercentage = statusPercentage;
            RunningStoreChanged?.Invoke(this);
        }

        public void SetStatusMessage(string statusMessage)
        {
            StatusMessage = statusMessage;
            RunningStoreChanged?.Invoke(this);
        }
    }
}
