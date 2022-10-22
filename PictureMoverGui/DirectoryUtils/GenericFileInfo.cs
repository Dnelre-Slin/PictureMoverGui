using MediaDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PictureMoverGui.DirectoryUtils
{
    internal class GenericFileInfo
    {
        private FileInfo fileInfo;
        private MediaFileInfo mediaFileInfo;
        private DateTime defaultDateTime;

        /** Expects one and only on of the parameters to not be null. If both or neither are null, exception will be raised*/
        public GenericFileInfo(FileInfo fileInfo, MediaFileInfo mediaFileInfo, DateTime? defaultDateTime = null)
        {
            this.fileInfo = fileInfo;
            this.mediaFileInfo = mediaFileInfo;

            if (this.fileInfo == null && this.mediaFileInfo == null)
            {
                throw new ArgumentNullException("Both fileInfo and mediaFileInfo are null");
            }
            else if (this.fileInfo != null && this.mediaFileInfo != null)
            {
                throw new ArgumentException("Neither fileInfo nor mediaFileInfo are null. Only accepts one of the parameters to be valid");
            }

            if (defaultDateTime != null)
            {
                this.defaultDateTime = defaultDateTime.Value;
            }
            else
            {
                this.defaultDateTime = DateTime.MinValue;
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                if (this.fileInfo != null)
                {
                    return this.fileInfo.LastWriteTime;
                }
                else
                {
                    return this.mediaFileInfo.LastWriteTime.HasValue ? this.mediaFileInfo.LastWriteTime.Value : defaultDateTime;
                }
            }
        }

        public string Name
        {
            get
            {
                if (this.fileInfo != null)
                {
                    return this.fileInfo.Name;
                }
                else
                {
                    return this.mediaFileInfo.Name;
                }
            }
        }

        public Stream OpenRead()
        {
            if (this.fileInfo != null)
            {
                return this.fileInfo.OpenRead();
            }
            else
            {
                return this.mediaFileInfo.OpenRead();
            }
        }

        public void CopyTo(string destFileName, bool overwrite = false)
        {
            if (this.fileInfo != null)
            {
                this.fileInfo.CopyTo(destFileName, overwrite);
            }
            else
            {
                this.mediaFileInfo.CopyTo(destFileName, overwrite);
            }
        }

        public void MoveTo(string destFileName, bool overwrite = false)
        {
            if (this.fileInfo != null)
            {
                this.fileInfo.MoveTo(destFileName, overwrite);
            }
            else
            {
                throw new NotSupportedException("MediaFileInfo does not support MoveTo");
            }
        }
    }
}
