using MediaDevices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class MediaDeviceModel
    {
        public string Name { get; }
        public string SerialId { get; }
        public DateTime LastRun { get; }
        public MediaDevice MediaDevice { get; }

        public MediaDeviceModel(string name, string serialId, DateTime lastRun, MediaDevice mediaDevice)
        {
            Name = name;
            SerialId = serialId;
            LastRun = lastRun;
            MediaDevice = mediaDevice;
        }
    }
}
