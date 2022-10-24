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

        public string Name => _masterStore.Dummy.Name;
        public string Description => _masterStore.Dummy.Description;

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
            get => _masterStore.Dummy.Active;
            set {
                if (_masterStore.Dummy.Active != value)
                {
                    _masterStore.Dummy.Active = value; 
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

        public ListShowerViewModel ListShowerVM { get; }

        public ICommand ChangeName { get; }
        //public ICommand CheckBoxChange { get; }

        public MainWindowViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;
            _masterStore.Dummy.DummyChanged += DummyChanged;

            _selectedIndex = 0;

            ListShowerVM = new ListShowerViewModel(masterStore);

            ChangeName = new CallbackCommand(OnChangeName);
            //CheckBoxChange = new CallbackCommand(_masterStore, CheckBoxChanged);
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.Dummy.DummyChanged -= DummyChanged;
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
            _masterStore.Dummy.Name = TypeText;
            TypeText = "";

            Debug.WriteLine($"Command executed. Name : {_masterStore.Dummy.Name} , Desc : {_masterStore.Dummy.Description} , Active : {_masterStore.Dummy.Active}");

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
