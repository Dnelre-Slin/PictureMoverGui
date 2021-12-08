using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PictureMoverGui
{
    class DirSearcher
    {
        static public void DirSearch(DirectoryInfo d, Action<DirectoryInfo, FileInfo> callback)
        {
            foreach (FileInfo file in d.GetFiles())
            {
                //if (file.Extension != ".ini" && file.Extension != ".db")
                {
                    callback(d, file);
                }
            }
            foreach (DirectoryInfo subD in d.GetDirectories())
            {
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
