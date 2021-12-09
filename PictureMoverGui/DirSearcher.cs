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
            foreach (FileInfo file in d.GetFiles())
            {
                if (this.cancel)
                {
                    break;
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
                    break;
                }
                DirSearch(subD, callback);
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
    }
}
