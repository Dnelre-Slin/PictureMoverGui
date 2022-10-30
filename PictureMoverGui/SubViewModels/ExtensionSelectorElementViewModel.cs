using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;

namespace PictureMoverGui.SubViewModels
{
    public class ExtensionSelectorElementViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private int _key;
        private FileExtensionModel FileExtension => _masterStore.FileExtensionStore.GetFileExtension(_key);

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

        public ExtensionSelectorElementViewModel(MasterStore masterStore, int key)
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
