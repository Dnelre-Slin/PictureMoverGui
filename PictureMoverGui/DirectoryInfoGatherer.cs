using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace PictureMoverGui
{
    class DirectoryInfoGatherer
    {
        private string search_dir;
        private Dictionary<string, int> extensionMap;
        private BackgroundWorker sender_worker;

        private DirSearcher dirSearcher;

        public DirectoryInfoGatherer(string search_dir, BackgroundWorker sender_worker)
        {
            this.search_dir = search_dir;
            this.extensionMap = new Dictionary<string, int>();
            this.sender_worker = sender_worker;
        }

        public Dictionary<string, int> GatherInfo()
        {
            this.dirSearcher = new DirSearcher();

            DirectoryInfo d = new DirectoryInfo(this.search_dir);
            this.dirSearcher.DirSearch(d, GetExtension);

            return this.extensionMap;
        }

        private void GetExtension(DirectoryInfo d, FileInfo file)
        {
            if (sender_worker.CancellationPending)
            {
                this.dirSearcher.cancel = true;
                return;
            }

            string ext = file.Extension.ToLower(); // To lower case. Example .JPEG -> .jpeg
            ext = ext.Substring(1); // Remove leading '.'. Example: .jpeg -> jpeg
            if (extensionMap.ContainsKey(ext))
            {
                extensionMap[ext] += 1;
            }
            else
            {
                extensionMap[ext] = 1;
            }
        }
    }
}
