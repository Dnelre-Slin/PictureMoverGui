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
                    SaveToFile();
                    SorterConfigurationChanged?.Invoke(_sorterConfiguration);
                }
            }
        }

        public SorterConfigurationStore()
        {
            //InitSorterConfiguration();
            LoadFromFile();
        }

        //protected void InitSorterConfiguration()
        //{
        //    _sorterConfiguration = new SorterConfigurationModel(
        //        "FirstFolder", 
        //        "destPath", 
        //        true, 
        //        true, 
        //        true,
        //        NameCollisionActionEnum.CompareFiles,
        //        CompareFilesActionEnum.NameAndDateOnly,
        //        HashTypeEnum.MD5,
        //        MediaTypeEnum.NormalDirectory);
        //    // Todo: Load from settings
        //}

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

        protected void SaveToFile()
        {
            if (Properties.Settings.Default.SourceDir != SorterConfiguration.SourcePath)
            {
                Properties.Settings.Default.SourceDir = SorterConfiguration.SourcePath;
            }
            if (Properties.Settings.Default.DestinationDir != SorterConfiguration.DestinationPath)
            {
                Properties.Settings.Default.DestinationDir = SorterConfiguration.DestinationPath;
            }
            if (Properties.Settings.Default.DoCopy != SorterConfiguration.DoCopy)
            {
                Properties.Settings.Default.DoCopy = SorterConfiguration.DoCopy;
            }
            if (Properties.Settings.Default.DoStructured != SorterConfiguration.DoStructured)
            {
                Properties.Settings.Default.DoStructured = SorterConfiguration.DoStructured;
            }
            if (Properties.Settings.Default.DoDateName != SorterConfiguration.DoRename)
            {
                Properties.Settings.Default.DoDateName = SorterConfiguration.DoRename;
            }
            if (Properties.Settings.Default.NameCollisionAction != (int)SorterConfiguration.NameCollisionAction)
            {
                Properties.Settings.Default.NameCollisionAction = (int)SorterConfiguration.NameCollisionAction;
            }            
            if (Properties.Settings.Default.CompareFilesAction != (int)SorterConfiguration.CompareFilesAction)
            {
                Properties.Settings.Default.CompareFilesAction = (int)SorterConfiguration.CompareFilesAction;
            }            
            if (Properties.Settings.Default.HashTypeAction != (int)SorterConfiguration.HashType)
            {
                Properties.Settings.Default.HashTypeAction = (int)SorterConfiguration.HashType;
            }            
            if (Properties.Settings.Default.MediaTypeAction != (int)SorterConfiguration.MediaType)
            {
                Properties.Settings.Default.MediaTypeAction = (int)SorterConfiguration.MediaType;
            }
            Properties.Settings.Default.Save();
        }

        protected void LoadFromFile()
        {
            string sourcePath = Properties.Settings.Default.SourceDir;
            // Check if source path is valid and exists. No need to do this for destination, as destination is allowed to not exist
            sourcePath = !string.IsNullOrEmpty(sourcePath) && new System.IO.DirectoryInfo(sourcePath).Exists ? sourcePath : "";
            SorterConfigurationModel sorterConfigurationModel = new SorterConfigurationModel(
                sourcePath,
                Properties.Settings.Default.DestinationDir,
                Properties.Settings.Default.DoCopy,
                Properties.Settings.Default.DoStructured,
                Properties.Settings.Default.DoDateName,
                (NameCollisionActionEnum)Properties.Settings.Default.NameCollisionAction,
                (CompareFilesActionEnum)Properties.Settings.Default.CompareFilesAction,
                (HashTypeEnum)Properties.Settings.Default.HashTypeAction,
                (MediaTypeEnum)Properties.Settings.Default.MediaTypeAction
                );
            _sorterConfiguration = sorterConfigurationModel;
        }
    }
}
