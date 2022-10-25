using MediaDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace PictureMoverGui.DirectoryUtils
{
    public class PictureRetriever : IDisposable
    {
        private GenericFileInfoServer _genericFileInfoServer;
        public PictureRetriever(string sourceDir)
        {
            //DirectoryInfo d = new DirectoryInfo(sourceDir);
            //_genericFileInfoServer = new GenericFileInfoServer(d, null);

            MediaDevice m = null;
            foreach (var dev in MediaDevice.GetDevices())
            {
                if (dev.FriendlyName == "Nils sin S20+")
                {
                    m = dev;
                    break;
                }
            }
            if (m != null)
            {
                _genericFileInfoServer = new GenericFileInfoServer(null, m);
            }
        }

        public IEnumerable<GenericFileInfo> EnumerateFiles()
        {
            return _genericFileInfoServer.EnumerateFiles();
        }

        public IEnumerable<GenericFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return _genericFileInfoServer.EnumerateFiles(searchPattern, searchOption);
        }

        public void Dispose()
        {
            if (_genericFileInfoServer != null)
            {
                _genericFileInfoServer.Dispose();
            }
        }

        static private void tmpCatcher(Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e);
        }

        static public Dictionary<string, int> GetExtensions(string search_dir, BackgroundWorker sender_worker)
        {
            //DirectoryInfo d = new DirectoryInfo(search_dir);
            using (PictureRetriever pictureRetriever = new PictureRetriever(search_dir))
            {
                Dictionary<string, int> extensionMap = new Dictionary<string, int>();

                foreach (GenericFileInfo file in pictureRetriever.EnumerateFiles("*", SearchOption.AllDirectories).CatchExceptions(tmpCatcher))
                {
                    if (string.IsNullOrEmpty(file.StrictExtension))
                    {
                        continue; // Do not add extension, if file has no extension.
                    }

                    string ext = file.StrictExtension;
                    if (extensionMap.ContainsKey(ext))
                    {
                        extensionMap[ext] += 1;
                    }
                    else
                    {
                        extensionMap[ext] = 1;
                    }

                    if (sender_worker.CancellationPending)
                    {
                        break;
                    }
                }

                return extensionMap;
            }
        }
    }
}
