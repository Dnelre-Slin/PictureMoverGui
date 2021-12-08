using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
    }
}
