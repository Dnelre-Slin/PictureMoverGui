using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class AdvancedOptionsViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public NameCollisionActionEnum NameCollisionOption { get; set; }
        public CompareFilesActionEnum CompareFilesOption { get; set; }
        public HashTypeEnum HashTypeOption { get; set; }
        public MediaTypeEnum SorterMediaTypeOption { get; set; }

        public ICommand ResetSettings { get; }
        public ICommand TestButton { get; }

        public AdvancedOptionsViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            ResetSettings = new CallbackCommand(OnResetSettings);
            TestButton = new CallbackCommand(OnTestButton);
        }

        protected void OnResetSettings(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnResetSettings");
        }

        protected void OnTestButton(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnTestButton");
        }
    }
}
