using MediaDevices;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace PictureMoverGui.DirectoryWorkers
{
    public class UsbMediaDeviceUnlockWorker
    {
        private BackgroundWorker _worker;
        //private string _chosenMediaDeviceName;
        //private Action<WorkStatus, CollectiveDeviceInfoModel> _workDone;
        private MediaDevice _mediaDevice;
        private Action<WorkStatus> _workDone;
        private WorkStatus _workStatus;
        private ManualResetEvent _manualEvent;

        public UsbMediaDeviceUnlockWorker()
        {
            _worker = null;
        }

        public void StartWorker(MediaDevice mediaDevice, Action<WorkStatus> workDone)
        {
            if (_worker == null) // Make sure it is not already running
            {
                _mediaDevice = mediaDevice;
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
                int count = 0;
                bool runLoop = true;
                
                while (runLoop)
                {
                    System.Diagnostics.Debug.WriteLine("Checking unlock status");
                    if (_worker.CancellationPending)
                    {
                        _workStatus = WorkStatus.Cancelled;
                        return;
                    }

                    count = 0;
                    if (_mediaDevice != null)
                    {
                        _mediaDevice.Connect();
                        count = _mediaDevice.GetDrives().Length;
                        _mediaDevice.Disconnect();
                    }
                    else
                    {
                        _workStatus = WorkStatus.Invalid;
                        return;
                    }

                    if (count > 0)
                    {
                        runLoop = false;
                    }
                    else
                    {
                        _manualEvent.WaitOne(1000); // Wait a little, and check again
                    }
                }

                if (_worker.CancellationPending)
                {
                    _workStatus = WorkStatus.Cancelled;
                    return;
                }

                _workStatus = WorkStatus.Success;
                //e.Result = mediaDeviceList;
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"UsbMediaDeviceUnlockWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
            }
        }

        private void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            //List<MediaDeviceModel> mediaDeviceList = e.Result as List<MediaDeviceModel>;

            _worker = null;
            _workDone(_workStatus);
        }
    }
}
