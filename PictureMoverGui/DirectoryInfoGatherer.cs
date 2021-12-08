using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PictureMoverGui
{
    class DirectoryInfoGatherer
    {
        private string search_dir;
        private Dictionary<string, int> extensionMap;

        public DirectoryInfoGatherer(string search_dir)
        {
            this.search_dir = search_dir;
            this.extensionMap = new Dictionary<string, int>();
        }

        public Dictionary<string, int> GatherInfo()
        {
            DirectoryInfo d = new DirectoryInfo(this.search_dir);
            DirSearcher.DirSearch(d, GetExtension);

            return this.extensionMap;
        }

        private void GetExtension(DirectoryInfo d, FileInfo file)
        {
            string ext = file.Extension.ToLower();
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
