using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace PictureMoverGui
{
    class PictureMoverModel : INotifyPropertyChanged
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
            _sourceDirSat = false;
            _destinationDirSat = false;
            _gatherDirInfoRunning = false;
            _pictureMoverRunning = false;
            _nrOfFilesInCurrentDir = 0;
            _extensionMapInCurrentDir = new Dictionary<string, int>();
            _chkboxDoCopyChecked = false;
            _chkboxDoStructuredChecked = true;
            _chkboxDoRenameChecked = true;
            _labelSourceDirContent = "";
            _labelDestinationDirContent = "";
            _showDoneStatusMessage = false;

            //Set source directory content to the stored value, if it is a valid directory, else set it to an empty string.
            string start_source_dir = Properties.Settings.Default.UnsortedDir;
            labelSourceDirContent = !string.IsNullOrEmpty(start_source_dir) && new DirectoryInfo(start_source_dir).Exists ? start_source_dir : "";
            //Set destination directory content to the stored value. No need to check if IsNullOrEmpty, as the destination folder is allowed to not exist.
            labelDestinationDirContent = Properties.Settings.Default.SortedDir;
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

        private bool _gatherDirInfoRunning;
        public bool gatherDirInfoRunning
        {
            get { return _gatherDirInfoRunning; }
            set
            {
                _gatherDirInfoRunning = value;
                OnPropertyChanged("gatherDirInfoRunning");
                OnPropertyChanged("GatherInfoDirNotRunning");
                OnPropertyChanged("AllowSwapOperation");
                OnPropertyChanged("AllowStartingMover");
                OnPropertyChanged("StatusMessageContent");
                OnPropertyChanged("GatherDirInfoCancelButtonVisibility");
            }
        }

        private bool _pictureMoverRunning;
        public bool pictureMoverRunning
        {
            get { return _pictureMoverRunning; }
            set
            {
                _pictureMoverRunning = value;
                OnPropertyChanged("pictureMoverRunning");
                OnPropertyChanged("AllowStartingMover");
                OnPropertyChanged("StatusMessageContent");
            }
        }

        private int _nrOfFilesInCurrentDir;
        public int nrOfFilesInCurrentDir
        {
            get { return _nrOfFilesInCurrentDir; }
            set
            {
                _nrOfFilesInCurrentDir = value;
                OnPropertyChanged("nrOfFilesInCurrentDir");
            }
        }

        private Dictionary<string, int> _extensionMapInCurrentDir;
        public Dictionary<string, int> extensionMapInCurrentDir
        {
            get { return _extensionMapInCurrentDir; }
            set
            {
                _extensionMapInCurrentDir = value;
                OnPropertyChanged("extensionMapInCurrentDir");
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

        private bool _showDoneStatusMessage;
        public bool showDoneStatusMessage
        {
            get { return _showDoneStatusMessage; }
            set
            {
                _showDoneStatusMessage = value;
                OnPropertyChanged("showDoneStatusMessage");
                OnPropertyChanged("StatusMessageContent");
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

        //private string _labelStatusMessageContent;
        //public string labelStatusMessageContent
        //{
        //    get { return _labelStatusMessageContent; }
        //    set
        //    {
        //        _labelStatusMessageContent = value;
        //        OnPropertyChanged("labelStatusMessageContent");
        //    }
        //}

        //private double _arcProgressBarAngle;
        //public double arcProgressBarAngle
        //{
        //    get { return _arcProgressBarAngle; }
        //    set
        //    {
        //        _arcProgressBarAngle = value;
        //        OnPropertyChanged("arcProgressBarAngle");
        //    }
        //}

        public bool AllowSwapOperation
        {
            get { return sourceDirSat && destinationDirSat && GatherInfoDirNotRunning; }
        }

        public bool GatherInfoDirNotRunning
        {
            get { return !gatherDirInfoRunning; }
        }

        public bool AllowStartingMover
        {
            get { return AllowSwapOperation && !pictureMoverRunning; }
        }

        public Visibility GatherDirInfoCancelButtonVisibility
        {
            get { return gatherDirInfoRunning ? Visibility.Visible : Visibility.Hidden; }
        }

        public string StatusMessageContent
        {
            get
            {
                if (showDoneStatusMessage)
                {
                    return App.Current.FindResource("DoneStatusMessage").ToString();
                }
                else if (pictureMoverRunning)
                {
                    return $"{statusPercentage}%";
                }
                else if (AllowStartingMover)
                {
                    return App.Current.FindResource("ReadyStatusMessage").ToString();
                }
                else
                {
                    return App.Current.FindResource("NotReadyStatusMessage").ToString();
                }
            }
        }

        public double ArcEndAngle
        {
            get { return statusPercentage * 3.6; }
        }
    }
}
