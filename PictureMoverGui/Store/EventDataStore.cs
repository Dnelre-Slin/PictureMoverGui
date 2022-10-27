using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PictureMoverGui.Store
{
    public class EventDataStore
    {
        public event Action<EventDataStore> EventDataStoreChanged;
        public event Action<Guid, EventDataModel> EventDataAdded;
        public event Action<Guid, EventDataModel> EventDataUpdated;
        public event Action<Guid, EventDataModel> EventDataRemoved;

        private Dictionary<Guid, EventDataModel> _eventDataList;
        public IEnumerable<Guid> EventDataKeys => _eventDataList.Keys;
        public IEnumerable<EventDataModel> EventDataValues => _eventDataList.Values;

        public EventDataStore()
        {
            _eventDataList = new Dictionary<Guid, EventDataModel>();
            // Todo : Read from datastore
        }

        public void CreateEventData(EventDataModel eventData)
        {
            Guid key = Guid.NewGuid();
            _eventDataList.Add(key, eventData);
            EventDataStoreChanged?.Invoke(this);
            EventDataAdded?.Invoke(key, eventData);
        }

        public EventDataModel ReadEventData(Guid key)
        {
            return _eventDataList[key];
        }

        public void UpdateEventData(Guid key, EventDataModel eventData)
        {
            if (_eventDataList.ContainsKey(key))
            {
                _eventDataList[key] = eventData;
                EventDataStoreChanged?.Invoke(this);
                EventDataUpdated?.Invoke(key, eventData);
            }
        }

        public void DeleteEventData(Guid key)
        {
            if (_eventDataList.ContainsKey(key))
            {
                EventDataModel eventData = _eventDataList[key];
                _eventDataList.Remove(key);
                EventDataStoreChanged?.Invoke(this);
                EventDataRemoved(key, eventData);
            }
        }
    }
}
