using MediaDevices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class MediaDeviceModel
    {
        public string FriendlyName { get; }
        public string SerialId { get; }
        public MediaDevice MediaDevice { get; }

        public MediaDeviceModel(string friendlyName, string serialId, MediaDevice mediaDevice)
        {
            FriendlyName = friendlyName;
            SerialId = serialId;
            MediaDevice = mediaDevice;
        }
    }
}
