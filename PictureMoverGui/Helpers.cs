using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PictureMoverGui
{
    public struct SimpleEventData
    {
        public string Name;
        public long StartTick;
        public long EndTick;
        public SimpleEventData(string Name, long StartTick, long EndTick)
        {
            this.Name = Name;
            this.StartTick = StartTick;
            this.EndTick = EndTick;
        }
    }

    public class Simplifiers
    {
        public static List<SimpleEventData> ToSimpleList(ObservableCollection<EventData> eventList)
        {
            List<SimpleEventData> simpleList = new List<SimpleEventData>(eventList.Count);
            foreach (var eventData in eventList)
            {
                simpleList.Add(new SimpleEventData(eventData.Name, eventData.StartDateTime.ToDateTime().Ticks, eventData.EndDateTime.ToDateTime().Ticks));
            }
            return simpleList;
        }
    }
}
