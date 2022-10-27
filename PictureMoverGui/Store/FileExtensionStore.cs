using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PictureMoverGui.Store
{
    public class FileExtensionStore
    {
        public event Action<FileExtension> FileExtensionChanged;
        public event Action<IEnumerable<FileExtension>> FileExtensionDictReset;

        private List<FileExtension> _fileExtensionList;
        public IEnumerable<int> FileExtensionKeys => Enumerable.Range(0, _fileExtensionList.Count);
        public IEnumerable<FileExtension> FileExtensionValues => _fileExtensionList;

        public FileExtensionStore()
        {
            _fileExtensionList = new List<FileExtension>();
            //_fileExtensionDict["jpeg"] = new FileExtension("jpeg", 14, true);
            //_fileExtensionDict["png"] = new FileExtension("png", 25, true);
            //_fileExtensionDict["db"] = new FileExtension("db", 4, false);
            //_fileExtensionDict["mp4"] = new FileExtension("mp4", 9, true);
            //_fileExtensionDict["ini"] = new FileExtension("ini", 2, false);

            // TODO: Read extension on startup, based on presat source dir
        }

        public void SetActive(int key, bool state)
        {
            FileExtension old = _fileExtensionList[key];
            _fileExtensionList[key] = new FileExtension(old.Name, old.Count, state);
            FileExtensionChanged?.Invoke(_fileExtensionList[key]);
        }

        public FileExtension GetFileExtension(int key)
        {
            return _fileExtensionList[key];
        }

        public void Set(List<FileExtension> newFileExtensionDict)
        {
            _fileExtensionList = newFileExtensionDict;
            FileExtensionDictReset?.Invoke(_fileExtensionList);
        }

        public void Set(Dictionary<string, int> extensionCount)
        {
            _fileExtensionList.Clear();
            foreach (var extCount in extensionCount)
            {
                _fileExtensionList.Add(new FileExtension(extCount.Key, extCount.Value, ExtensionLookup.imageAndVideoExtensions.Contains(extCount.Key)));
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

        public List<string> GetListOfValidExtension()
        {
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
