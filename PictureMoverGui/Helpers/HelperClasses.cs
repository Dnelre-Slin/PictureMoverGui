using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Helpers.HelperClasses
{
    public class PictureMoverArguments
    {
        public string DestinationPath { get; }
        public bool DoCopy { get; }
        public bool DoMakeStructured { get; }
        public bool DoRename { get; }
        public NameCollisionActionEnum NameCollisionAction { get; }
        public CompareFilesActionEnum CompareFilesAction { get; }
        public HashTypeEnum HashTypeAction { get; }
        public List<string> ValidExtensions { get; }
        public List<SimpleEventData> EventDataList { get; }
        public MediaTypeEnum SorterMediaType { get; }
        public string PictureRetrieverSource { get; }
        public DateTime PictureRetrieverNewerThan { get; }

        public Action<RunStates> UpdateRunState { get; }
        public Action<double> UpdateRunPercentage { get; }
        public Action<string> AddRunStatusLog { get; }
        public Action<int> WorkDone { get; }

        public PictureMoverArguments(
            string destinationPath, 
            bool doCopy, 
            bool doMakeStructured, 
            bool doRename, 
            NameCollisionActionEnum nameCollisionAction, 
            CompareFilesActionEnum compareFilesAction, 
            HashTypeEnum hashTypeAction, 
            List<string> validExtensions, 
            List<SimpleEventData> eventDataList, 
            MediaTypeEnum sorterMediaType, 
            string pictureRetrieverSource, 
            DateTime pictureRetrieverNewerThan, 
            Action<RunStates> updateRunState, 
            Action<double> updateRunPercentage, 
            Action<string> addRunStatusLog, 
            Action<int> workDone)
        {
            DestinationPath = destinationPath;
            DoCopy = doCopy;
            DoMakeStructured = doMakeStructured;
            DoRename = doRename;
            NameCollisionAction = nameCollisionAction;
            CompareFilesAction = compareFilesAction;
            HashTypeAction = hashTypeAction;
            ValidExtensions = validExtensions;
            EventDataList = eventDataList;
            SorterMediaType = sorterMediaType;
            PictureRetrieverSource = pictureRetrieverSource;
            PictureRetrieverNewerThan = pictureRetrieverNewerThan;
            UpdateRunState = updateRunState;
            UpdateRunPercentage = updateRunPercentage;
            AddRunStatusLog = addRunStatusLog;
            WorkDone = workDone;
        }
    }
}
