using System;
using System.Collections.Generic;
using System.Text;

namespace PictureMoverGui.Helpers
{
    public static class WorkerHelpers
    {
        static public void HandleFileAccessExceptions(Exception e)
        {
            System.Diagnostics.Trace.TraceError(e.Message);
        }

        static public bool IsValidFileExtension(string extension, List<string> validExtensions)
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

        static public bool IsFileNewerThan(DateTime fileLastWriteTime, DateTime newerThan)
        {
            return fileLastWriteTime >= newerThan;
        }
    }
}
