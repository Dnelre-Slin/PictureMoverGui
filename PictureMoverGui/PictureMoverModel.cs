using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace PictureMoverGui
{
    public class PictureMoverModel : INotifyPropertyChanged
    {
        public enum RunStates
        {
            DirectoryValidation,
            DirectoryGathering,
            RunningSorter,
            Idle
        }

        public class ExtensionInfo
        {
            public ExtensionInfo(string Name, int Amount, bool Active)
            {
                this.Name = Name;
                this.Amount = Amount;
                this.Active = Active;
            }
            public string Name { get; set; }
            public int Amount { get; set; }
            public bool Active { get; set; }
        }

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
            _disableAllConfigDuringRun = true;

            _sourceDirSat = false;
            _destinationDirSat = false;
            _nameCollisionAction = NameCollisionActionEnum.HashCheck;
            _runningState = RunStates.Idle;
            _nrOfFilesInCurrentDir = 0;
            //_extensionMapInCurrentDir = new Dictionary<string, int>();
            _extensionInfoList = new List<ExtensionInfo>();
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

        private int _nrOfFilesInCurrentDir;
        public int nrOfFilesInCurrentDir
        {
            get { return _nrOfFilesInCurrentDir; }
            private set
            {
                _nrOfFilesInCurrentDir = value;
                OnPropertyChanged("nrOfFilesInCurrentDir");
            }
        }

        private List<string> _validExtensionsInCurrentDir;
        public List<string> validExtensionsInCurrentDir
        {
            get { return _validExtensionsInCurrentDir; }
            private set
            {
                _validExtensionsInCurrentDir = value;
                OnPropertyChanged("validExtensionsInCurrentDir");
            }
        }

        private List<ExtensionInfo> _extensionInfoList;
        public List<ExtensionInfo> extensionInfoList
        {
            get { return _extensionInfoList; }
            set
            {
                _extensionInfoList = value;
                int nrOfFiles = 0;
                List<string> validExtension = new List<string>();
                foreach (ExtensionInfo info in _extensionInfoList)
                {
                    if (info.Active) // Count files that have extensions that are 'Active'
                    {
                        nrOfFiles += info.Amount;
                        validExtension.Add(info.Name);
                    }
                }
                nrOfFilesInCurrentDir = nrOfFiles;
                validExtensionsInCurrentDir = validExtension;
                OnPropertyChanged("extensionInfoList");
                OnPropertyChanged("nrOfFilesInCurrentDir");
                OnPropertyChanged("validExtensionsInCurrentDir");
            }
        }

        //private Dictionary<string, int> _extensionMapInCurrentDir;
        //public Dictionary<string, int> extensionMapInCurrentDir
        //{
        //    get { return _extensionMapInCurrentDir; }
        //    set
        //    {
        //        _extensionMapInCurrentDir = value;
        //        OnPropertyChanged("extensionMapInCurrentDir");
        //    }
        //}

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
    }
}
