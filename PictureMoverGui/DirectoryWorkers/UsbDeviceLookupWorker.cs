using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace PictureMoverGui.DirectoryWorkers
{
    public class UsbDeviceLookupWorker
    {
        private BackgroundWorker _worker;
        //private string _chosenMediaDeviceName;
        private Action<WorkStatus, CollectiveDeviceInfoModel> _workDone;
        private WorkStatus _workStatus;
        private ManualResetEvent _manualEvent;

        public UsbDeviceLookupWorker()
        {
            _worker = null;
        }

        public void StartWorker(Action<WorkStatus, CollectiveDeviceInfoModel> workDone)
        {
            if (_worker == null) // Make sure it is not already running
            {
                //_chosenMediaDeviceName = chosenMediaDeviceName;
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
                _manualEvent.WaitOne(300); // Wait for possible multiple usb events in a row, to only have to do the look up once, for several events in a short interval of time

                if (_worker.CancellationPending)
                {
                    _workStatus = WorkStatus.Cancelled;
                    return;
                }

                List<DriveInfoModel> driveInfoList = UsbDeviceListGatherer.GetDriveInfoList();
                List<string> exclusionList = driveInfoList.Select(di => di.SerialId).ToList();
                List<MediaDeviceModel> mediaDeviceList = UsbDeviceListGatherer.GetMediaDeviceList(exclusionList);
                //MediaDeviceModel selectedMediaDevice = mediaDeviceList.Find(md => md.FriendlyName == _chosenMediaDeviceName);
                //CollectiveDeviceInfoModel collectiveDeviceInfo = new CollectiveDeviceInfoModel(driveInfoList, mediaDeviceList, selectedMediaDevice);
                CollectiveDeviceInfoModel collectiveDeviceInfo = new CollectiveDeviceInfoModel(driveInfoList, mediaDeviceList);

                _workStatus = WorkStatus.Success;
                e.Result = collectiveDeviceInfo;
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError($"ExtensionCounterWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
            }
        }

        private void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            CollectiveDeviceInfoModel collectiveDeviceInfo = e.Result as CollectiveDeviceInfoModel;

            _worker = null;
            _workDone(_workStatus, collectiveDeviceInfo);
        }
    }
}
