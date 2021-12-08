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
        }

        private bool _sourceDirSat;
        private bool _destinationDirSat;

        public bool sourceDirSat
        {
            get { return _sourceDirSat; }
            set
            {
                _sourceDirSat = value;
                OnPropertyChanged("sourceDirSat");
                OnPropertyChanged("AllowSwapOperation");
            }
        }

        public bool destinationDirSat
        {
            get { return _destinationDirSat; }
            set
            {
                _destinationDirSat = value;
                OnPropertyChanged("destinationDirSat");
                OnPropertyChanged("AllowSwapOperation");
            }
        }

        public bool AllowSwapOperation
        {
            get { return sourceDirSat && destinationDirSat; }
        }
    }
}
