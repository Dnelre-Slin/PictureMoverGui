using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Models
{
    public class SorterConfigurationModel
    {
        public string SourcePath { get; }

        public SorterConfigurationModel(string sourcePath)
        {
            SourcePath = sourcePath;
        }
    }
}
