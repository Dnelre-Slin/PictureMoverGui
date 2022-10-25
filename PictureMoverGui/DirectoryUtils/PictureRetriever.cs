using MediaDevices;
using PictureMoverGui.Helpers;
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

        public bool IsValid { get; }

        public PictureRetriever(MediaTypeEnum mediaType, string source)
        {
            if (mediaType == MediaTypeEnum.NormalDirectory)
            {
                DirectoryInfo d = new DirectoryInfo(source);
                if (d.Exists)
                {
                    _genericFileInfoServer = new GenericFileInfoServer(d, null);
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
            }
            else if (mediaType == MediaTypeEnum.MediaDevice)
            {
                MediaDevice m = null;
                foreach (var dev in MediaDevice.GetDevices())
                {
                    if (dev.FriendlyName == source)
                    {
                        m = dev;
                        break;
                    }
                }
                if (m != null)
                {
                    _genericFileInfoServer = new GenericFileInfoServer(null, m);
                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
            }
            else
            {
                IsValid = false;
                throw new NotImplementedException($"PictureRetreiver constructor does not account for MediaType : {mediaType}");
            }
        }

        public void Dispose()
        {
            if (_genericFileInfoServer != null)
            {
                _genericFileInfoServer.Dispose();
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

        static private void tmpCatcher(Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e);
        }

        static public Dictionary<string, int> GetExtensions(MediaTypeEnum mediaType, string source, BackgroundWorker sender_worker)
        {
            return GetExtensions(mediaType, source, sender_worker, DateTime.MinValue);
        }

        static public Dictionary<string, int> GetExtensions(MediaTypeEnum mediaType, string source, BackgroundWorker sender_worker, DateTime newerThan)
        {
            //DirectoryInfo d = new DirectoryInfo(search_dir);
            using (PictureRetriever pictureRetriever = new PictureRetriever(mediaType, source))
            {
                if (!pictureRetriever.IsValid)
                {
                    return null;
                }

                Dictionary<string, int> extensionMap = new Dictionary<string, int>();

                foreach (GenericFileInfo file in pictureRetriever.EnumerateFiles("*", SearchOption.AllDirectories).CatchExceptions(tmpCatcher))
                {
                    if (file.LastWriteTime < newerThan)
                    {
                        continue; // Skip older files
                    }
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
