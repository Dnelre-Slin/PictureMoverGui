using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class ExtensionSelectorElementViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private string _key;
        private FileExtension FileExtension => _masterStore.FileExtensionStore.GetFileExtension(_key);

        public string Name => FileExtension.Name;
        public int Count => FileExtension.Count;

        public bool Active
        {
            get { return FileExtension.Active; }
            set
            {
                if (FileExtension.Active != value)
                {
                    _masterStore.FileExtensionStore.SetActive(_key, value);
                }
            }
        }

        public ExtensionSelectorElementViewModel(MasterStore masterStore, string key)
        {
            _masterStore = masterStore;
            _key = key;
        }

        public void RefreshActive()
        {
            OnPropertyChanged(nameof(Active));
        }
    }
}
