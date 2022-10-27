using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PictureMoverGui.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public EventListViewModel EventList{ get; }
        public EventEditViewModel EventEdit{ get; }

        public Visibility ListVisiblity => Visibility.Collapsed;
        public Visibility EditVisiblity => Visibility.Visible;

        public EventsViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            EventList = new EventListViewModel(masterStore);
            EventEdit = new EventEditViewModel(masterStore, null);
        }
    }
}
