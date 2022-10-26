using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class MasterStore
    {
        public DummyStore DummyStore { get; }
        public FileExtensionStore FileExtensionStore { get; }
        public SorterConfigurationStore SorterConfigurationStore { get; }
        public RunningStore RunningStore { get; }

        public MasterStore(string name, bool active)
        {
            DummyStore = new DummyStore(name, active);
            FileExtensionStore = new FileExtensionStore();
            SorterConfigurationStore = new SorterConfigurationStore();
            RunningStore = new RunningStore();
        }
    }
}
