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
        private BackgroundWorker worker_sender;

        private string sourceDir;
        private string destinationDir;
        private List<string> validExtensions;
        private int total_files;
        private NameCollisionActionEnum nameCollisionAction;
        private CompareFilesActionEnum compareFilesAction;
        private HashTypeEnum hashType;
        private List<SimpleEventData> eventDataList;

        private int nrOfErrors;
        private int current_progress;

        private Action<FileInfo, string, string> copyOrMoveTransferAction;
        private Func<FileInfo, DirectoryInfo> structuredOrDirectTransferAction;
        private Func<FileInfo, string> datePrependOrOriginalFilenameAction;

        private DirSearcher dirSearcher;
        private bool cancel;

        public PictureMover(PictureMoverModel moverModel, BackgroundWorker worker_sender)
        {
            this.worker_sender = worker_sender;

            this.sourceDir = moverModel.labelSourceDirContent;
            this.destinationDir = moverModel.labelDestinationDirContent;
            this.nameCollisionAction = moverModel.nameCollisionAction;
            this.compareFilesAction = moverModel.compareFilesAction;
            this.hashType = moverModel.hashTypeAction;
            this.eventDataList = Simplifiers.EventListToSimpleListValidOnly(moverModel.eventDataList);

            this.cancel = false;

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
                this.copyOrMoveTransferAction = this.DoCopyFile;
            }
            else
            {
                this.copyOrMoveTransferAction = this.DoMoveFile;
            }
            if (moverModel.chkboxDoStructuredChecked)
            {
                this.structuredOrDirectTransferAction = GetDestinationDirectoryStructured;
            }
            else
            {
                this.structuredOrDirectTransferAction = GetDestinationDirectoryDirect;
            }
            if (moverModel.chkboxDoRenameChecked)
            {
                this.datePrependOrOriginalFilenameAction = RenameFileDatePrepend;
            }
            else
            {
                this.datePrependOrOriginalFilenameAction = RenameFileOriginal;
            }
        }

        public void Mover()
        {
            Directory.CreateDirectory(this.destinationDir);
            DirectoryInfo d = new DirectoryInfo(this.sourceDir);

            this.current_progress = 0;

            dirSearcher = new DirSearcher(this.validExtensions);

            List<FileInfo> fileInfos = dirSearcher.GetAllFileInfosInDirectoryRecursively(d, this.total_files);

            foreach (FileInfo fileInfo in fileInfos)
            {
                DoStructuredOrDirectTransferFile(null, fileInfo);
                if (this.cancel)
                {
                    break;
                }
            }
        }

        public int GetNrOfErrors()
        {
            return this.nrOfErrors;
        }

        private void DoStructuredOrDirectTransferFile(DirectoryInfo _, FileInfo file)
        {
            DirectoryInfo destinationDir = GetDestinationDirectory(file);
            string destinationFilename = this.datePrependOrOriginalFilenameAction(file);
            FilenameCollisionRenamer filenameCollisionRenamer = new FilenameCollisionRenamer(this.nameCollisionAction, this.compareFilesAction, this.hashType, destinationDir, file, destinationFilename);
            if (filenameCollisionRenamer.IsValid()) // Only transfer file, if FilenameCollisionRenamer has allowed it
            {
                string validFilename = filenameCollisionRenamer.GetValidFilename();
                if (filenameCollisionRenamer.WasFileRenamed())
                {
                    Trace.TraceInformation($"Renamed {file.Name} to {validFilename}");
                }
                DoTransferFile(file, destinationDir.FullName, validFilename);
            }
            else
            {
                Trace.TraceInformation($"File: \"{file.Name}\" was not transferred");
            }
            UpdateWorker();
        }

        private void UpdateWorker()
        {
            if (worker_sender.CancellationPending)
            {
                this.dirSearcher.cancel = true;
                this.cancel = true;
                return;
            }

            this.current_progress++;
            int progress_percent = (this.current_progress * 100) / this.total_files;

            this.worker_sender.ReportProgress(progress_percent);
        }

        private string RenameFileOriginal(FileInfo file)
        {
            return file.Name;
        }

        private string RenameFileDatePrepend(FileInfo file)
        {
            string new_filename = file.Name;
            string date_str = file.LastWriteTime.ToString("yyyyMMdd");
            if (!file.Name.StartsWith(date_str)) // Do not rename, if it is already a date prepended filename
            {
                new_filename = date_str + "_" + new_filename;
            }
            return new_filename;
        }

        private DirectoryInfo GetDestinationDirectory(FileInfo file)
        {
            string eventName = "";
            if (FileInEvent(file, out eventName))
            {
                DirectoryInfo eventDirectory = Directory.CreateDirectory($"{this.destinationDir}\\{eventName}");
                return eventDirectory;
            }
            else
            {
                return this.structuredOrDirectTransferAction(file);
            }
        }

        private bool FileInEvent(FileInfo file, out string eventName)
        {
            eventName = "";
            long fileTick = file.LastWriteTime.Ticks;
            foreach (var evnt in this.eventDataList)
            {
                if (fileTick >= evnt.StartTick && fileTick <= evnt.EndTick)
                {
                    eventName = evnt.Name;
                    return true;
                }
            }
            return false;
        }

        private DirectoryInfo GetDestinationDirectoryStructured(FileInfo file)
        {
            DateTime dt = file.LastWriteTime;

            string monthName = char.ToUpper(dt.ToString("MMMM")[0]) + dt.ToString("MMMM").Substring(1);
            string monthNameAndDate = $"{dt.ToString("MM")} {monthName}";
            string thisDirectory = $"{this.destinationDir}\\{dt.Year}\\{monthNameAndDate}";

            DirectoryInfo destinationDir = Directory.CreateDirectory(thisDirectory);
            return destinationDir;
        }

        private DirectoryInfo GetDestinationDirectoryDirect(FileInfo file)
        {
            DirectoryInfo destinationDir = Directory.CreateDirectory(this.destinationDir);
            return destinationDir;
        }

        private void DoTransferFile(FileInfo file, string path_to_dir, string new_filename)
        {
            try
            {
                this.copyOrMoveTransferAction(file, path_to_dir, new_filename);
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
