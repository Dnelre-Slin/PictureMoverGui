using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.ViewModels
{
    public class EventListElementViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public EventListElementViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
        }
    }
}
