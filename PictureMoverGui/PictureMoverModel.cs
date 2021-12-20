﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace PictureMoverGui
{
    public class PictureMoverModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string arg)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(arg));
            }
        }

        public PictureMoverModel()
        {
            ////_eventThing = new EventStruct("Test", DateTime.Now, DateTime.Now);
            //_eventThing = new EventStruct("Test", "13.12.2021 17:13:05", "13.12.2021 17:13:05");
            ////_eventThing = DateTime.Now;
            ////_eventThing = "13.12.2021 17:13:05";

            //_timeString = "00:00:00";

            //_listOfStuffHours = new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
            //_listOfStuffMinutesAndSeconds = new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };

            //_editDateTime = false;

            _disableAllConfigDuringRun = true;


            EventData eventData1 = new EventData("Paris", new EventDateTime(new DateTime(2019, 03, 04, 12, 13, 14)), new EventDateTime(new DateTime(2019, 05, 14, 21, 04, 55)));
            EventData eventData2 = new EventData("New York", new EventDateTime(new DateTime(2015, 11, 11, 09, 17, 54)), new EventDateTime(new DateTime(2015, 12, 02, 11, 12, 05)));

            _eventDataList = new ObservableCollection<EventData>();
            _eventDataList.Add(eventData1);
            _eventDataList.Add(eventData2);

            _eventDataEdit = null;

            _sourceDirSat = false;
            _destinationDirSat = false;
            _nameCollisionAction = NameCollisionActionEnum.CompareFiles;
            _compareFilesAction = CompareFilesActionEnum.NameDateAndHash;
            _hashTypeAction = HashTypeEnum.MD5;
            _runningState = RunStates.Idle;
            //_nrOfFilesInCurrentDir = 0;
            //_extensionMapInCurrentDir = new Dictionary<string, int>();
            _extensionInfoList = new ObservableCollection<ExtensionInfo>();
            _chkboxDoCopyChecked = false;
            _chkboxDoStructuredChecked = true;
            _chkboxDoRenameChecked = true;
            _labelSourceDirContent = "";
            _labelDestinationDirContent = "";
            _lastSourceInfoGatherTime = DateTime.Now;

            //Set source directory content to the stored value, if it is a valid directory, else set it to an empty string.
            string start_source_dir = Properties.Settings.Default.UnsortedDir;
            labelSourceDirContent = !string.IsNullOrEmpty(start_source_dir) && new DirectoryInfo(start_source_dir).Exists ? start_source_dir : "";
            //Set destination directory content to the stored value. No need to check if IsNullOrEmpty, as the destination folder is allowed to not exist.
            labelDestinationDirContent = Properties.Settings.Default.SortedDir;
        }


        private bool _disableAllConfigDuringRun;
        public bool disableAllConfigDuringRun
        {
            get { return _disableAllConfigDuringRun; }
            private set
            {
                _disableAllConfigDuringRun = value;
                OnPropertyChanged("disableAllConfigDuringRun");
                OnPropertyChanged("AllowConfigationButtons");
            }
        }

        private bool _sourceDirSat;
        public bool sourceDirSat
        {
            get { return _sourceDirSat; }
            private set
            {
                _sourceDirSat = value;
                OnPropertyChanged("sourceDirSat");
                OnPropertyChanged("AllowSwapOperation");
                OnPropertyChanged("AllowStartingMover");
                OnPropertyChanged("StatusMessageContent");
            }
        }

        private bool _destinationDirSat;
        public bool destinationDirSat
        {
            get { return _destinationDirSat; }
            private set
            {
                _destinationDirSat = value;
                OnPropertyChanged("destinationDirSat");
                OnPropertyChanged("AllowSwapOperation");
                OnPropertyChanged("AllowStartingMover");
                OnPropertyChanged("StatusMessageContent");
            }
        }

        private RunStates _runningState;
        public RunStates runningState
        {
            get { return _runningState; }
            set
            {
                _runningState = value;
                OnPropertyChanged("runningState");
                OnPropertyChanged("AllowGatherInfoDir");
                OnPropertyChanged("AllowSwapOperation");
                OnPropertyChanged("AllowStartingMover");
                OnPropertyChanged("StatusMessageContent");
                OnPropertyChanged("GatherDirInfoCancelButtonVisibility");
                OnPropertyChanged("StartSorterCancelButtonVisibility");
                OnPropertyChanged("AllowConfigationButtons");
            }
        }

        private ObservableCollection<ExtensionInfo> _extensionInfoList;
        public ObservableCollection<ExtensionInfo> extensionInfoList
        {
            get { return _extensionInfoList; }
            set
            {
                _extensionInfoList = value;
                OnPropertyChanged("extensionInfoList");
                OnPropertyChanged("nrOfFilesInCurrentDir");
                OnPropertyChanged("validExtensionsInCurrentDir");
            }
        }

        private ObservableCollection<EventData> _eventDataList;
        public ObservableCollection<EventData> eventDataList
        {
            get { return _eventDataList; }
            set
            {
                _eventDataList = value;
                OnPropertyChanged("eventDataList");
                OnPropertyChanged("eventDataEdit");
                OnPropertyChanged("ShowEventListView");
                OnPropertyChanged("ShowEventEditView");
            }
        }

        private EventData _eventDataEdit;
        public EventData eventDataEdit
        {
            get { return _eventDataEdit; }
            set
            {
                _eventDataEdit = value;
                OnPropertyChanged("eventDataEdit");
                OnPropertyChanged("ShowEventListView");
                OnPropertyChanged("ShowEventEditView");
            }
        }

        private bool _chkboxDoCopyChecked;
        public bool chkboxDoCopyChecked
        {
            get { return _chkboxDoCopyChecked; }
            set
            {
                _chkboxDoCopyChecked = value;
                OnPropertyChanged("chkboxDoCopyChecked");
            }
        }

        private bool _chkboxDoStructuredChecked;
        public bool chkboxDoStructuredChecked
        {
            get { return _chkboxDoStructuredChecked; }
            set
            {
                _chkboxDoStructuredChecked = value;
                OnPropertyChanged("chkboxDoStructuredChecked");
            }
        }

        private bool _chkboxDoRenameChecked;
        public bool chkboxDoRenameChecked
        {
            get { return _chkboxDoRenameChecked; }
            set
            {
                _chkboxDoRenameChecked = value;
                OnPropertyChanged("chkboxDoRenameChecked");
            }
        }

        private string _labelSourceDirContent;
        public string labelSourceDirContent
        {
            get { return _labelSourceDirContent; }
            set
            {
                if (value != Properties.Settings.Default.UnsortedDir)
                {
                    Properties.Settings.Default.UnsortedDir = value;
                    Properties.Settings.Default.Save();
                }
                if (string.IsNullOrEmpty(value))
                {
                    sourceDirSat = false;
                    _labelSourceDirContent = App.Current.FindResource("DefaultEmptyPath").ToString();
                }
                else
                {
                    sourceDirSat = true;
                    _labelSourceDirContent = value;
                }
                OnPropertyChanged("labelSourceDirContent");
            }
        }

        private string _labelDestinationDirContent;
        public string labelDestinationDirContent
        {
            get { return _labelDestinationDirContent; }
            set
            {
                if (value != Properties.Settings.Default.SortedDir)
                {
                    Properties.Settings.Default.SortedDir = value;
                    Properties.Settings.Default.Save();
                }
                if (string.IsNullOrEmpty(value))
                {
                    destinationDirSat = false;
                    _labelDestinationDirContent = App.Current.FindResource("DefaultEmptyPath").ToString();
                }
                else
                {
                    destinationDirSat = true;
                    _labelDestinationDirContent = value;
                }
                OnPropertyChanged("labelDestinationDirContent");
            }
        }

        private int _statusPercentage;
        public int statusPercentage
        {
            get { return _statusPercentage; }
            set
            {
                _statusPercentage = value;
                OnPropertyChanged("statusPercentage");
                OnPropertyChanged("StatusMessageContent");
                OnPropertyChanged("ArcEndAngle");
            }
        }

        private NameCollisionActionEnum _nameCollisionAction;
        public NameCollisionActionEnum nameCollisionAction
        {
            get { return _nameCollisionAction; }
            set
            {
                _nameCollisionAction = value;
                OnPropertyChanged("nameCollisionAction");
                OnPropertyChanged("CompareFilesOptionsEditable");
                OnPropertyChanged("HashTypeOptionsEditable");
            }
        }

        private CompareFilesActionEnum _compareFilesAction;
        public CompareFilesActionEnum compareFilesAction
        {
            get { return _compareFilesAction; }
            set
            {
                _compareFilesAction = value;
                OnPropertyChanged("compareFilesAction");
                OnPropertyChanged("HashTypeOptionsEditable");
            }
        }

        private HashTypeEnum _hashTypeAction;
        public HashTypeEnum hashTypeAction
        {
            get { return _hashTypeAction; }
            set
            {
                _hashTypeAction = value;
                OnPropertyChanged("hashTypeAction");
            }
        }

        private DateTime _lastSourceInfoGatherTime;
        public DateTime lastSourceInfoGatherTime
        {
            get { return _lastSourceInfoGatherTime; }
            set
            {
                _lastSourceInfoGatherTime = value;
                OnPropertyChanged("lastSourceInfoGatherTime");
            }
        }

        public bool AllowSwapOperation
        {
            get { return sourceDirSat && destinationDirSat && runningState == RunStates.Idle; }
        }

        public bool AllowGatherInfoDir
        {
            get { return runningState == RunStates.Idle; }
        }

        public bool AllowStartingMover
        {
            get { return AllowSwapOperation; }
        }

        public Visibility GatherDirInfoCancelButtonVisibility
        {
            get { return runningState == RunStates.DirectoryGathering ? Visibility.Visible : Visibility.Hidden; }
        }        
        
        public Visibility StartSorterCancelButtonVisibility
        {
            get { return runningState == RunStates.RunningSorter ? Visibility.Visible : Visibility.Hidden; }
        }

        public string StatusMessageContent
        {
            get
            {
                switch(runningState)
                {
                    case RunStates.DirectoryValidation:
                        return App.Current.FindResource("DirValidateStatusMessage").ToString();
                    case RunStates.DirectoryGathering:
                        return App.Current.FindResource("DirGatherStatusMessage").ToString();
                    case RunStates.RunningSorter:
                        return $"{statusPercentage}%";
                    case RunStates.Idle:
                        if (AllowStartingMover)
                        {
                            return App.Current.FindResource("ReadyStatusMessage").ToString();
                        }
                        else
                        {
                            return App.Current.FindResource("NotReadyStatusMessage").ToString();
                        }
                    default:
                        return "Error";
                }
            }
        }
        public double ArcEndAngle
        {
            get { return statusPercentage * 3.6; }
        }

        public bool AllowConfigationButtons
        {
            get { return !disableAllConfigDuringRun || runningState != RunStates.RunningSorter; }
        }

        public Visibility ShowEventListView
        {
            get { return eventDataEdit == null ? Visibility.Visible : Visibility.Collapsed; }
        }    
        
        public Visibility ShowEventEditView
        {
            get { return eventDataEdit != null ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool CompareFilesOptionsEditable
        {
            get { return nameCollisionAction == NameCollisionActionEnum.CompareFiles; }
        }

        public bool HashTypeOptionsEditable
        {
            get { return CompareFilesOptionsEditable && compareFilesAction != CompareFilesActionEnum.NameAndDateOnly; }
        }
    }
}
