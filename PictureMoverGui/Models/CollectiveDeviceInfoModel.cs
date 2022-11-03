using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class CollectiveDeviceInfoModel
    {
        public List<DriveInfoModel> DriveInfoList { get; }
        public List<MediaDeviceModel> MediaDeviceList { get; }
        public MediaDeviceModel SelectedMediaDevice { get; }

        public CollectiveDeviceInfoModel(List<DriveInfoModel> driveInfoList, List<MediaDeviceModel> mediaDeviceList, MediaDeviceModel selectedMediaDevice)
        {
            DriveInfoList = driveInfoList;
            MediaDeviceList = mediaDeviceList;
            SelectedMediaDevice = selectedMediaDevice;
        }
    }
}
