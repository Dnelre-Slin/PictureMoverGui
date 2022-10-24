using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class FileDataStore
    {
        private List<FileData> _fileDatas;
        public IEnumerable<FileData> FileDatas => _fileDatas;

        public event Action FileDataChanged;

        public FileDataStore()
        {
            _fileDatas = new List<FileData>();
            int index = 0;
            _fileDatas.Add(new FileData(index++, "jpeg", 14, true));
            _fileDatas.Add(new FileData(index++, "png", 25, true));
            _fileDatas.Add(new FileData(index++, "db", 4, false));
            _fileDatas.Add(new FileData(index++, "mp4", 9, true));
            _fileDatas.Add(new FileData(index++, "ini", 2, false));
        }

        //public void SetActive(int index, bool state)
        //{
        //    _fileDatas[index].Active = state;
        //    FileDataChanged?.Invoke();
        //}
    }
}
