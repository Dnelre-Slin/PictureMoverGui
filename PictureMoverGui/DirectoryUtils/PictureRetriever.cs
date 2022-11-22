using MediaDevices;
using PictureMoverGui.DirectoryWorkers;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace PictureMoverGui.DirectoryUtils
{
    public class PictureRetriever : IDisposable
    {
        private GenericFileInfoServer _genericFileInfoServer;

        public bool IsValid { get; }

        public PictureRetriever(MediaTypeEnum mediaType, string source, MediaDevice mediaDevice)
        {
            if (mediaType == MediaTypeEnum.NormalDirectory)
            {
                if (string.IsNullOrEmpty(source))
                {
                    IsValid = false;
                    return;
                }
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
                if (mediaDevice != null)
                {
                    _genericFileInfoServer = new GenericFileInfoServer(null, mediaDevice);
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
            return _genericFileInfoServer.EnumerateFiles().CatchExceptions(WorkerHelpers.HandleFileAccessExceptions);
        }

        public IEnumerable<GenericFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return _genericFileInfoServer.EnumerateFiles(searchPattern, searchOption).CatchExceptions(WorkerHelpers.HandleFileAccessExceptions);
        }

        static private void tmpCatcher(Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e);
        }

        static public Dictionary<string, int> GetExtensions(MediaTypeEnum mediaType, string source, MediaDevice mediaDevice, BaseWorker worker, Action<int> incrementInfoFileCount)
        {
            return GetExtensions(mediaType, source, mediaDevice, worker, incrementInfoFileCount, DateTime.MinValue);
        }

        static public Dictionary<string, int> GetExtensions(MediaTypeEnum mediaType, string source, MediaDevice mediaDevice, BaseWorker worker, Action<int> incrementInfoFileCount, DateTime newerThan)
        {
            using (PictureRetriever pictureRetriever = new PictureRetriever(mediaType, source, mediaDevice))
            {
                if (!pictureRetriever.IsValid)
                {
                    return null;
                }

                Dictionary<string, int> extensionMap = new Dictionary<string, int>();

                foreach (GenericFileInfo file in pictureRetriever.EnumerateFiles("*", SearchOption.AllDirectories)
                    .CatchExceptions(tmpCatcher)
                    .IncrementInfoFileCount(incrementInfoFileCount))
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

                    //worker.WorkSuspender.WaitOne(Timeout.Infinite); // Wait if suspended
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                }

                return extensionMap;
            }
        }
    }
}
