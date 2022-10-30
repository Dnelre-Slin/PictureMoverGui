using PictureMoverGui.Commands;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.SubViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private enum EventState
        {
            Viewing,
            Creating,
            Editing
        }

        private MasterStore _masterStore;

        public EventListViewModel EventList{ get; }
        public EventEditViewModel EventEdit{ get; }

        private EventState _eventState;
        private Guid _editKey;

        public Visibility ListVisiblity => _eventState == EventState.Viewing ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EditVisiblity => _eventState != EventState.Viewing ? Visibility.Visible : Visibility.Collapsed;

        public ICommand CreateEvent { get; }
        public ICommand EditDone { get; }

        public EventsViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            EventList = new EventListViewModel(masterStore, OnEditEvent);
            EventEdit = new EventEditViewModel(masterStore);

            CreateEvent = new CallbackCommand(OnCreateEvent);
            EditDone = new CallbackCommand(OnEditDone);

            _eventState = EventState.Viewing;
            _editKey = Guid.Empty;
        }

        public override void Dispose()
        {
            base.Dispose();

            EventList.Dispose();
            EventEdit.Dispose();
        }

        protected void OnCreateEvent(object parameter)
        {
            _eventState = EventState.Creating;
            _editKey = Guid.Empty;
            EventEdit.SetEventData(App.Current.FindResource("EventEditNameNewEvent").ToString(), DateTime.Now, DateTime.Now);
            OnPropertyChanged(nameof(ListVisiblity));
            OnPropertyChanged(nameof(EditVisiblity));
        }

        protected void OnEditEvent(Guid key, EventDataModel eventDataModel)
        {
            _eventState = EventState.Editing;
            _editKey = key;
            EventEdit.SetEventData(eventDataModel.Name, eventDataModel.StartTime, eventDataModel.EndTime);
            OnPropertyChanged(nameof(ListVisiblity));
            OnPropertyChanged(nameof(EditVisiblity));
        }

        protected void OnEditDone(object parameter)
        {
            System.Diagnostics.Debug.WriteLine(EventEdit.EventName);
            System.Diagnostics.Debug.WriteLine(EventEdit.StartDateTime);
            System.Diagnostics.Debug.WriteLine(EventEdit.EndDateTime);
            if (_eventState == EventState.Creating)
            {
                _masterStore.EventDataStore.CreateEventData(new EventDataModel(EventEdit.EventName, EventEdit.StartDateTime, EventEdit.EndDateTime));
            }
            else if (_eventState == EventState.Editing)
            {
                _masterStore.EventDataStore.UpdateEventData(_editKey, new EventDataModel(EventEdit.EventName, EventEdit.StartDateTime, EventEdit.EndDateTime));
            }
            _editKey = Guid.Empty;
            _eventState = EventState.Viewing;
            OnPropertyChanged(nameof(ListVisiblity));
            OnPropertyChanged(nameof(EditVisiblity));
        }
    }
}
