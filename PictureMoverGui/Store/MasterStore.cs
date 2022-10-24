using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class MasterStore
    {
        public DummyStore Dummy { get; }
        public FileDataStore FileData { get; }

        public MasterStore(string name, bool active)
        {
            Dummy = new DummyStore(name, active);
            FileData = new FileDataStore();
        }
    }
}
