using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            _labelStatusMessageContent = "";
            _arcProgressBarAngle = 0;
        }


        private bool _sourceDirSat;
        public bool sourceDirSat
        {
            get { return _sourceDirSat; }
            set
            {
                _sourceDirSat = value;
                OnPropertyChanged("sourceDirSat");
                OnPropertyChanged("AllowSwapOperation");
                OnPropertyChanged("AllowStartingMover");
            }
        }

        private bool _destinationDirSat;
        public bool destinationDirSat
        {
            get { return _destinationDirSat; }
            set
            {
                _destinationDirSat = value;
                OnPropertyChanged("destinationDirSat");
                OnPropertyChanged("AllowSwapOperation");
                OnPropertyChanged("AllowStartingMover");
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
                _labelSourceDirContent = value;
                OnPropertyChanged("labelSourceDirContent");
            }
        }

        private string _labelDestinationDirContent;
        public string labelDestinationDirContent
        {
            get { return _labelDestinationDirContent; }
            set
            {
                _labelDestinationDirContent = value;
                OnPropertyChanged("labelDestinationDirContent");
            }
        }

        private string _labelStatusMessageContent;
        public string labelStatusMessageContent
        {
            get { return _labelStatusMessageContent; }
            set
            {
                _labelStatusMessageContent = value;
                OnPropertyChanged("labelStatusMessageContent");
            }
        }

        private double _arcProgressBarAngle;
        public double arcProgressBarAngle
        {
            get { return _arcProgressBarAngle; }
            set
            {
                _arcProgressBarAngle = value;
                OnPropertyChanged("arcProgressBarAngle");
            }
        }

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
    }
}
