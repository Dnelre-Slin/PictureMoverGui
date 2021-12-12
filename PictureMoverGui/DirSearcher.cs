using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PictureMoverGui
{
    class DirSearcher
    {
        private List<string> validExtensions;
        private bool _cancel;
        public bool cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        public DirSearcher(List<string> validExtensions = null)
        {
            this._cancel = false;
            this.validExtensions = validExtensions;
        }

        public void DirSearch(DirectoryInfo d, Action<DirectoryInfo, FileInfo> callback)
        {
            try
            {
                foreach (FileInfo file in d.GetFiles())
                {
                    if (this.cancel)
                    {
                        return;
                    }
                    //if (file.Extension != ".ini" && file.Extension != ".db")
                    if (this.IsValidFileExtension(file.Extension))
                    {
                        callback(d, file); // Callback will return false, if the dirsearch should break.
                    }
                }
                foreach (DirectoryInfo subD in d.GetDirectories())
                {
                    if (this.cancel)
                    {
                        return;
                    }
                    DirSearch(subD, callback);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                System.Diagnostics.Trace.TraceError(e.Message);
            }
        }

        private bool IsValidFileExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return false; // Not a valid extension, if it has no extension.
            }
            string ext = extension.ToLower(); // To lower case. Example .JPEG -> .jpeg
            ext = ext.Substring(1);           // Remove leading '.'. Example: .jpeg -> jpeg
            if (validExtensions == null)
            {
                return true;
            }
            return validExtensions.Contains(ext);
        }

        static public bool FilenameInDir(DirectoryInfo d, string filename)
        {
            foreach (FileInfo file in d.GetFiles())
            {
                if (file.Name.ToLower() == filename.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        // Check if there are any directories in d, with a newer 'LastWriteTime' than dt.
        static public bool DirLastWriteCompare(DirectoryInfo d, DateTime dt, int max_depth = 10, int current_depth = 0)
        {
            if (d.LastWriteTime > dt)
            {
                return true;
            }
            if (current_depth >= max_depth)
            {
                return false;
            }
            foreach (DirectoryInfo subD in d.GetDirectories())
            {
                if (DirLastWriteCompare(subD, dt, max_depth, current_depth + 1))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
