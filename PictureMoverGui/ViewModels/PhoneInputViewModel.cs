using PictureMoverGui.Store;
using PictureMoverGui.SubViewModels;
using System.Windows;

namespace PictureMoverGui.ViewModels
{
    public class PhoneInputViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public MediaDeviceSelectorViewModel MediaDeviceSelector { get; }
        public RemovableDeviceSelectorViewModel RemovableDeviceSelector { get; }
        public DirectorySelectorLiteViewModel DestinationDirectorySelector { get; }
        public SorterInterfaceViewModel SorterInterface { get; }

        public PhoneInputViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            MediaDeviceSelector = new MediaDeviceSelectorViewModel(masterStore);
            RemovableDeviceSelector = new RemovableDeviceSelectorViewModel(masterStore);
            DestinationDirectorySelector = new DirectorySelectorLiteViewModel(masterStore);
            SorterInterface = new SorterInterfaceViewModel(masterStore);

            SorterInterface.CopyVisibility = Visibility.Collapsed;
        }

        public override void Dispose()
        {
            base.Dispose();

            MediaDeviceSelector.Dispose();
            RemovableDeviceSelector.Dispose();
            DestinationDirectorySelector.Dispose();
            SorterInterface.Dispose();
        }
    }
}
