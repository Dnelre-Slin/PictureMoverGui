using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PictureMoverGui.Store
{
    public class EventDataStore
    {
        public event Action<EventDataStore> EventDataChanged;

        private List<EventDataModel> _eventDataList;
        public IEnumerable<int> EventDataKeys => Enumerable.Range(0, _eventDataList.Count);
        public IEnumerable<EventDataModel> EventDataValues => _eventDataList;

        public EventDataStore()
        {
            _eventDataList = new List<EventDataModel>();
            // Todo : Read from datastore
        }

        public void CreateEventData(EventDataModel eventData)
        {
            _eventDataList.Add(eventData);
            EventDataChanged?.Invoke(this);
        }

        public EventDataModel ReadEventData(int key)
        {
            return _eventDataList[key];
        }

        public void UpdateEventData(int key, EventDataModel eventData)
        {
            if (key >= 0 && key < _eventDataList.Count)
            {
                _eventDataList[key] = eventData;
                EventDataChanged?.Invoke(this);
            }
        }

        public void DeleteEventData(int key)
        {
            if (key >= 0 && key < _eventDataList.Count)
            {
                _eventDataList.RemoveAt(key);
                EventDataChanged?.Invoke(this);
            }
        }
    }
}
