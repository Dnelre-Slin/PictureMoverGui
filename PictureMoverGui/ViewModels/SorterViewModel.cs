using PictureMoverGui.Commands;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class SorterViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public DirectorySelectorViewModel SourceDirectorySelector { get; }
        public DirectorySelectorLiteViewModel DestinationDirectorySelector { get; }
        public SorterInterfaceViewModel SorterInterface { get; }

        public bool AllowSwap => true;

        public ICommand SwapSourceAndDestination { get; }

        public SorterViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            SourceDirectorySelector = new DirectorySelectorViewModel(masterStore);
            DestinationDirectorySelector = new DirectorySelectorLiteViewModel(masterStore);
            SorterInterface = new SorterInterfaceViewModel(masterStore);

            SwapSourceAndDestination = new CallbackCommand(OnSwapSourceAndDestination);
        }

        public override void Dispose()
        {
            base.Dispose();

            SourceDirectorySelector.Dispose();
            DestinationDirectorySelector.Dispose();
            SorterInterface.Dispose();
        }

        public void OnSwapSourceAndDestination(object parameter)
        {
            string newSourcePath = DestinationDirectorySelector.DestinationPath;
            DestinationDirectorySelector.DestinationPath = SourceDirectorySelector.SourcePath;
            SourceDirectorySelector.SourcePath = newSourcePath;
        }
    }
}
