using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.ViewModels
{
    public class SorterViewModel : ViewModelBase
    {
        MasterStore _masterStore;

        public DirectorySelectorViewModel DirectorySelector { get; }

        public SorterViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            DirectorySelector = new DirectorySelectorViewModel(masterStore);
        }
    }
}
