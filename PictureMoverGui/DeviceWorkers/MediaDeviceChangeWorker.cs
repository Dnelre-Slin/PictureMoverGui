using MediaDevices;
using PictureMoverGui.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace PictureMoverGui.DeviceWorkers
{
    public class MediaDeviceChangeWorker
    {
        private BackgroundWorker _worker;
        private DeviceChangeType _deviceChangeType;
        private int _currentCount;
        private Action<WorkStatus, Dictionary<string, MediaDevice>> _workDone;
        private WorkStatus _workStatus;
        private ManualResetEvent _manualEvent;

        public MediaDeviceChangeWorker()
        {
            _worker = null;
        }

        public void StartWorker(DeviceChangeType deviceChangeType, int currentCount, Action<WorkStatus, Dictionary<string, MediaDevice>> workDone)
        {
            if (_worker == null) // Make sure it is not already running
            {
                _deviceChangeType = deviceChangeType;
                _currentCount = currentCount;
                _workDone = workDone;

                _workStatus = WorkStatus.Unfinished;

                _manualEvent = new ManualResetEvent(false);

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
                _manualEvent.Set();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                const int stopIndex = 50;
                const int waitTimeMs = 100; // Max wait time = 50 * 100 ms = 5 seconds

                Dictionary<string, MediaDevice> mediaDeviceDict = new Dictionary<string, MediaDevice>();

                int newCount;
                int index = 0;

                while (index < stopIndex)
                {
                    System.Diagnostics.Debug.WriteLine("Runnin worker gloop");
                    foreach (MediaDevice mediaDevice in MediaDevice.GetDevices())
                    {
                        mediaDevice.Connect();
                        if (mediaDevice.Protocol.Contains("MTP"))
                        {
                            mediaDeviceDict.Add(mediaDevice.SerialNumber, mediaDevice);

                        }
                        mediaDevice.Disconnect();
                    }
                    newCount = mediaDeviceDict.Count;
                    System.Diagnostics.Debug.WriteLine(_currentCount);
                    System.Diagnostics.Debug.WriteLine(newCount);

                    if ((_deviceChangeType == DeviceChangeType.None) ||
                        (_deviceChangeType == DeviceChangeType.Added && newCount > _currentCount) ||
                        (_deviceChangeType == DeviceChangeType.Removed && newCount < _currentCount)
                        )
                    {
                        index = stopIndex;
                    }
                    else
                    {
                        mediaDeviceDict.Clear();
                        index++;
                        _manualEvent.WaitOne(waitTimeMs);
                    }

                    if (_worker.CancellationPending)
                    {
                        _workStatus = WorkStatus.Cancelled;
                        return;
                    }
                }

                _workStatus = WorkStatus.Success;
                e.Result = mediaDeviceDict;
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"MediaDeviceChangeWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
            }
        }

        private void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Dictionary<string, MediaDevice> mediaDeviceDict = e.Result as Dictionary<string, MediaDevice>;

            _worker = null;
            _workDone(_workStatus, mediaDeviceDict);
        }
    }
}
