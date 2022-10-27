using PictureMoverGui.Commands;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class EventListViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private ObservableCollection<EventListElementViewModel> _eventDataList;
        public IEnumerable<EventListElementViewModel> EventDataList => _eventDataList;

        public Visibility ErrorVisibility => Visibility.Collapsed;
        public Visibility NonErrorVisibility => Visibility.Visible;

        public ICommand CreateEvent { get; }

        public EventListViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            CreateEvent = new CallbackCommand(OnCreateEvent);
        }

        protected void OnCreateEvent(object parameter)
        {
            //
        }
    }
}
