using MediaDevices;
using PictureMoverGui.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace PictureMoverGui.DeviceWorkers
{
    public class RemovableDeviceChangeWorker
    {
        private BackgroundWorker _worker;
        private DeviceChangeType _deviceChangeType;
        private int _currentCount;
        private Action<WorkStatus, Dictionary<string, string>> _workDone;
        private WorkStatus _workStatus;
        private ManualResetEvent _manualEvent;

        public RemovableDeviceChangeWorker()
        {
            _worker = null;
        }

        public void StartWorker(DeviceChangeType deviceChangeType, int currentCount, Action<WorkStatus, Dictionary<string, string>> workDone)
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

                Dictionary<string, string> removableDeviceDict = new Dictionary<string, string>();

                int newCount;
                int index = 0;

                while (index < stopIndex)
                {
                    foreach (var y in new ManagementObjectSearcher("Select Name, VolumeSerialNumber From Win32_LogicalDisk Where Description='Removable Disk'").Get())
                    {
                        removableDeviceDict.Add(y.GetPropertyValue("VolumeSerialNumber").ToString(), y.GetPropertyValue("Name").ToString());
                    }
                    newCount = removableDeviceDict.Count;

                    if ((_deviceChangeType == DeviceChangeType.None) ||
                        (_deviceChangeType == DeviceChangeType.Added && newCount > _currentCount) ||
                        (_deviceChangeType == DeviceChangeType.Removed && newCount < _currentCount)
                        )
                    {
                        index = stopIndex;
                    }
                    else
                    {
                        removableDeviceDict.Clear();
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
                e.Result = removableDeviceDict;
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"UsbDeviceLookupWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
            }
        }

        private void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Dictionary<string, string> removableDeviceDict = e.Result as Dictionary<string, string>;

            _worker = null;
            _workDone(_workStatus, removableDeviceDict);
        }
    }
}
