using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PictureMoverGui.Helpers
{
    [Serializable]
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
        public static List<SimpleEventData> EventListToSimpleListValidOnly(ObservableCollection<EventData> eventList)
        {
            List<SimpleEventData> simpleList = new List<SimpleEventData>(eventList.Count);
            foreach (var eventData in eventList)
            {
                if (eventData.ValidDateOrder)
                {
                    simpleList.Add(new SimpleEventData(eventData.Name, eventData.StartDateTime.ToDateTime().Ticks, eventData.EndDateTime.ToDateTime().Ticks));
                }
            }
            return simpleList;
        }

        public static List<SimpleEventData> EventListToSimpleList(ObservableCollection<EventData> eventList)
        {
            List<SimpleEventData> simpleList = new List<SimpleEventData>(eventList.Count);
            foreach (var eventData in eventList)
            {
                simpleList.Add(new SimpleEventData(eventData.Name, eventData.StartDateTime.ToDateTime().Ticks, eventData.EndDateTime.ToDateTime().Ticks));
            }
            return simpleList;
        }

        public static ObservableCollection<EventData> SimpleListToEventList(List<SimpleEventData> simpleList)
        {
            ObservableCollection<EventData> eventList = new ObservableCollection<EventData>();
            foreach (var simpleData in simpleList)
            {
                eventList.Add(new EventData(simpleData.Name, new EventDateTime(new DateTime(simpleData.StartTick)), new EventDateTime(new DateTime(simpleData.EndTick))));
            }
            return eventList;
        }
    }
}
