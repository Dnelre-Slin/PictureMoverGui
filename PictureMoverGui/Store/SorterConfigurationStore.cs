using PictureMoverGui.Helpers;
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
                true,
                NameCollisionActionEnum.CompareFiles,
                CompareFilesActionEnum.NameAndDateOnly,
                HashTypeEnum.MD5,
                MediaTypeEnum.NormalDirectory);
            // Todo: Load from settings
        }

        public void SetSourcePath(string sourcePath)
        {
            if (sourcePath != SorterConfiguration.SourcePath)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.SourcePath = sourcePath;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDestinationPath(string destinationPath)
        {
            if (destinationPath != SorterConfiguration.DestinationPath)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DestinationPath = destinationPath;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDoCopy(bool doCopy)
        {
            if (doCopy != SorterConfiguration.DoCopy)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DoCopy = doCopy;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDoStructured(bool doStructured)
        {
            if (doStructured != SorterConfiguration.DoStructured)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DoStructured = doStructured;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetDoRename(bool doRename)
        {
            if (doRename != SorterConfiguration.DoRename)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.DoRename = doRename;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetNameCollisionAction(NameCollisionActionEnum nameCollisionAction)
        {
            if (nameCollisionAction != SorterConfiguration.NameCollisionAction)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.NameCollisionAction = nameCollisionAction;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetCompareFilesAction(CompareFilesActionEnum compareFilesAction)
        {
            if (compareFilesAction != SorterConfiguration.CompareFilesAction)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.CompareFilesAction = compareFilesAction;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetHashType(HashTypeEnum hashType)
        {
            if (hashType != SorterConfiguration.HashType)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.HashType = hashType;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }

        public void SetMediaType(MediaTypeEnum mediaType)
        {
            if (mediaType != SorterConfiguration.MediaType)
            {
                SorterConfigurationEditor editor = new SorterConfigurationEditor(SorterConfiguration);
                editor.MediaType = mediaType;
                SorterConfiguration = new SorterConfigurationModel(editor);
            }
        }
    }
}
