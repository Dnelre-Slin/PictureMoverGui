using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class MasterStore
    {
        public FileExtensionStore FileExtensionStore { get; }
        public SorterConfigurationStore SorterConfigurationStore { get; }
        public RunningStore RunningStore { get; }
        public EventDataStore EventDataStore { get; }

        public MasterStore()
        {
            FileExtensionStore = new FileExtensionStore();
            SorterConfigurationStore = new SorterConfigurationStore();
            RunningStore = new RunningStore();
            EventDataStore = new EventDataStore();
        }
    }
}
