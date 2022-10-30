using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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
            LoadFromFile();
        }

        public void CreateEventData(EventDataModel eventData)
        {
            Guid key = Guid.NewGuid();
            _eventDataList.Add(key, eventData);
            SaveToFile();
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
                SaveToFile();
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
                SaveToFile();
                EventDataStoreChanged?.Invoke(this);
                EventDataRemoved(key, eventData);
            }
        }

        protected void SaveToFile()
        {
            StringCollection stringCollection = new StringCollection();
            
            foreach (EventDataModel eventData in EventDataValues)
            {
                stringCollection.Add(eventData.Name);
                stringCollection.Add(eventData.StartTime.ToString());
                stringCollection.Add(eventData.EndTime.ToString());
            }

            Properties.Datastore.Default.EventDataList = stringCollection;
            Properties.Datastore.Default.Save();
        }

        protected void LoadFromFile()
        {
            StringCollection stringCollection = Properties.Datastore.Default.EventDataList;

            if (stringCollection.Count % 3 != 0)
            {
                throw new ArrayTypeMismatchException("EventDataList should be stored in 3s. The current eventDataList has not been, so it cannot be loaded");
            }

            // Step through 3 at a time, recreating event data from string list
            for (int i = 0; i < stringCollection.Count; i += 3)
            {
                CreateEventData(new EventDataModel(
                    stringCollection[i],
                    DateTime.Parse(stringCollection[i+1].ToString()),
                    DateTime.Parse(stringCollection[i+2].ToString())));
            }
        }
    }
}
