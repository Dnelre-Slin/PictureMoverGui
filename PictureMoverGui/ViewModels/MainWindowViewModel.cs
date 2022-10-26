using PictureMoverGui.Commands;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public ExtensionSelectorViewModel ExtensionSelector { get; }
        public SorterViewModel Sorter { get; }

        public string Name => _masterStore.DummyStore.Name;
        public string Description => _masterStore.DummyStore.Description;

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

        public bool Active 
        {
            get => _masterStore.DummyStore.Active;
            set {
                if (_masterStore.DummyStore.Active != value)
                {
                    _masterStore.DummyStore.Active = value; 
                    OnPropertyChanged(nameof(Active));
                    OnActiveChanged();
                }
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged(nameof(SelectedIndex));
                    OnSelectedIndexChanged();
                }
            }
        }


        public ICommand ChangeName { get; }
        //public ICommand CheckBoxChange { get; }

        public MainWindowViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _masterStore.DummyStore.DummyChanged += DummyChanged;

            _selectedIndex = 0;

            ExtensionSelector = new ExtensionSelectorViewModel(masterStore);
            Sorter = new SorterViewModel(masterStore);

            ChangeName = new CallbackCommand(OnChangeName);
            //CheckBoxChange = new CallbackCommand(_masterStore, CheckBoxChanged);
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.DummyStore.DummyChanged -= DummyChanged;
        }

        protected void DummyChanged()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
            //OnPropertyChanged(nameof(TypeText));
            OnPropertyChanged(nameof(Active));
        }

        protected IEnumerable<string> GetNames()
        {
            List<string> ls = new List<string>();
            ls.Add("hello");
            ls.Add("sir");
            ls.Add("good");
            ls.Add("lady");
            ls.Add("person");

            foreach (var s in ls)
            {
                yield return s;
            }
        }

        protected void OnChangeName(object parameter)
        {
            _masterStore.DummyStore.Name = TypeText;
            TypeText = "";

            Debug.WriteLine($"Command executed. Name : {_masterStore.DummyStore.Name} , Desc : {_masterStore.DummyStore.Description} , Active : {_masterStore.DummyStore.Active}");

            //foreach (var item in ListShowerVM.FileDatas)
            //{
            //    Debug.WriteLine($"{item.Name} : {item.Count} : {item.Active}");
            //}
        }

        protected void OnActiveChanged()
        {
            Debug.WriteLine($"CheckBox changed : {Active}");
        }

        protected void OnSelectedIndexChanged()
        {
            Debug.WriteLine($"Tab changed : {SelectedIndex}");
        }

        //protected void CheckBoxChanged()
        //{
        //    //_masterStore.Active = Active;
        //}
    }
}
