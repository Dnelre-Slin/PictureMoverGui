using MediaDevices;
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
    //public class UsbDeviceLookupWorker
    //{
    //    private BackgroundWorker _worker;
    //    //private string _chosenMediaDeviceName;
    //    //private Action<WorkStatus, CollectiveDeviceInfoModel> _workDone;
    //    private Action<WorkStatus, Dictionary<string, MediaDevice>, Dictionary<string, string>> _workDone;
    //    private WorkStatus _workStatus;
    //    private ManualResetEvent _manualEvent;
    //    private Dictionary<string, MediaDevice> _mediaDeviceDict;
    //    private Dictionary<string, string> _removableDeviceDict;

    //    public UsbDeviceLookupWorker()
    //    {
    //        _worker = null;
    //    }

    //    public void StartWorker(Action<WorkStatus, Dictionary<string, MediaDevice>, Dictionary<string, string>> workDone)
    //    {
    //        if (_worker == null) // Make sure it is not already running
    //        {
    //            //_chosenMediaDeviceName = chosenMediaDeviceName;
    //            _workDone = workDone;

    //            _workStatus = WorkStatus.Unfinished;

    //            _manualEvent = new ManualResetEvent(false);

    //            _mediaDeviceDict = null;
    //            _removableDeviceDict = null;

    //            _worker = new BackgroundWorker();
    //            _worker.WorkerSupportsCancellation = true;
    //            _worker.DoWork += worker_DoWork;
    //            _worker.RunWorkerCompleted += worker_WorkDone;
    //            _worker.RunWorkerAsync();
    //        }
    //    }

    //    public void CancelWorker()
    //    {
    //        if (_worker != null) // Make sure it is running
    //        {
    //            _worker.CancelAsync();
    //            _manualEvent.Set();
    //        }
    //    }

    //    private void worker_DoWork(object sender, DoWorkEventArgs e)
    //    {
    //        try
    //        {

    //            //foreach (var mos in new System.Management.ManagementObjectSearcher("Select * From Win32_PnPEntity").Get())
    //            //{
    //            //    if (mos.GetPropertyValue("Name")?.ToString().Contains("Nils") ?? false)
    //            //    {
    //            //        System.Diagnostics.Debug.WriteLine("#######################################");
    //            //        System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("DeviceID"));
    //            //        System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Name"));
    //            //        System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Availability"));
    //            //        System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Description"));
    //            //        System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("PNPDeviceID"));
    //            //        System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Status"));
    //            //        System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Caption"));
    //            //        System.Diagnostics.Debug.WriteLine("---------------------------------------");
    //            //    }
    //            //}

    //            //foreach (var mos in new System.Management.ManagementObjectSearcher("Select Name From Win32_PnPEntity").Get())
    //            //{
    //            //    if (mos.GetPropertyValue("Name")?.ToString().Contains("Nils") ?? false)
    //            //    {
    //            //        //System.Diagnostics.Debug.WriteLine("#######################################");
    //            //        ////System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("DeviceID"));
    //            //        //System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Name"));
    //            //        ////System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Availability"));
    //            //        ////System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Description"));
    //            //        ////System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("PNPDeviceID"));
    //            //        ////System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Status"));
    //            //        ////System.Diagnostics.Debug.WriteLine(mos.GetPropertyValue("Caption"));
    //            //        //System.Diagnostics.Debug.WriteLine("---------------------------------------");
    //            //    }
    //            //}
    //            //new System.Management.ManagementObjectSearcher("Select * From Win32_PnPEntity").Get(); // Refresh for MediaDevices below
    //            //foreach (var mos in new System.Management.ManagementObjectSearcher("Select * From Win32_PnPEntity").Get()) { }


    //            //_manualEvent.WaitOne(300); // Wait for possible multiple usb events in a row, to only have to do the look up once, for several events in a short interval of time
    //            _manualEvent.WaitOne(1000); // Wait for possible multiple usb events in a row, to only have to do the look up once, for several events in a short interval of time
    //            //_manualEvent.WaitOne(3000); // Wait for possible multiple usb events in a row, to only have to do the look up once, for several events in a short interval of time

    //            if (_worker.CancellationPending)
    //            {
    //                _workStatus = WorkStatus.Cancelled;
    //                return;
    //            }

    //            //List<DriveInfoModel> driveInfoList = UsbDeviceListGatherer.GetDriveInfoList();
    //            //List<string> exclusionList = driveInfoList.Select(di => di.SerialId).ToList();
    //            //List<MediaDeviceModel> mediaDeviceList = UsbDeviceListGatherer.GetMediaDeviceList(exclusionList);
    //            ////MediaDeviceModel selectedMediaDevice = mediaDeviceList.Find(md => md.FriendlyName == _chosenMediaDeviceName);
    //            ////CollectiveDeviceInfoModel collectiveDeviceInfo = new CollectiveDeviceInfoModel(driveInfoList, mediaDeviceList, selectedMediaDevice);
    //            //CollectiveDeviceInfoModel collectiveDeviceInfo = new CollectiveDeviceInfoModel(driveInfoList, mediaDeviceList);

    //            //Dictionary<string, string> removableDeviceDict = new Dictionary<string, string>();
    //            _removableDeviceDict = new Dictionary<string, string>();
    //            foreach (MediaDevice removableDevice in MediaDevice.GetDevices())
    //            {
    //                removableDevice.Connect();
    //                if (removableDevice.Protocol.Contains("MSC"))
    //                {
    //                    //mediaDeviceList.Add(new MediaDeviceModel(mediaDevice.FriendlyName, mediaDevice.SerialNumber, DateTime.MinValue, mediaDevice));
    //                    _removableDeviceDict.Add(removableDevice.SerialNumber, RemovableDeviceLookup.GetDriveLetterFromSerialNumber(removableDevice.SerialNumber));

    //                }
    //                removableDevice.Disconnect();
    //            }


    //            //Dictionary<string, MediaDevice> mediaDeviceDict = new Dictionary<string, MediaDevice>();
    //            _mediaDeviceDict = new Dictionary<string, MediaDevice>();
    //            //foreach (MediaDevices.MediaDevice mediaDevice in MediaDevices.MediaDevice.GetDevices())
    //            //foreach (MediaDevices.MediaDevice mediaDevice in MediaDevice.GetDevices())
    //            //var x = MediaDevices.MediaDevice.GetDevices();
    //            foreach (MediaDevice mediaDevice in MediaDevice.GetDevices())
    //            {
    //                mediaDevice.Connect();
    //                if (mediaDevice.Protocol.Contains("MTP"))
    //                {
    //                    //mediaDeviceList.Add(new MediaDeviceModel(mediaDevice.FriendlyName, mediaDevice.SerialNumber, DateTime.MinValue, mediaDevice));
    //                    _mediaDeviceDict.Add(mediaDevice.SerialNumber, mediaDevice);

    //                }
    //                mediaDevice.Disconnect();
    //            }

    //            if (_worker.CancellationPending)
    //            {
    //                _workStatus = WorkStatus.Cancelled;
    //                return;
    //            }

    //            _workStatus = WorkStatus.Success;
    //            //e.Result = mediaDeviceDict;
    //            //e.Result = collectiveDeviceInfo;
    //        }
    //        catch (Exception err)
    //        {
    //            System.Diagnostics.Trace.TraceError($"UsbDeviceLookupWorker crashed unexpectedly in worker_DoWork. Message: {err.Message}");
    //        }
    //    }

    //    private void worker_WorkDone(object sender, RunWorkerCompletedEventArgs e)
    //    {
    //        //CollectiveDeviceInfoModel collectiveDeviceInfo = e.Result as CollectiveDeviceInfoModel;
    //        //Dictionary<string, MediaDevice> mediaDeviceDict = e.Result as Dictionary<string, MediaDevice>;

    //        _worker = null;
    //        _workDone(_workStatus, _mediaDeviceDict, _removableDeviceDict);
    //        _mediaDeviceDict = null;
    //        _removableDeviceDict = null;
    //    }
    //}
}
