using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class RemovableDeviceModel
    {
        public string Name { get; }
        public string SerialId { get; }
        public string Path { get; }
        public bool IsConnected { get; }

        public RemovableDeviceModel(string name, string serialId, string path, bool isConnected)
        {
            Name = name;
            SerialId = serialId;
            Path = path;
            IsConnected = isConnected;
        }
    }
}
