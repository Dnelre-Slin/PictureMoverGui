using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class FileExtensionStore
    {
        private Dictionary<string, FileExtension> _fileExtensionDict;
        //public IEnumerable<string, FileData> FileDatas => _fileDatas;
        public IEnumerable<string> FileExtensionKeys => _fileExtensionDict.Keys;
        public IEnumerable<FileExtension> FileExtensionValues => _fileExtensionDict.Values;
        public IEnumerable<KeyValuePair<string, FileExtension>> FileExtensionDict => _fileExtensionDict;
        //public IEnumerable<Dictionary<string, FileData>.> FileData2 => _fileDatas.;

        public event Action<FileExtension> FileExtensionChanged;
        public event Action<IEnumerable<KeyValuePair<string, FileExtension>>> FileExtensionDictReset;

        public FileExtensionStore()
        {
            _fileExtensionDict = new Dictionary<string, FileExtension>();
            _fileExtensionDict["jpeg"] = new FileExtension("jpeg", 14, true);
            _fileExtensionDict["png"] = new FileExtension("png", 25, true);
            _fileExtensionDict["db"] = new FileExtension("db", 4, false);
            _fileExtensionDict["mp4"] = new FileExtension("mp4", 9, true);
            _fileExtensionDict["ini"] = new FileExtension("ini", 2, false);
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
            FileExtension old = _fileExtensionDict[key];
            _fileExtensionDict[key] = new FileExtension(old.Name, old.Count, state);
            //_fileDatas.Find((f) => f.Name == fileData.Name).SetActive(state);
            FileExtensionChanged?.Invoke(_fileExtensionDict[key]);
            //return _fileDatas[index];
        }

        public FileExtension GetFileExtension(string key)
        {
            return _fileExtensionDict[key];
        }

        public void Set(Dictionary<string, FileExtension> newFileExtensionDict)
        {
            _fileExtensionDict = newFileExtensionDict;
            FileExtensionDictReset?.Invoke(_fileExtensionDict);
        }

        public void Set(Dictionary<string, int> extensionCount)
        {
            _fileExtensionDict.Clear();
            foreach (var extCount in extensionCount)
            {
                _fileExtensionDict[extCount.Key] = new FileExtension(extCount.Key, extCount.Value, true);
            }
            FileExtensionDictReset?.Invoke(_fileExtensionDict);
        }
    }
}
