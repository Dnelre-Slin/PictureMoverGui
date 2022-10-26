using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class ListShowerElementViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        //private FileData _fileData;
        //private int _index;
        private string _key;

        private FileData FileData => _masterStore.FileDataStore.GetFileData(_key);

        public string Name => FileData.Name;
        public int Count => FileData.Count;

        public bool Active
        {
            get { return FileData.Active; }
            set
            {
                if (FileData.Active != value)
                {
                    _masterStore.FileDataStore.SetActive(_key, value);
                    OnPropertyChanged(nameof(Active));
                }
            }
        }

        public ListShowerElementViewModel(MasterStore masterStore, string key)
        {
            _masterStore = masterStore;
            //_fileData = fileData;
            //_index = index;
            _key = key;
        }
    }
}
