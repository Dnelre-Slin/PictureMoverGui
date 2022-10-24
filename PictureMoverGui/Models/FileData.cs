using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class FileData
    {
        public int Index { get; }
        public string Name { get; }
        public int Count { get; }
        //public bool Active { get; set; }

        public FileData(int index, string name, int count, bool active)
        {
            Index = index;
            Name = name;
            Count = count;
            //Active = active;
        }
    }
}
