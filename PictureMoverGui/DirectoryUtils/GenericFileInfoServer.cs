using PictureMoverGui.Helpers;
using MediaDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PictureMoverGui.DirectoryUtils
{
    public class GenericFileInfoServer : IDisposable
    {
        private MediaDevice _mediaDevice;
        private DirectoryInfo _directroyInfo;

        /** Expects one and only on of the parameters to not be null. If both or neither are null, exception will be raised*/
        public GenericFileInfoServer(DirectoryInfo directoryInfo, MediaDevice mediaDevice)
        {
            _directroyInfo = directoryInfo;
            _mediaDevice = mediaDevice;

            if (this._directroyInfo == null && this._mediaDevice == null)
            {
                throw new ArgumentNullException("Both directroyInfo and mediaDevice are null");
            }
            else if (this._directroyInfo != null && this._mediaDevice != null)
            {
                throw new ArgumentException("Neither directroyInfo nor mediaDevice are null. Only accepts one of the parameters to be valid");
            }

            if (_mediaDevice != null)
            {
                _mediaDevice.Connect();
            }
        }

        public IEnumerable<GenericFileInfo> EnumerateFiles()
        {
            return EnumerateFiles("*");
        }

        public IEnumerable<GenericFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (_directroyInfo != null)
            {
                //foreach (var v in _directroyInfo.EnumerateFiles(,))
                foreach (var file in _directroyInfo.EnumerateFiles(searchPattern, searchOption).CatchUnauthorizedAccessExceptions(WorkerHelpers.HandleFileAccessExceptions))
                {
                    yield return new GenericFileInfo(file, null);
                }
            }
            else
            {
                //_mediaDevice.EnumerateFiles(searchPattern, searchOption);
                MediaDriveInfo[] mi = _mediaDevice.GetDrives();
                foreach (var drive in _mediaDevice.GetDrives())
                {
                    foreach (var file in drive.RootDirectory.EnumerateFiles(searchPattern, searchOption).CatchUnauthorizedAccessExceptions(WorkerHelpers.HandleFileAccessExceptions))
                    {
                        yield return new GenericFileInfo(null, file);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_mediaDevice != null)
            {
                _mediaDevice.Disconnect();
            }
        }
    }
}
