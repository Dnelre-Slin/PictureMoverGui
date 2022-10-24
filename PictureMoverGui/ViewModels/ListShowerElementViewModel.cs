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

        private int _index;
        private FileData _fileData;
        private ICommand _activeChanged;

        public string Name => _fileData.Name;
        public int Count => _fileData.Count;

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    OnPropertyChanged(nameof(Active));
                    _activeChanged?.Execute(this);
                    //_masterStore.FileData.SetActive(_index, value);
                }
            }
        }

        public ListShowerElementViewModel(MasterStore masterStore, FileData fileData, ICommand activeChanged)
        {
            _masterStore = masterStore;
            _fileData = fileData;
            _index = _fileData.Index;
            _activeChanged = activeChanged;
            //_active = _fileData.Active;
        }
    }
}
