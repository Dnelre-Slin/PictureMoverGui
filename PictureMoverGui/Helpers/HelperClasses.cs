using PictureMoverGui.Models;
using System;
using System.Collections.Generic;

namespace PictureMoverGui.Helpers.HelperClasses
{
    public class PictureMoverArguments
    {
        public RunStates RunState { get; }
        public string DestinationPath { get; }
        public bool DoCopy { get; }
        public bool DoMakeStructured { get; }
        public bool DoRename { get; }
        public NameCollisionActionEnum NameCollisionAction { get; }
        public CompareFilesActionEnum CompareFilesAction { get; }
        public HashTypeEnum HashTypeAction { get; }
        public List<string> ValidExtensions { get; }
        public List<EventDataModel> EventDataList { get; }
        public MediaTypeEnum SorterMediaType { get; }
        public string PictureRetrieverSource { get; }
        public MediaDeviceModel ChosenMediaDevice { get; }
        public DateTime PictureRetrieverNewerThan { get; }

        public Action<RunStates> UpdateRunState { get; }
        public Action<double> UpdateRunPercentage { get; }
        public Action<string> AddRunStatusLog { get; }
        public Action<WorkStatus, int> WorkDone { get; }

        public PictureMoverArguments(
            RunStates runState, 
            string destinationPath, 
            bool doCopy, 
            bool doMakeStructured, 
            bool doRename, 
            NameCollisionActionEnum nameCollisionAction, 
            CompareFilesActionEnum compareFilesAction, 
            HashTypeEnum hashTypeAction, 
            List<string> validExtensions, 
            List<EventDataModel> eventDataList, 
            MediaTypeEnum sorterMediaType, 
            string pictureRetrieverSource,
            MediaDeviceModel chosenMediaDevice,
            DateTime pictureRetrieverNewerThan,
            Action<RunStates> updateRunState, 
            Action<double> updateRunPercentage, 
            Action<string> addRunStatusLog, 
            Action<WorkStatus, int> workDone)
        {
            RunState = runState;
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
            ChosenMediaDevice = chosenMediaDevice;
            PictureRetrieverNewerThan = pictureRetrieverNewerThan;
            UpdateRunState = updateRunState;
            UpdateRunPercentage = updateRunPercentage;
            AddRunStatusLog = addRunStatusLog;
            WorkDone = workDone;
        }
    }

    public class ExtensionCounterArguments
    {
        public RunStates RunState { get; }
        public MediaTypeEnum MediaType;
        public string Source;
        public MediaDeviceModel ChosenMediaDevice { get; }
        public DateTime NewerThan;

        public Action<RunStates> UpdateRunState;
        public Action<WorkStatus, Dictionary<string, int>> WorkDone;

        public ExtensionCounterArguments(
            RunStates runState,
            MediaTypeEnum mediaType,
            string source,
            MediaDeviceModel chosenMediaDevice,
            DateTime newerThan,
            Action<RunStates> updateRunState,
            Action<WorkStatus, Dictionary<string, int>> workDone)
        {
            RunState = runState;
            MediaType = mediaType;
            Source = source;
            ChosenMediaDevice = chosenMediaDevice;
            NewerThan = newerThan;
            UpdateRunState = updateRunState;
            WorkDone = workDone;
        }
    }
}
