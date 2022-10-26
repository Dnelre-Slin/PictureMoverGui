﻿using PictureMoverGui.Models;
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
                    //OnPropertyChanged(nameof(Active));
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

        public void RefreshActive()
        {
            OnPropertyChanged(nameof(Active));
        }
    }
}
