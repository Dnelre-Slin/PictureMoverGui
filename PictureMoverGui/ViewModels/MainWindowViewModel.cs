using PictureMoverGui.Store;
using System.Diagnostics;

namespace PictureMoverGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public SorterViewModel Sorter { get; }
        public PhoneInputViewModel PhoneInput { get; }
        public EventsViewModel Events { get; }
        public ExtensionSelectorViewModel ExtensionSelector { get; }
        public AdvancedOptionsViewModel AdvancedOptions { get; }
        public StatusInfoViewModel StatusInfo { get; }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged(nameof(SelectedTabIndex));
                    OnSelectedIndexChanged();
                }
            }
        }

        public MainWindowViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            _selectedTabIndex = 0;

            Sorter = new SorterViewModel(masterStore);
            PhoneInput = new PhoneInputViewModel(masterStore);
            Events = new EventsViewModel(masterStore);
            ExtensionSelector = new ExtensionSelectorViewModel(masterStore);
            AdvancedOptions = new AdvancedOptionsViewModel(masterStore);
            StatusInfo = new StatusInfoViewModel(masterStore);
        }

        public override void Dispose()
        {
            base.Dispose();

            Debug.WriteLine("Disposing");

            Sorter.Dispose();
            PhoneInput.Dispose();
            Events.Dispose();
            ExtensionSelector.Dispose();
            AdvancedOptions.Dispose();
            StatusInfo.Dispose();
        }

        protected void OnSelectedIndexChanged()
        {
            Debug.WriteLine($"Tab changed : {SelectedTabIndex}");
        }
    }
}
