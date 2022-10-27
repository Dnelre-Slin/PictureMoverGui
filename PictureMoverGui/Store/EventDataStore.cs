using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class EventDataStore
    {
        public event Action<EventDataModel> EventDataChanged;

        private Dictionary<string, EventDataModel> _eventDataDict;
        public IEnumerable<string> EventDataKeys => _eventDataDict.Keys;
        public IEnumerable<EventDataModel> EventDataValues => _eventDataDict.Values;
        public IEnumerable<KeyValuePair<string, EventDataModel>> EventDataDict => _eventDataDict;

        public EventDataStore()
        {
            _eventDataDict = new Dictionary<string, EventDataModel>();
            // Todo : Read from datastore
        }

        public void CreateEventData(string key, EventDataModel eventData)
        {
            _eventDataDict.Add(key, eventData);
        }

        public EventDataModel ReadEventData(string key)
        {
            return _eventDataDict[key];
        }

        public void UpdateEventData(string key, EventDataModel eventData)
        {
            if (_eventDataDict.ContainsKey(key))
            {
                _eventDataDict[key] = eventData;
            }
        }

        public void DeleteEventData(string key)
        {
            if (_eventDataDict.ContainsKey(key))
            {
                _eventDataDict.Remove(key);
            }
        }
    }
}
