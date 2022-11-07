using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class CollectiveDeviceInfoModel
    {
        public List<RemovableDeviceModel> DriveInfoList { get; }
        public List<MediaDeviceModel> MediaDeviceList { get; }
        //public MediaDeviceModel SelectedMediaDevice { get; }

        public CollectiveDeviceInfoModel(List<RemovableDeviceModel> driveInfoList, List<MediaDeviceModel> mediaDeviceList)
        {
            DriveInfoList = driveInfoList;
            MediaDeviceList = mediaDeviceList;
            //SelectedMediaDevice = selectedMediaDevice;
        }
    }
}
