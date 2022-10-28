﻿using MediaDevices;
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
        private BackgroundWorker _workerSender;
        private List<GenericFileInfo> _fileInfoList;

        private string _destinationPath;
        private NameCollisionActionEnum _nameCollisionAction;
        private CompareFilesActionEnum _compareFilesAction;
        private HashTypeEnum _hashType;
        private List<EventDataModel> _eventDataList;

        private int _totalFiles;

        private int _nrOfErrors;
        private int _currentProgress;

        private Action<GenericFileInfo, string, string> _copyOrMoveTransferAction;
        private Func<GenericFileInfo, DirectoryInfo> _structuredOrDirectTransferAction;
        private Func<GenericFileInfo, string> _datePrependOrOriginalFilenameAction;

        private Action<string> _addRunStatusLog;

        private bool _cancel;

        public PictureMover(PictureMoverArguments pictureMoverArguments, List<GenericFileInfo> fileInfoList, BackgroundWorker workerSender)
        {
            _workerSender = workerSender;
            _fileInfoList = fileInfoList;

            _destinationPath = pictureMoverArguments.DestinationPath;
            _nameCollisionAction = pictureMoverArguments.NameCollisionAction;
            _compareFilesAction = pictureMoverArguments.CompareFilesAction;
            _hashType = pictureMoverArguments.HashTypeAction;
            _eventDataList = pictureMoverArguments.EventDataList;
            _addRunStatusLog = pictureMoverArguments.AddRunStatusLog;

            _cancel = false;

            _nrOfErrors = 0;
            _currentProgress = 0;

            _totalFiles = fileInfoList.Count;

            if (pictureMoverArguments.DoCopy)
            {
                _copyOrMoveTransferAction = this.DoCopyFile;
            }
            else
            {
                _copyOrMoveTransferAction = this.DoMoveFile;
            }
            if (pictureMoverArguments.DoMakeStructured)
            {
                _structuredOrDirectTransferAction = GetDestinationDirectoryStructured;
            }
            else
            {
                _structuredOrDirectTransferAction = GetDestinationDirectoryDirect;
            }
            if (pictureMoverArguments.DoRename)
            {
                _datePrependOrOriginalFilenameAction = RenameFileDatePrepend;
            }
            else
            {
                _datePrependOrOriginalFilenameAction = RenameFileOriginal;
            }
        }

        public int Mover()
        {
            Directory.CreateDirectory(_destinationPath);

            _currentProgress = 0;

            foreach (GenericFileInfo fileInfo in _fileInfoList)
            {
                DoStructuredOrDirectTransferFile(fileInfo);
                if (_cancel)
                {
                    _addRunStatusLog?.Invoke($"Cancelled during sorting");
                    break;
                }
            }

            return GetNrOfErrors();
        }

        public int GetNrOfErrors()
        {
            return _nrOfErrors;
        }

        private void DoStructuredOrDirectTransferFile(GenericFileInfo file)
        {
            DirectoryInfo destinationDir = GetDestinationDirectory(file);
            string destinationFilename = _datePrependOrOriginalFilenameAction(file);
            FilenameCollisionRenamer filenameCollisionRenamer = new FilenameCollisionRenamer(
                _nameCollisionAction, 
                _compareFilesAction, 
                _hashType, 
                destinationDir, 
                file, 
                destinationFilename);
            
            if (filenameCollisionRenamer.IsValid()) // Only transfer file, if FilenameCollisionRenamer has allowed it
            {
                string validFilename = filenameCollisionRenamer.GetValidFilename();
                if (filenameCollisionRenamer.WasFileRenamed())
                {
                    _addRunStatusLog?.Invoke($"Renamed {file.Name} to {validFilename}");
                }
                DoTransferFile(file, destinationDir.FullName, validFilename);
            }
            else
            {
                _addRunStatusLog?.Invoke($"File: \"{file.Name}\" was not transferred");
            }
            UpdateWorker();
        }

        private void UpdateWorker()
        {
            if (_workerSender.CancellationPending)
            {
                //this.dirSearcher.cancel = true;
                _cancel = true;
                return;
            }

            _currentProgress++;
            int progressPercent = (_currentProgress * 100) / _totalFiles;

            _workerSender.ReportProgress(progressPercent);
        }

        private string RenameFileOriginal(GenericFileInfo file)
        {
            return file.Name;
        }

        private string RenameFileDatePrepend(GenericFileInfo file)
        {
            string newFilename = file.Name;
            string dateStr = file.LastWriteTime.ToString("yyyyMMdd_HHmmss");
            if (!file.Name.StartsWith(dateStr)) // Do not rename, if it is already a date prepended filename
            {
                newFilename = dateStr + "_" + newFilename;
            }
            return newFilename;
        }

        private DirectoryInfo GetDestinationDirectory(GenericFileInfo file)
        {
            string eventName = "";
            if (FileInEvent(file, out eventName))
            {
                DirectoryInfo eventDirectory = Directory.CreateDirectory($"{_destinationPath}\\{eventName}");
                return eventDirectory;
            }
            else
            {
                return _structuredOrDirectTransferAction(file);
            }
        }

        private bool FileInEvent(GenericFileInfo file, out string eventName)
        {
            eventName = "";
            long fileTick = file.LastWriteTime.Ticks;
            foreach (var evnt in _eventDataList)
            {
                if (fileTick >= evnt.StartTime.Ticks && fileTick <= evnt.EndTime.Ticks)
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
            string thisDirectory = $"{_destinationPath}\\{dt.Year}\\{monthNameAndDate}";

            DirectoryInfo destinationDir = Directory.CreateDirectory(thisDirectory);
            return destinationDir;
        }

        private DirectoryInfo GetDestinationDirectoryDirect(GenericFileInfo _)
        {
            DirectoryInfo destinationDir = Directory.CreateDirectory(_destinationPath);
            return destinationDir;
        }

        private void DoTransferFile(GenericFileInfo file, string pathToDir, string newFilename)
        {
            try
            {
                _copyOrMoveTransferAction(file, pathToDir, newFilename);
            }
            catch (System.IO.IOException e)
            {
                //Skip copy / move
                _nrOfErrors++;
                Trace.TraceError($"Copy/Move error: \"{e.Message}\". File name: \"{file.Name}\"");
            }
            catch (Exception e)
            {
                _nrOfErrors++;
                Trace.TraceError(e.Message);
                throw;
            }
        }

        private void DoCopyFile(GenericFileInfo file, string pathToDir, string newFilename)
        {
            file.CopyTo($"{pathToDir}\\{newFilename}", false);
        }
        private void DoMoveFile(GenericFileInfo file, string pathToDir, string newFilename)
        {
            file.MoveTo($"{pathToDir}\\{newFilename}", false);
        }

    }
}
