using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class SorterInterfaceViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public bool DoCopy { get; set; }

        public bool DoStructured { get; set; }

        public bool DoRename { get; set; }

        public bool AllowConfiguration => true;
        public bool AllowStartSorting => true;

        public Visibility CancelVisibility => Visibility.Visible;

        public string StatusMessage => "Ready";
        public double StatusProgressDegrees => 0 * 3.6;

        public ICommand StartSorting { get; }
        public ICommand CancelSorting { get; }

        public SorterInterfaceViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
        }
    }
}
