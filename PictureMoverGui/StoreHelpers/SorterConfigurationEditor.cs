using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.StoreHelpers
{
    public class SorterConfigurationEditor
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public bool DoCopy { get; set; }
        public bool DoStructured { get; set; }
        public bool DoRename { get; set; }

        public SorterConfigurationEditor(SorterConfigurationModel model)
        {
            SourcePath = model.SourcePath;
            DestinationPath = model.DestinationPath;
            DoCopy = model.DoCopy;
            DoStructured = model.DoStructured;
            DoRename = model.DoRename;
        }
    }
}
