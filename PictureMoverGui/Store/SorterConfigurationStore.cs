using PictureMoverGui.Models;
using PictureMoverGui.StoreHelpers;
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
            InitSorterConfiguration();
        }

        protected void InitSorterConfiguration()
        {
            _sorterConfiguration = new SorterConfigurationModel(
                "FirstFolder", 
                "destPath", 
                true, 
                true, 
                true);
        }

        public void SetSourcePath(string newSourcePath)
        {
            if (newSourcePath != SorterConfiguration.SourcePath)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.SourcePath = newSourcePath;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDestinationPath(string newDestinationPath)
        {
            if (newDestinationPath != SorterConfiguration.DestinationPath)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DestinationPath = newDestinationPath;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDoCopy(bool newDoCopy)
        {
            if (newDoCopy != SorterConfiguration.DoCopy)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DoCopy = newDoCopy;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDoStructured(bool newDoStructured)
        {
            if (newDoStructured != SorterConfiguration.DoStructured)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DoStructured = newDoStructured;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDoRename(bool newDoRename)
        {
            if (newDoRename != SorterConfiguration.DoRename)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DoRename = newDoRename;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }
    }
}
