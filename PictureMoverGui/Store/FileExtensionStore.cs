using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PictureMoverGui.Store
{
    public class FileExtensionStore
    {
        public event Action<FileExtensionModel> FileExtensionChanged;
        public event Action<IEnumerable<FileExtensionModel>> FileExtensionDictReset;

        private List<FileExtensionModel> _fileExtensionList;
        public IEnumerable<int> FileExtensionKeys => Enumerable.Range(0, _fileExtensionList.Count);
        public IEnumerable<FileExtensionModel> FileExtensionValues => _fileExtensionList;

        public FileExtensionStore()
        {
            _fileExtensionList = new List<FileExtensionModel>();
        }

        public void SetActive(int key, bool state)
        {
            FileExtensionModel old = _fileExtensionList[key];
            _fileExtensionList[key] = new FileExtensionModel(old.Name, old.Count, state);
            FileExtensionChanged?.Invoke(_fileExtensionList[key]);
        }

        public FileExtensionModel GetFileExtension(int key)
        {
            return _fileExtensionList[key];
        }

        public void Set(List<FileExtensionModel> newFileExtensionDict)
        {
            _fileExtensionList = newFileExtensionDict;
            FileExtensionDictReset?.Invoke(_fileExtensionList);
        }

        public void Set(Dictionary<string, int> extensionCount)
        {
            _fileExtensionList.Clear();
            foreach (var extCount in extensionCount)
            {
                _fileExtensionList.Add(new FileExtensionModel(extCount.Key, extCount.Value, ExtensionLookup.imageAndVideoExtensions.Contains(extCount.Key)));
            }
            _fileExtensionList.Sort((a, b) =>
            {
                if (a.Active == b.Active)
                {
                    return a.Name.CompareTo(b.Name); // Sort alfabetically if both have the same active state
                }
                else // One will have active true, and the other will have active false
                {
                    return a.Active ? -1 : 1; // Sort all active true before active false
                }
            });
            FileExtensionDictReset?.Invoke(_fileExtensionList);
        }

        public void Clear()
        {
            _fileExtensionList.Clear();
            FileExtensionDictReset?.Invoke(_fileExtensionList);
        }

        public List<string> GetListOfValidExtension()
        {
            if (_fileExtensionList.Count == 0)
            {
                return ExtensionLookup.imageAndVideoExtensions;
            }
            List<string> list = new List<string>();
            foreach (var ext in _fileExtensionList)
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
