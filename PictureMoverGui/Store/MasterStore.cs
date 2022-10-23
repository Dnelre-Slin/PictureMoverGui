using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class MasterStore
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            {
                if (_name != value)
                {
                    _name = value;
                    MasterPropertyChanged?.Invoke();
                }
            }
        }
        private string _description;
        public string Description 
        { 
            get { return _description;}
            set
            {
                if (_description != value)
                {
                    _description = value; 
                    MasterPropertyChanged?.Invoke();
                }
            }
        }
        //private string _typeText;
        //public string TypeText
        //{ 
        //    get { return _typeText;}
        //    set
        //    {
        //        if (_typeText != value)
        //        {
        //            _typeText = value;
        //            MasterPropertyChanged?.Invoke();
        //        }
        //    }
        //}
        private bool _active;
        public bool Active
        {
            get { return _active;}
            set
            {
                if (_active != value)
                {
                    _active = value;
                    MasterPropertyChanged?.Invoke();
                }
            }
        }

        public event Action MasterPropertyChanged;

        public MasterStore(string name, string description, string typeText, bool active)
        {
            _name = name;
            _description = description;
            //_typeText = typeText;
            _active = active;
        }
    }
}
