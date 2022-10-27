using PictureMoverGui.Commands;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class EventListElementViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private Guid _key;
        public Guid Key => _key;
        private Action<Guid, EventDataModel> _onEditCallback;
        private EventDataModel EventData => _masterStore.EventDataStore.ReadEventData(_key);

        public string EventName => EventData.Name;
        public string EventStartTime => EventData.StartTime.ToString();
        public string EventEndTime => EventData.EndTime.ToString();

        public Visibility ErrorVisibility => EventData.StartTime <= EventData.EndTime ? Visibility.Collapsed : Visibility.Visible;
        public Visibility NonErrorVisibility => EventData.StartTime <= EventData.EndTime ? Visibility.Visible : Visibility.Collapsed;

        public ICommand EditEvent { get; }
        public ICommand DeleteEvent { get; }

        public EventListElementViewModel(MasterStore masterStore, Guid key, Action<Guid, EventDataModel> onEditCallback)
        {
            _masterStore = masterStore;

            _key = key;
            _onEditCallback = onEditCallback;

            EditEvent = new CallbackCommand(OnEditEvent);
            DeleteEvent = new CallbackCommand(OnDeleteEvent);
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(EventName));
            OnPropertyChanged(nameof(EventStartTime));
            OnPropertyChanged(nameof(EventEndTime));
            OnPropertyChanged(nameof(ErrorVisibility));
            OnPropertyChanged(nameof(NonErrorVisibility));
        }

        protected void OnEditEvent(object parameter)
        {
            _onEditCallback(_key, EventData);
        }

        protected void OnDeleteEvent(object parameter)
        {
            MessageBoxResult result = MessageBox.Show($"{App.Current.FindResource("MessageBoxDeleteEventText")} \"{EventName}\"?", $"{App.Current.FindResource("MessageBoxDeleteEventTitle")} \"{EventName}\"", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _masterStore.EventDataStore.DeleteEventData(_key);
            }
        }
    }
}
