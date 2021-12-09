using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PictureMoverGui
{
    class DirSearcher
    {
        private bool _cancel;
        public bool cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        public DirSearcher()
        {
            this._cancel = false;
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
