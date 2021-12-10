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
        private string path_to_source;
        private string path_to_destination;
        private int nrOfErrors;
        private BackgroundWorker worker_sender;
        private int current_progress;
        private int total_files;
        private bool doStructured;
        private bool doRename;
        private List<string> validExtensions;

        const int max_rename_tries = 100;

        Action<FileInfo, string, string> copyMoveAction;

        DirSearcher dirSearcher;

        public PictureMover(string path_to_source, string path_to_destination, bool doCopy, BackgroundWorker worker_sender, int total_files, bool doStructured, bool doRename, List<string> validExtensions)
        {
            this.path_to_source = path_to_source;
            this.path_to_destination = path_to_destination;
            this.nrOfErrors = 0;
            this.worker_sender = worker_sender;
            this.current_progress = 0;
            this.total_files = total_files > 0 ? total_files : 1; // To avoid division by zero issues
            this.doStructured = doStructured;
            this.doRename = doRename;
            this.validExtensions = validExtensions;
            dirSearcher = null;

            if (doCopy)
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
            Directory.CreateDirectory(path_to_destination);
            DirectoryInfo d = new DirectoryInfo(path_to_source);

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
            string thisDirectory = $"{path_to_destination}\\{dt.Year}\\{monthNameAndDate}";

            DirectoryInfo destinationDir = Directory.CreateDirectory(thisDirectory);

            string new_filename = this.GetNewFilename(file, destinationDir);
            this.DoCopyMoveFile(file, thisDirectory, new_filename);
        }

        private void DoNormalCopyMoveFile(DirectoryInfo d, FileInfo file)
        {
            DirectoryInfo destinationDir = new DirectoryInfo(path_to_destination);
            string new_filename = this.GetNewFilename(file, destinationDir);
            this.DoCopyMoveFile(file, path_to_destination, new_filename);
        }

        private void DoCopyMoveFile(FileInfo file, string path_to_dir, string new_filename)
        {
            try
            {
                if (worker_sender.CancellationPending)
                {
                    this.dirSearcher.cancel = true;
                    return;
                }

                this.copyMoveAction(file, path_to_dir, new_filename);

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
