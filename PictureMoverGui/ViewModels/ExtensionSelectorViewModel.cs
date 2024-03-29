﻿using PictureMoverGui.Commands;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.SubViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class ExtensionSelectorViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private ObservableCollection<ExtensionSelectorElementViewModel> _fileExtensionList;
        public IEnumerable<ExtensionSelectorElementViewModel> FileExtensionList => _fileExtensionList;

        public int NumberOfFiles
        {
            get
            {
                int count = 0;
                foreach (var fileExtension in FileExtensionList)
                {
                    count += fileExtension.Count;
                }
                return count;
            }
        }
        public int ActiveFiles
        {
            get
            {
                int count = 0;
                foreach (var fileExtension in FileExtensionList)
                {
                    if (fileExtension.Active)
                    {
                        count += fileExtension.Count;
                    }
                }
                return count;
            }
        }

        public bool AllowEdit => _masterStore.RunningStore.RunState == RunStates.Idle;

        public ICommand FalsifyList { get; }
        public ICommand ActiveChanged { get; }

        public ExtensionSelectorViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            _masterStore.FileExtensionStore.FileExtensionChanged += FileExtensionStore_FileExtensionChanged;
            _masterStore.FileExtensionStore.FileExtensionDictReset += FileExtensionStore_FileExtensionDictReset;
            _masterStore.RunningStore.RunningStoreChanged += RunningStore_RunningStoreChanged;

            FalsifyList = new CallbackCommand(OnFalsifyList);
            ActiveChanged = new CallbackCommand(OnActiveChanged);

            _fileExtensionList = new ObservableCollection<ExtensionSelectorElementViewModel>();
 
            ResetFileExtensionsFromStore();

        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.FileExtensionStore.FileExtensionChanged -= FileExtensionStore_FileExtensionChanged;
            _masterStore.FileExtensionStore.FileExtensionDictReset -= FileExtensionStore_FileExtensionDictReset;
            _masterStore.RunningStore.RunningStoreChanged -= RunningStore_RunningStoreChanged;
            ClearFileExtensionList();
        }

        protected void ClearFileExtensionList()
        {
            foreach (var fileExtension in FileExtensionList)
            {
                fileExtension?.Dispose();
            }
            _fileExtensionList.Clear();
        }

        protected void FileExtensionStore_FileExtensionChanged(FileExtensionModel fileExtensionModel)
        {
            Debug.WriteLine("FileExtensionStore_FileExtensionChanged");
            foreach (var fileExtension in FileExtensionList)
            {
                fileExtension?.RefreshActive();
            }
            OnPropertyChanged(nameof(FileExtensionList));
            OnPropertyChanged(nameof(NumberOfFiles));
            OnPropertyChanged(nameof(ActiveFiles));
        }
        
        protected void FileExtensionStore_FileExtensionDictReset(IEnumerable<FileExtensionModel> fileExtensionDict)
        {
            Debug.WriteLine("FileExtensionStore_FileExtensionReset");
            ResetFileExtensionsFromStore();
        }

        protected void RunningStore_RunningStoreChanged(RunningStore runningStore)
        {
            OnPropertyChanged(nameof(AllowEdit));
        }

        protected void ResetFileExtensionsFromStore()
        {
            ClearFileExtensionList();
            foreach (var fileExtensionKey in _masterStore.FileExtensionStore.FileExtensionKeys)
            {
                _fileExtensionList.Add(new ExtensionSelectorElementViewModel(_masterStore, fileExtensionKey));
            }
            OnPropertyChanged(nameof(FileExtensionList));
            OnPropertyChanged(nameof(NumberOfFiles));
            OnPropertyChanged(nameof(ActiveFiles));
        }

        protected void OnFalsifyList(object parameter)
        {
            Debug.WriteLine("OnFalsifyList");

            foreach (var item in FileExtensionList)
            {
                item.Active = false;
            }
        }

        protected void OnActiveChanged(object parameter)
        {
            Debug.WriteLine("OnActiveChanged");
            OnPropertyChanged(nameof(NumberOfFiles));
            OnPropertyChanged(nameof(ActiveFiles));
        }
    }
}
