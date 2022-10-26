﻿using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Store
{
    public class SorterConfigurationStore
    {
        public event Action<SorterConfigurationModel> SorterConfigurationChanged;

        private SorterConfigurationModel _sorterConfiguration;
        public SorterConfigurationModel SorterConfiguration
        {
            get { return _sorterConfiguration; }
            set 
            {
                if (_sorterConfiguration != value)
                {
                    _sorterConfiguration = value;
                    SorterConfigurationChanged?.Invoke(_sorterConfiguration);
                }
            }
        }

        public SorterConfigurationStore()
        {
            _sorterConfiguration = new SorterConfigurationModel("FirstFolder");
        }

        public void SetSourcePath(string newSourcePath)
        {
            if (newSourcePath != SorterConfiguration.SourcePath)
            {
                SorterConfiguration = new SorterConfigurationModel(newSourcePath);
            }
        }
    }
}