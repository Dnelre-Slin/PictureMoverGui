using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class DriveInfoModel
    {
        public string DriveId { get; }
        public string SerialId { get; }

        public DriveInfoModel(string driveId, string serialId)
        {
            DriveId = driveId;
            SerialId = serialId;
        }
    }
}
