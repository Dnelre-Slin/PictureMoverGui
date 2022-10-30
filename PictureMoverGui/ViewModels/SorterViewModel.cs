using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Store;
using PictureMoverGui.SubViewModels;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class SorterViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public DirectorySelectorViewModel SourceDirectorySelector { get; }
        public DirectorySelectorLiteViewModel DestinationDirectorySelector { get; }
        public SorterInterfaceViewModel SorterInterface { get; }

        public bool AllowSwap => _masterStore.RunningStore.RunState == RunStates.Idle;

        public ICommand SwapSourceAndDestination { get; }

        public SorterViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            SourceDirectorySelector = new DirectorySelectorViewModel(masterStore);
            DestinationDirectorySelector = new DirectorySelectorLiteViewModel(masterStore);
            SorterInterface = new SorterInterfaceViewModel(masterStore);

            SwapSourceAndDestination = new CallbackCommand(OnSwapSourceAndDestination);

            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;

            SourceDirectorySelector.Dispose();
            DestinationDirectorySelector.Dispose();
            SorterInterface.Dispose();
        }

        protected void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(AllowSwap));
        }

        public void OnSwapSourceAndDestination(object parameter)
        {
            if (AllowSwap)
            {
                string newSourcePath = DestinationDirectorySelector.DestinationPath;
                DestinationDirectorySelector.DestinationPath = SourceDirectorySelector.SourcePath;
                SourceDirectorySelector.SourcePath = newSourcePath;
            }
        }
    }
}
