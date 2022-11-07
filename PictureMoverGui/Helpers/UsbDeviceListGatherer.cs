using MediaDevices;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace PictureMoverGui.Helpers
{
    //public static class UsbDeviceListGatherer
    //{
    //    public static List<RemovableDeviceModel> GetDriveInfoList()
    //    {
    //        List<RemovableDeviceModel> driveInfoList = new List<RemovableDeviceModel>();

    //        ManagementClass logicalDisks = new ManagementClass("Win32_LogicalDisk");
    //        {
    //            foreach (ManagementObject ld in logicalDisks.GetInstances())
    //            {
    //                foreach (ManagementObject dp in ld.GetRelated("Win32_DiskPartition"))
    //                {
    //                    foreach (ManagementObject dd in dp.GetRelated("Win32_Diskdrive"))
    //                    {
    //                        driveInfoList.Add(new RemovableDeviceModel(ld["DeviceId"]?.ToString(), dd["SerialNumber"]?.ToString()));
    //                    }
    //                }
    //            }
    //        }

    //        return driveInfoList;
    //    }

    //    public static List<MediaDeviceModel> GetMediaDeviceList(List<string> exclusionList)
    //    {
    //        List<MediaDeviceModel> mediaDeviceList = new List<MediaDeviceModel>();

    //        foreach (MediaDevice mediaDevice in MediaDevice.GetDevices())
    //        {
    //            if (mediaDevice.DeviceId != null)
    //            {
    //                mediaDevice.Connect();
    //                if (!exclusionList.Contains(mediaDevice.SerialNumber))
    //                {
    //                    mediaDeviceList.Add(new MediaDeviceModel(mediaDevice.FriendlyName, mediaDevice.SerialNumber, mediaDevice));
    //                }
    //                mediaDevice.Disconnect();
    //            }
    //        }

    //        return mediaDeviceList;
    //    }
    //}
}
