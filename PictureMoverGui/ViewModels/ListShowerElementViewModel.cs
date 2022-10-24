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

        private FileData _fileData;

        public string Name => _fileData.Name;
        public int Count => _fileData.Count;

        public bool Active
        {
            get { return _fileData.Active; }
            set
            {
                if (_fileData.Active != value)
                {
                    _masterStore.FileData.SetActive(_fileData, value);
                    OnPropertyChanged(nameof(Active));
                }
            }
        }

        public ListShowerElementViewModel(MasterStore masterStore, FileData fileData)
        {
            _masterStore = masterStore;
            _fileData = fileData;
        }
    }
}
