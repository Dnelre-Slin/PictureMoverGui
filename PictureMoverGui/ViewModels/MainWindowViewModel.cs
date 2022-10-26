using PictureMoverGui.Commands;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public SorterViewModel Sorter { get; }
        public ExtensionSelectorViewModel ExtensionSelector { get; }
        public AdvancedOptionsViewModel AdvancedOptions { get; }

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
            ExtensionSelector = new ExtensionSelectorViewModel(masterStore);
            AdvancedOptions = new AdvancedOptionsViewModel(masterStore);
        }

        public override void Dispose()
        {
            base.Dispose();

            Sorter.Dispose();
            ExtensionSelector.Dispose();
            AdvancedOptions.Dispose();
        }

        protected void OnSelectedIndexChanged()
        {
            Debug.WriteLine($"Tab changed : {SelectedTabIndex}");
        }
    }
}
