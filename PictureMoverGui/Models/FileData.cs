using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class FileData
    {
        public string Name { get; }
        public int Count { get; }
        public bool Active { get; set; }

        public FileData(string name, int count, bool active)
        {
            Name = name;
            Count = count;
            Active = active;
        }
    }
}
