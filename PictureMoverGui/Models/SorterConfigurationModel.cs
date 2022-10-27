using PictureMoverGui.Helpers;
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
        public NameCollisionActionEnum NameCollisionAction { get; }
        public CompareFilesActionEnum CompareFilesAction { get; }
        public HashTypeEnum HashType { get; }
        public MediaTypeEnum MediaType { get; }

        public SorterConfigurationModel(
            string sourcePath, 
            string destinationPath,
            bool doCopy,
            bool doStructured,
            bool doRename,
            NameCollisionActionEnum nameCollisionAction,
            CompareFilesActionEnum compareFilesAction,
            HashTypeEnum hashType,
            MediaTypeEnum mediaType)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            DoCopy = doCopy;
            DoStructured = doStructured;
            DoRename = doRename;
            NameCollisionAction = nameCollisionAction;
            CompareFilesAction = compareFilesAction;
            HashType = hashType;
            MediaType = mediaType;
        }

        public SorterConfigurationModel(SorterConfigurationEditor editor)
        {
            SourcePath = editor.SourcePath;
            DestinationPath = editor.DestinationPath;
            DoCopy = editor.DoCopy;
            DoStructured = editor.DoStructured;
            DoRename = editor.DoRename;
            NameCollisionAction = editor.NameCollisionAction;
            CompareFilesAction = editor.CompareFilesAction;
            HashType = editor.HashType;
            MediaType = editor.MediaType;
        }
    }
}
