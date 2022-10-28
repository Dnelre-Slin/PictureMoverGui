using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class AdvancedOptionsViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private SorterConfigurationModel SorterConfig => _masterStore.SorterConfigurationStore.SorterConfiguration;

        public NameCollisionActionEnum NameCollisionOption
        {
            get { return SorterConfig.NameCollisionAction; }
            set
            {
                if (SorterConfig.NameCollisionAction != value)
                {
                    _masterStore.SorterConfigurationStore.SetNameCollisionAction(value);
                }
            }
        }
        public CompareFilesActionEnum CompareFilesOption
        {
            get { return SorterConfig.CompareFilesAction; }
            set
            {
                if (SorterConfig.CompareFilesAction != value)
                {
                    _masterStore.SorterConfigurationStore.SetCompareFilesAction(value);
                }
            }
        }
        public HashTypeEnum HashTypeOption
        {
            get { return SorterConfig.HashType; }
            set
            {
                if (SorterConfig.HashType != value)
                {
                    _masterStore.SorterConfigurationStore.SetHashType(value);
                }
            }
        }
        public MediaTypeEnum SorterMediaTypeOption
        {
            get { return SorterConfig.MediaType; }
            set
            {
                if (SorterConfig.MediaType != value)
                {
                    _masterStore.SorterConfigurationStore.SetMediaType(value);
                }
            }
        }

        public bool AllowEdit => _masterStore.RunningStore.RunState == RunStates.Idle;
        public bool AllowEditCompareFiles => SorterConfig.NameCollisionAction == NameCollisionActionEnum.CompareFiles;
        public bool AllowEditHashType
        {
            get
            {
                return (AllowEditCompareFiles &&
                        (SorterConfig.CompareFilesAction == CompareFilesActionEnum.NameAndHashOnly ||
                        SorterConfig.CompareFilesAction == CompareFilesActionEnum.NameDateAndHash));
            }
        }

        public ICommand ResetSettings { get; }
        public ICommand TestButton { get; }

        public AdvancedOptionsViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfigurationStore_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            ResetSettings = new CallbackCommand(OnResetSettings);
            TestButton = new CallbackCommand(OnTestButton);
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.SorterConfigurationStore.SorterConfigurationChanged -= SorterConfigurationStore_SorterConfigurationChanged;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
        }

        protected void SorterConfigurationStore_SorterConfigurationChanged(SorterConfigurationModel sorterConfigurationModel)
        {
            OnPropertyChanged(nameof(NameCollisionOption));
            OnPropertyChanged(nameof(CompareFilesOption));
            OnPropertyChanged(nameof(HashTypeOption));
            OnPropertyChanged(nameof(SorterMediaTypeOption));
            OnPropertyChanged(nameof(AllowEditCompareFiles));
            OnPropertyChanged(nameof(AllowEditHashType));
        }

        protected void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(AllowEdit));
        }

        protected void OnResetSettings(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnResetSettings");
            MessageBoxResult result = MessageBox.Show($"{App.Current.FindResource("MessageBoxResetSettingsText")}", $"{App.Current.FindResource("MessageBoxResetSettingsTitle")}", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _masterStore.SorterConfigurationStore.ResetToDefaultSettings();
                //Properties.Settings.Default.Reset();
                //this.moverModel.SettingsRefresh();
                //Properties.Datastore.Default.EventList = Simplifiers.EventListToSimpleList(this.moverModel.eventDataList);
                //Properties.Settings.Default.Save();
            }
        }

        protected void OnTestButton(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnTestButton");
            foreach (var ext in _masterStore.FileExtensionStore.FileExtensionValues)
            {
                System.Diagnostics.Debug.WriteLine(ext.Name + " : " + ext.Active);
            }
        }
    }
}
