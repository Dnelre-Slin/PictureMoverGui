using System;

namespace PictureMoverGui.Models
{
    public class EventDataModel
    {
        public string Name { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public EventDataModel(string name, DateTime startTime, DateTime endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
