using PictureMoverGui.Commands;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public string Name => _masterStore.Name;
        public string Description => _masterStore.Description;

        private string _typeText;
        public string TypeText
        {
            get { return _typeText; }
            set 
            { 
                _typeText = value;
                OnPropertyChanged(nameof(TypeText));
            }
        }

        //private bool _active;
        public bool Active 
        {
            get => _masterStore.Active;
            set {
                if (_masterStore.Active != value)
                {
                    _masterStore.Active = value; 
                    OnPropertyChanged(nameof(Active));
                }
            }
        }

        public ICommand ChangeName { get; }
        public ICommand CheckBoxChange { get; }

        public MainWindowViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _masterStore.MasterPropertyChanged += MasterStoreChanged;

            ChangeName = new CallbackCommand(_masterStore, ButtonClicked);
            CheckBoxChange = new CallbackCommand(_masterStore, CheckBoxChanged);
        }

        protected override void Dispose()
        {
            base.Dispose();

            _masterStore.MasterPropertyChanged -= MasterStoreChanged;
        }

        protected void MasterStoreChanged()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
            //OnPropertyChanged(nameof(TypeText));
            OnPropertyChanged(nameof(Active));
        }

        protected void ButtonClicked()
        {
            _masterStore.Name = TypeText;
            TypeText = "";
        }

        protected void CheckBoxChanged()
        {
            //_masterStore.Active = Active;
        }
    }
}
