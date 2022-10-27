using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class FileExtensionStore
    {
        public event Action<FileExtension> FileExtensionChanged;
        public event Action<IEnumerable<KeyValuePair<string, FileExtension>>> FileExtensionDictReset;

        private Dictionary<string, FileExtension> _fileExtensionDict;
        public IEnumerable<string> FileExtensionKeys => _fileExtensionDict.Keys;
        public IEnumerable<FileExtension> FileExtensionValues => _fileExtensionDict.Values;
        public IEnumerable<KeyValuePair<string, FileExtension>> FileExtensionDict => _fileExtensionDict;

        public FileExtensionStore()
        {
            _fileExtensionDict = new Dictionary<string, FileExtension>();
            //_fileExtensionDict["jpeg"] = new FileExtension("jpeg", 14, true);
            //_fileExtensionDict["png"] = new FileExtension("png", 25, true);
            //_fileExtensionDict["db"] = new FileExtension("db", 4, false);
            //_fileExtensionDict["mp4"] = new FileExtension("mp4", 9, true);
            //_fileExtensionDict["ini"] = new FileExtension("ini", 2, false);

            // TODO: Read extension on startup, based on presat source dir
        }

        public void SetActive(string key, bool state)
        {
            FileExtension old = _fileExtensionDict[key];
            _fileExtensionDict[key] = new FileExtension(old.Name, old.Count, state);
            FileExtensionChanged?.Invoke(_fileExtensionDict[key]);
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
                _fileExtensionDict[extCount.Key] = new FileExtension(extCount.Key, extCount.Value, ExtensionLookup.imageAndVideoExtensions.Contains(extCount.Key));
            }
            FileExtensionDictReset?.Invoke(_fileExtensionDict);
        }

        public List<string> GetListOfValidExtension()
        {
            List<string> list = new List<string>();
            foreach (var ext in _fileExtensionDict.Values)
            {
                if (ext.Active)
                {
                    list.Add(ext.Name);
                }
            }
            return list;
        }
    }
}
