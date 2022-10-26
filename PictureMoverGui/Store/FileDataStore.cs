using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class FileDataStore
    {
        private Dictionary<string, FileData> _fileDatas;
        //public IEnumerable<string, FileData> FileDatas => _fileDatas;
        public IEnumerable<string> FileDataKeys => _fileDatas.Keys;
        public IEnumerable<FileData> FileDatas => _fileDatas.Values;
        public IEnumerable<KeyValuePair<string, FileData>> FileDataKV => _fileDatas;
        //public IEnumerable<Dictionary<string, FileData>.> FileData2 => _fileDatas.;

        public event Action FileDataChanged;

        public FileDataStore()
        {
            _fileDatas = new Dictionary<string, FileData>();
            _fileDatas["jpeg"] = new FileData("jpeg", 14, true);
            _fileDatas["png"] = new FileData("png", 25, true);
            _fileDatas["db"] = new FileData("db", 4, false);
            _fileDatas["mp4"] = new FileData("mp4", 9, true);
            _fileDatas["ini"] = new FileData("ini", 2, false);
            //_fileDatas.Add(new FileData("jpeg", 14, true));
            //_fileDatas.Add(new FileData("png", 25, true));
            //_fileDatas.Add(new FileData("db", 4, false));
            //_fileDatas.Add(new FileData("mp4", 9, true));
            //_fileDatas.Add(new FileData("ini", 2, false));
        }

        public void SetActive(string key, bool state)
        {
            //FileData old = _fileDatas[index];
            //_fileDatas[index] = new FileData(old.Index, old.Name, old.Count, state);
            //int index = _fileDatas.FindIndex((f) => f.Name == fileData.Name);
            //FileData old = _fileDatas[index];
            //_fileDatas[index] = new FileData(old.Name, old.Count, state);
            FileData old = _fileDatas[key];
            _fileDatas[key] = new FileData(old.Name, old.Count, state);
            //_fileDatas.Find((f) => f.Name == fileData.Name).SetActive(state);
            FileDataChanged?.Invoke();
            //return _fileDatas[index];
        }

        public FileData GetFileData(string key)
        {
            return _fileDatas[key];
        }
    }
}
