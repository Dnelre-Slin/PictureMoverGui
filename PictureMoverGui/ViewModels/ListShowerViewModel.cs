using PictureMoverGui.Commands;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class ListShowerViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private ObservableCollection<ListShowerElementViewModel> _fileDatas;
        public IEnumerable<ListShowerElementViewModel> FileDatas => _fileDatas;

        public int NumberOfFiles
        {
            get
            {
                int count = 0;
                foreach (var fileData in FileDatas)
                {
                    count += fileData.Count;
                }
                return count;
            }
        }
        public int ActiveFiles
        {
            get
            {
                int count = 0;
                foreach (var fileData in FileDatas)
                {
                    if (fileData.Active)
                    {
                        count += fileData.Count;
                    }
                }
                return count;
            }
        }

        public ICommand FalsifyList { get; }
        public ICommand ActiveChanged { get; }

        public ListShowerViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            _masterStore.FileData.FileDataChanged += FileDataStore_FileDataChanged;

            FalsifyList = new CallbackCommand(OnFalsifyList);
            ActiveChanged = new CallbackCommand(OnActiveChanged);

            _fileDatas = new ObservableCollection<ListShowerElementViewModel>();
            //foreach (var fileData in _masterStore.FileData.FileDatas)
            //{
            //    _fileDatas.Add(new ListShowerElementViewModel(_masterStore, fileData));
            //}
            GetFileDatasFromStore();

        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.FileData.FileDataChanged -= FileDataStore_FileDataChanged;
        }

        protected void FileDataStore_FileDataChanged()
        {
            Debug.WriteLine("FileDataStore_FileDataChanged");
            OnPropertyChanged(nameof(FileDatas));
            OnPropertyChanged(nameof(NumberOfFiles));
            OnPropertyChanged(nameof(ActiveFiles));
        }

        protected void GetFileDatasFromStore()
        {
            foreach (var fileDataVM in FileDatas)
            {
                fileDataVM?.Dispose();
            }
            _fileDatas.Clear();

            foreach (var fileData in _masterStore.FileData.FileDatas)
            {
                _fileDatas.Add(new ListShowerElementViewModel(_masterStore, fileData));
            }
        }

        protected void OnFalsifyList(object parameter)
        {
            Debug.WriteLine("OnFalsifyList");

            foreach (var item in FileDatas)
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
