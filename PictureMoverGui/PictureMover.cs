using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PictureMoverGui
{
    class PictureMover
    {
        //private readonly PictureMoverModel moverModel;
        private BackgroundWorker worker_sender;

        private string sourceDir;
        private string destinationDir;
        private List<string> validExtensions;
        private int total_files;
        private bool doStructured;
        private bool doRename;
        private NameCollisionActionEnum nameCollisionAction;
        private CompareFilesActionEnum compareFilesAction;
        private List<SimpleEventData> eventData;

        private int nrOfErrors;
        private int current_progress;

        const int max_rename_tries = 100;

        Action<FileInfo, string, string> copyMoveAction;

        DirSearcher dirSearcher;

        public PictureMover(PictureMoverModel moverModel, BackgroundWorker worker_sender)
        {
            //this.moverModel = moverModel;
            this.worker_sender = worker_sender;

            this.sourceDir = moverModel.labelSourceDirContent;
            this.destinationDir = moverModel.labelDestinationDirContent;
            //this.validExtensions = new List<string>(moverModel.validExtensionsInCurrentDir); // Get copy of list
            //this.validExtensions = moverModel.validExtensionsInCurrentDir;
            //this.total_files = moverModel.nrOfFilesInCurrentDir > 0 ? moverModel.nrOfFilesInCurrentDir : 1; // To avoid division by zero issues
            this.doStructured = moverModel.chkboxDoStructuredChecked;
            this.doRename = moverModel.chkboxDoRenameChecked;
            this.nameCollisionAction = moverModel.nameCollisionAction;
            this.compareFilesAction = moverModel.compareFilesAction;
            this.eventData = Simplifiers.ToSimpleList(moverModel.eventDataList);

            this.nrOfErrors = 0;
            this.current_progress = 0;

            this.total_files = 0;
            this.validExtensions = new List<string>();
            foreach (ExtensionInfo info in moverModel.extensionInfoList)
            {
                if (info.Active) // Count files that have extensions that are 'Active'
                {
                    this.total_files += info.Amount;
                    this.validExtensions.Add(info.Name);
                }
            }
            this.total_files = total_files > 0 ? total_files : 1; // To avoid division by zero issues;

            dirSearcher = null;

            if (moverModel.chkboxDoCopyChecked)
            {
                this.copyMoveAction = this.DoCopyFile;
            }
            else
            {
                this.copyMoveAction = this.DoMoveFile;
            }
        }

        public void Mover()
        {
            Directory.CreateDirectory(this.destinationDir);
            DirectoryInfo d = new DirectoryInfo(this.sourceDir);

            this.current_progress = 0;

            dirSearcher = new DirSearcher(this.validExtensions);

            if (this.doStructured)
            {
                dirSearcher.DirSearch(d, DoStructuredCopyMoveFile);
            }
            else
            {
                dirSearcher.DirSearch(d, DoNormalCopyMoveFile);
            }
        }

        public int GetNrOfErrors()
        {
            return this.nrOfErrors;
        }

        private string GetNewFilename(FileInfo file, DirectoryInfo destinationDir)
        {

            string new_filename = file.Name;
            if (this.doRename) // Do date prefix renaming. Example: filename.png -> 20210304_filename.png
            {
                string date_str = file.LastWriteTime.ToString("yyyyMMdd");
                if (!file.Name.StartsWith(date_str))
                {
                    new_filename = date_str + "_" + new_filename;
                }
            }
            if (DirSearcher.FilenameInDir(destinationDir, new_filename)) // Rename to a int postfix, if name is already taken. Example: filename.png -> filename_1.png
            {
                string[] filename_extension_split = new_filename.Split(".");
                string fname = filename_extension_split[0];
                string extname = filename_extension_split[1];
                for (int i = 0; i < max_rename_tries; i++)
                {
                    string renamed_filename = $"{fname}_{i + 1}.{extname}";
                    if (!DirSearcher.FilenameInDir(destinationDir, renamed_filename))
                    {
                        Trace.TraceInformation($"Renamed {new_filename} to {renamed_filename}");
                        new_filename = renamed_filename;
                        break;
                    }
                }
            }
            return new_filename;
        }

        private void DoStructuredCopyMoveFile(DirectoryInfo d, FileInfo file)
        {
            DateTime dt = file.LastWriteTime;

            string monthName = char.ToUpper(dt.ToString("MMMM")[0]) + dt.ToString("MMMM").Substring(1);
            string monthNameAndDate = $"{dt.ToString("MM")} {monthName}";
            string thisDirectory = $"{this.destinationDir}\\{dt.Year}\\{monthNameAndDate}";

            DirectoryInfo destinationDir = Directory.CreateDirectory(thisDirectory);

            string new_filename = this.GetNewFilename(file, destinationDir);
            this.DoCopyMoveFile(file, thisDirectory, new_filename);
        }

        private void DoNormalCopyMoveFile(DirectoryInfo d, FileInfo file)
        {
            DirectoryInfo destinationDir = new DirectoryInfo(this.destinationDir);
            string new_filename = this.GetNewFilename(file, destinationDir);
            this.DoCopyMoveFile(file, this.destinationDir, new_filename);
        }

        private void DoCopyMoveFile(FileInfo file, string path_to_dir, string new_filename)
        {
            try
            {
                this.copyMoveAction(file, path_to_dir, new_filename);

                if (worker_sender.CancellationPending)
                {
                    this.dirSearcher.cancel = true;
                    return;
                }

                this.current_progress++;
                int progress_percent = (this.current_progress * 100) / this.total_files;

                this.worker_sender.ReportProgress(progress_percent);
            }
            catch (System.IO.IOException e)
            {
                //Skip copy / move
                this.nrOfErrors++;
                Trace.TraceError($"Copy/Move error: \"{e.Message}\". File name: \"{file.Name}\"");
            }
            catch (Exception e)
            {
                this.nrOfErrors++;
                Trace.TraceError(e.Message);
                throw;
            }
        }

        private void DoCopyFile(FileInfo file, string path_to_dir, string new_filename)
        {
            file.CopyTo($"{path_to_dir}\\{new_filename}", false);
        }
        private void DoMoveFile(FileInfo file, string path_to_dir, string new_filename)
        {
            file.MoveTo($"{path_to_dir}\\{new_filename}", false);
        }

    }
}
