using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class MasterStore
    {
        public DummyStore DummyStore { get; }
        public FileDataStore FileDataStore { get; }
        public SorterConfigurationStore SorterConfigurationStore { get; }

        public MasterStore(string name, bool active)
        {
            DummyStore = new DummyStore(name, active);
            FileDataStore = new FileDataStore();
            SorterConfigurationStore = new SorterConfigurationStore();
        }
    }
}
