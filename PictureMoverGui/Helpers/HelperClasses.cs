using MediaDevices;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;

namespace PictureMoverGui.Helpers.HelperClasses
{
    public class PictureMoverArguments
    {
        public RunStates RunState { get; }
        public List<string> DestinationPaths { get; }
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
        public MediaDevice SelectedMediaDevice { get; }
        public DateTime PictureRetrieverNewerThan { get; }

        public Action<RunStates> UpdateRunState { get; }
        public Action<double> UpdateRunPercentage { get; }
        public Action<string> AddRunStatusLog { get; }
        public Action<int> IncrementInfoFileCount { get; }
        public Action<WorkStatus, int> WorkDone { get; }

        public PictureMoverArguments(
            RunStates runState, 
            List<string> destinationPaths, 
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
            MediaDevice selectedMediaDevice,
            DateTime pictureRetrieverNewerThan,
            Action<RunStates> updateRunState, 
            Action<double> updateRunPercentage, 
            Action<string> addRunStatusLog, 
            Action<int> incrementInfoFileCount, 
            Action<WorkStatus, int> workDone)
        {
            RunState = runState;
            DestinationPaths = destinationPaths;
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
            SelectedMediaDevice = selectedMediaDevice;
            PictureRetrieverNewerThan = pictureRetrieverNewerThan;
            UpdateRunState = updateRunState;
            UpdateRunPercentage = updateRunPercentage;
            AddRunStatusLog = addRunStatusLog;
            IncrementInfoFileCount = incrementInfoFileCount;
            WorkDone = workDone;
        }
    }

    public class ExtensionCounterArguments
    {
        public RunStates RunState { get; }
        public MediaTypeEnum MediaType;
        public string Source;
        public MediaDevice SelectedMediaDevice { get; }
        public DateTime NewerThan;

        public Action<RunStates> UpdateRunState;
        public Action<int> IncrementInfoFileCount { get; }
        public Action<WorkStatus, Dictionary<string, int>> WorkDone;

        public ExtensionCounterArguments(
            RunStates runState,
            MediaTypeEnum mediaType,
            string source,
            MediaDevice selectedMediaDevice,
            DateTime newerThan,
            Action<RunStates> updateRunState,
            Action<int> incrementInfoFileCount,
            Action<WorkStatus, Dictionary<string, int>> workDone)
        {
            RunState = runState;
            MediaType = mediaType;
            Source = source;
            SelectedMediaDevice = selectedMediaDevice;
            NewerThan = newerThan;
            UpdateRunState = updateRunState;
            IncrementInfoFileCount = incrementInfoFileCount;
            WorkDone = workDone;
        }
    }
}
