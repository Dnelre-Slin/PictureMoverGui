using PictureMoverGui.Models;
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

        private List<FileData> _fileDatas;
        public IEnumerable<FileData> FileDatas => _fileDatas;

        public event Action MasterPropertyChanged;

        public MasterStore(string name, bool active)
        {
            _name = name;
            _description = "Change name";
            _active = active;

            _fileDatas = new List<FileData>();
            _fileDatas.Add(new FileData("jpeg", 14, true));
            _fileDatas.Add(new FileData("png", 25, true));
            _fileDatas.Add(new FileData("db", 4, false));
            _fileDatas.Add(new FileData("mp4", 9, true));
            _fileDatas.Add(new FileData("ini", 2, false));
        }
    }
}
