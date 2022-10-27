using MediaDevices;
using PictureMoverGui.DirectoryUtils;
using PictureMoverGui.Helpers;
using PictureMoverGui.Helpers.HelperClasses;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PictureMoverGui.DirectoryUtils
{
    public class PictureMover
    {
        //private readonly PictureMoverModel moverModel;
        private BackgroundWorker worker_sender;
        private List<GenericFileInfo> fileInfoList;

        private string destinationPath;
        //private List<string> validExtensions;
        private int total_files;
        //private bool doStructured;
        //private bool doRename;
        private NameCollisionActionEnum nameCollisionAction;
        private CompareFilesActionEnum compareFilesAction;
        private HashTypeEnum hashType;
        private List<SimpleEventData> eventDataList;

        private int nrOfErrors;
        private int current_progress;

        private Action<GenericFileInfo, string, string> copyOrMoveTransferAction;
        private Func<GenericFileInfo, DirectoryInfo> structuredOrDirectTransferAction;
        private Func<GenericFileInfo, string> datePrependOrOriginalFilenameAction;

        //private List<string> infoStatusMessages;
        private Action<string> _addRunStatusLog;

        private bool cancel;

        public PictureMover(PictureMoverArguments pictureMoverArguments, List<GenericFileInfo> fileInfoList, BackgroundWorker worker_sender)
        {
            //this.moverModel = moverModel;
            this.worker_sender = worker_sender;
            this.fileInfoList = fileInfoList;

            this.destinationPath = pictureMoverArguments.DestinationPath;
            //this.validExtensions = new List<string>(moverModel.validExtensionsInCurrentDir); // Get copy of list
            //this.validExtensions = moverModel.validExtensionsInCurrentDir;
            //this.total_files = moverModel.nrOfFilesInCurrentDir > 0 ? moverModel.nrOfFilesInCurrentDir : 1; // To avoid division by zero issues
            //this.doStructured = moverModel.chkboxDoStructuredChecked;
            //this.doRename = moverModel.chkboxDoRenameChecked;
            this.nameCollisionAction = pictureMoverArguments.NameCollisionAction;
            this.compareFilesAction = pictureMoverArguments.CompareFilesAction;
            this.hashType = pictureMoverArguments.HashTypeAction;
            //this.eventDataList = Simplifiers.EventListToSimpleListValidOnly(moverModel.eventDataList);
            this.eventDataList = pictureMoverArguments.EventDataList;
            //this.infoStatusMessages = new List<string>();
            _addRunStatusLog = pictureMoverArguments.AddRunStatusLog;

            this.cancel = false;

            this.nrOfErrors = 0;
            this.current_progress = 0;

            this.total_files = fileInfoList.Count;
            this.total_files = total_files > 0 ? total_files : 1; // To avoid division by zero issues;

            if (pictureMoverArguments.DoCopy)
            {
                this.copyOrMoveTransferAction = this.DoCopyFile;
            }
            else
            {
                this.copyOrMoveTransferAction = this.DoMoveFile;
            }
            if (pictureMoverArguments.DoMakeStructured)
            {
                this.structuredOrDirectTransferAction = GetDestinationDirectoryStructured;
            }
            else
            {
                this.structuredOrDirectTransferAction = GetDestinationDirectoryDirect;
            }
            if (pictureMoverArguments.DoRename)
            {
                this.datePrependOrOriginalFilenameAction = RenameFileDatePrepend;
            }
            else
            {
                this.datePrependOrOriginalFilenameAction = RenameFileOriginal;
            }
        }

        public int Mover()
        {
            Directory.CreateDirectory(this.destinationPath);

            this.current_progress = 0;

            foreach (GenericFileInfo fileInfo in fileInfoList)
            {
                DoStructuredOrDirectTransferFile(fileInfo);
                if (this.cancel)
                {
                    _addRunStatusLog?.Invoke($"Cancelled during sorting");
                    break;
                }
            }

            return GetNrOfErrors();
        }

        public int GetNrOfErrors()
        {
            return this.nrOfErrors;
        }

        private void DoStructuredOrDirectTransferFile(GenericFileInfo file)
        {
            //DirectoryInfo destinationDir = this.structuredOrDirectTransferAction(file);
            DirectoryInfo destinationDir = GetDestinationDirectory(file);
            string destinationFilename = this.datePrependOrOriginalFilenameAction(file);
            FilenameCollisionRenamer filenameCollisionRenamer = new FilenameCollisionRenamer(this.nameCollisionAction, this.compareFilesAction, this.hashType, destinationDir, file, destinationFilename);
            //if (this.CheckAllowedFilename(destinationDir, destinationFilename, out destinationFilename)) // Only transfer file, if CheckAllowedFilename has allowed it
            if (filenameCollisionRenamer.IsValid()) // Only transfer file, if FilenameCollisionRenamer has allowed it
            {
                string validFilename = filenameCollisionRenamer.GetValidFilename();
                if (filenameCollisionRenamer.WasFileRenamed())
                {
                    _addRunStatusLog?.Invoke($"Renamed {file.Name} to {validFilename}");
                    //this.infoStatusMessages.Add($"Renamed {file.Name} to {validFilename}");
                    //Trace.TraceInformation($"Renamed {file.Name} to {validFilename}");
                }
                DoTransferFile(file, destinationDir.FullName, validFilename);
            }
            else
            {
                _addRunStatusLog?.Invoke($"File: \"{file.Name}\" was not transferred");
                //this.infoStatusMessages.Add($"File: \"{file.Name}\" was not transferred");
                //Trace.TraceInformation($"File: \"{file.Name}\" was not transferred");
            }
            UpdateWorker();
        }

        private void UpdateWorker()
        {
            if (worker_sender.CancellationPending)
            {
                //this.dirSearcher.cancel = true;
                this.cancel = true;
                return;
            }

            this.current_progress++;
            int progress_percent = (this.current_progress * 100) / this.total_files;

            this.worker_sender.ReportProgress(progress_percent);
        }

        private string RenameFileOriginal(GenericFileInfo file)
        {
            return file.Name;
        }

        private string RenameFileDatePrepend(GenericFileInfo file)
        {
            string new_filename = file.Name;
            string date_str = file.LastWriteTime.ToString("yyyyMMdd_HHmmss");
            if (!file.Name.StartsWith(date_str)) // Do not rename, if it is already a date prepended filename
            {
                new_filename = date_str + "_" + new_filename;
            }
            return new_filename;
        }

        private DirectoryInfo GetDestinationDirectory(GenericFileInfo file)
        {
            string eventName = "";
            if (FileInEvent(file, out eventName))
            {
                DirectoryInfo eventDirectory = Directory.CreateDirectory($"{this.destinationPath}\\{eventName}");
                return eventDirectory;
            }
            else
            {
                return this.structuredOrDirectTransferAction(file);
            }
        }

        private bool FileInEvent(GenericFileInfo file, out string eventName)
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

        private DirectoryInfo GetDestinationDirectoryStructured(GenericFileInfo file)
        {
            DateTime dt = file.LastWriteTime;

            string monthName = char.ToUpper(dt.ToString("MMMM")[0]) + dt.ToString("MMMM").Substring(1);
            string monthNameAndDate = $"{dt.ToString("MM")} {monthName}";
            string thisDirectory = $"{this.destinationPath}\\{dt.Year}\\{monthNameAndDate}";

            DirectoryInfo destinationDir = Directory.CreateDirectory(thisDirectory);
            return destinationDir;

            //string new_filename = this.GetNewFilename(file, destinationDir);
            //this.DoCopyMoveFile(file, thisDirectory, new_filename);
        }

        private DirectoryInfo GetDestinationDirectoryDirect(GenericFileInfo _)
        {
            //DirectoryInfo destinationDir = new DirectoryInfo(this.destinationDir);
            DirectoryInfo destinationDir = Directory.CreateDirectory(this.destinationPath);
            return destinationDir;
            //string new_filename = this.GetNewFilename(file, destinationDir);
            //this.DoCopyMoveFile(file, this.destinationDir, new_filename);
        }

        private void DoTransferFile(GenericFileInfo file, string path_to_dir, string new_filename)
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

        private void DoCopyFile(GenericFileInfo file, string path_to_dir, string new_filename)
        {
            file.CopyTo($"{path_to_dir}\\{new_filename}", false);
        }
        private void DoMoveFile(GenericFileInfo file, string path_to_dir, string new_filename)
        {
            file.MoveTo($"{path_to_dir}\\{new_filename}", false);
        }

    }
}
