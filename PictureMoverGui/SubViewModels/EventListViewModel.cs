using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PictureMoverGui.SubViewModels
{
    public class EventListViewModel : ViewModelBase
    {
        private MasterStore _masterStore;
        private Action<Guid, EventDataModel> _onEditCallback;

        private ObservableCollection<EventListElementViewModel> _eventDataList;
        public IEnumerable<EventListElementViewModel> EventDataList => _eventDataList;

        public EventListViewModel(MasterStore masterStore, Action<Guid, EventDataModel> onEditCallback)
        {
            _masterStore = masterStore;
            _onEditCallback = onEditCallback;

            _eventDataList = new ObservableCollection<EventListElementViewModel>();
            ResetEventDataListFromStore();

            _masterStore.EventDataStore.EventDataAdded += EventDataStore_EventDataAdded;
            _masterStore.EventDataStore.EventDataUpdated += EventDataStore_EventDataUpdated;
            _masterStore.EventDataStore.EventDataRemoved += EventDataStore_EventDataRemoved;
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.EventDataStore.EventDataAdded -= EventDataStore_EventDataAdded;
            _masterStore.EventDataStore.EventDataUpdated -= EventDataStore_EventDataUpdated;
            _masterStore.EventDataStore.EventDataRemoved -= EventDataStore_EventDataRemoved;
        }

        protected void ResetEventDataListFromStore()
        {
            _eventDataList.Clear();
            foreach (Guid key in _masterStore.EventDataStore.EventDataKeys)
            {
                _eventDataList.Add(new EventListElementViewModel(_masterStore, key, _onEditCallback));
            }
        }

        protected void EventDataStore_EventDataAdded(Guid key, EventDataModel eventData)
        {
            _eventDataList.Add(new EventListElementViewModel(_masterStore, key, _onEditCallback));
        }

        protected void EventDataStore_EventDataUpdated(Guid key, EventDataModel eventData)
        {
            _eventDataList.Single(ed => ed.Key == key).Refresh(); ;
        }

        protected void EventDataStore_EventDataRemoved(Guid key, EventDataModel eventData)
        {
            EventListElementViewModel element = _eventDataList.Single(el => el.Key == key);
            _eventDataList.Remove(element);
        }
    }
}
