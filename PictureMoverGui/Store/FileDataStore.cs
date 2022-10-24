﻿using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class FileDataStore
    {
        private List<FileData> _fileDatas;
        public IEnumerable<FileData> FileDatas => _fileDatas;

        public FileDataStore()
        {
            _fileDatas = new List<FileData>();
            _fileDatas.Add(new FileData("jpeg", 14, true));
            _fileDatas.Add(new FileData("png", 25, true));
            _fileDatas.Add(new FileData("db", 4, false));
            _fileDatas.Add(new FileData("mp4", 9, true));
            _fileDatas.Add(new FileData("ini", 2, false));
        }
    }
}
