using PictureMoverGui.StoreHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class SorterConfigurationModel
    {
        public string SourcePath { get; }
        public string DestinationPath { get; }
        public bool DoCopy { get; }
        public bool DoStructured { get; }
        public bool DoRename { get; }

        public SorterConfigurationModel(
            string sourcePath, 
            string destinationPath,
            bool doCopy,
            bool doStructured,
            bool doRename)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            DoCopy = doCopy;
            DoStructured = doStructured;
            DoRename = doRename;
        }

        public SorterConfigurationModel(SorterConfigurationEditor editor)
        {
            SourcePath = editor.SourcePath;
            DestinationPath = editor.DestinationPath;
            DoCopy = editor.DoCopy;
            DoStructured = editor.DoStructured;
            DoRename = editor.DoRename;
        }
    }
}
