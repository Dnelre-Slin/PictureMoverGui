using PictureMoverGui.DirectoryUtils;
using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace PictureMoverGui.DirectoryWorkers
{
    //public class PictureMoverUiHandler
    //{

    //    private PictureMoverModel moverModel;

    //    private BackgroundWorker worker;

    //    public PictureMoverUiHandler(PictureMoverModel moverModel)
    //    {
    //        this.moverModel = moverModel;
    //        this.worker = null;
    //    }

    //    public void StartSorterButtonClick()
    //    {
    //        if (this.moverModel.AllowStartingMover)
    //        {
    //            this.moverModel.runningState = RunStates.DirectoryValidation;

    //            bool doCopy = false;
    //            bool doMakeStructures = false;
    //            bool doRename = false;
    //            string path_to_source = "";
    //            string path_to_destination = "";

    //            List<string> validExtensions = new List<string>();
    //            foreach (ExtensionInfo info in moverModel.extensionInfoList)
    //            {
    //                if (info.Active) // Count files that have extensions that are 'Active'
    //                {
    //                    validExtensions.Add(info.Name);
    //                }
    //            }

    //            worker = new BackgroundWorker();
    //            worker.WorkerReportsProgress = true;
    //            worker.WorkerSupportsCancellation = true;
    //            worker.DoWork += (obj, e) => worker_PictureMoverDoWork(obj, e, doCopy, doMakeStructures, doRename, path_to_source, path_to_destination, validExtensions);
    //            worker.ProgressChanged += worker_PictureMoverProgressChanged;
    //            worker.RunWorkerCompleted += worker_PictureMoverWorkDone;

    //            worker.RunWorkerAsync();

    //            this.moverModel.statusPercentage = 0;
    //        }
    //    }

    //    public void StartSorterButtonCancelClick()
    //    {
    //        if (this.worker != null)
    //        {
    //            this.worker.CancelAsync();
    //        }
    //    }

    //    static private void HandleFileAccessExceptions(Exception e)
    //    {
    //        System.Diagnostics.Trace.TraceError(e.Message);
    //    }

    //    static private bool IsValidFileExtension(string extension, List<string> validExtensions)
    //    {
    //        if (string.IsNullOrEmpty(extension))
    //        {
    //            return false; // Not a valid extension, if it has no extension.
    //        }
    //        string ext = extension.ToLower(); // To lower case. Example .JPEG -> .jpeg
    //        ext = ext.Substring(1);           // Remove leading '.'. Example: .jpeg -> jpeg
    //        if (validExtensions == null)
    //        {
    //            return true;
    //        }
    //        return validExtensions.Contains(ext);
    //    }

    //    static private bool IsFileNewerThan(DateTime fileLastWriteTime, DateTime newerThan)
    //    {
    //        return fileLastWriteTime >= newerThan;
    //    }

    //    private void worker_PictureMoverDoWork(object sender, DoWorkEventArgs e, bool doCopy, bool doMakeStructures, bool doRename, string path_to_source, string path_to_destination, List<string> validExtensions)
    //    {
    //        //System.Threading.Thread.Sleep(4000);
    //        //e.Result = 0;

    //        try
    //        {
    //            using (PictureRetriever pictureRetriever = new PictureRetriever(this.moverModel.sorterMediaType, this.moverModel.PictureRetrieverSource))
    //            {
    //                if (!pictureRetriever.IsValid)
    //                {
    //                    e.Result = new List<string>() { "Source dir no longer exists" };
    //                    return;
    //                }
    //                //DirectoryInfo d = new DirectoryInfo(this.moverModel.labelSourceDirContent);
    //                //if (!d.Exists)
    //                //{
    //                //    e.Result = new List<string>() { "Source dir no longer exists" };
    //                //    return;
    //                //}

    //                List<GenericFileInfo> fileInfoList = pictureRetriever.EnumerateFiles("*", SearchOption.AllDirectories)
    //                    .Where(f => IsValidFileExtension(f.Extension, validExtensions) && IsFileNewerThan(f.LastWriteTime, this.moverModel.PictureRetrieverNewerThan))
    //                    .CancelWorker(worker)
    //                    .CatchUnauthorizedAccessExceptions(HandleFileAccessExceptions)
    //                    .ToList();

    //                this.moverModel.runningState = RunStates.RunningSorter;

    //                if (worker.CancellationPending)
    //                {
    //                    e.Result = new List<string>() { "Cancelled during preparation" };
    //                    return;
    //                }

    //                //PictureMover pictureMover = new PictureMover(this.moverModel.labelSourceDirContent, this.moverModel.labelDestinationDirContent, this.moverModel.chkboxDoCopyChecked, sender as BackgroundWorker, this.moverModel.nrOfFilesInCurrentDir, this.moverModel.chkboxDoStructuredChecked, this.moverModel.chkboxDoRenameChecked, this.moverModel.validExtensionsInCurrentDir);
    //                PictureMover pictureMover = new PictureMover(this.moverModel, fileInfoList, sender as BackgroundWorker);
    //                List<string> infoStatusMessages = pictureMover.Mover();
    //                e.Result = infoStatusMessages;
    //                //int nrOfErrors = pictureMover.GetNrOfErrors();
    //                //e.Result = nrOfErrors;
    //            }
    //        }
    //        catch (Exception err)
    //        {
    //            System.Diagnostics.Trace.TraceError(err.Message);
    //        }
    //    }

    //    private void worker_PictureMoverProgressChanged(object sender, ProgressChangedEventArgs e)
    //    {
    //        this.moverModel.statusPercentage = e.ProgressPercentage;
    //    }

    //    private void worker_PictureMoverWorkDone(object sender, RunWorkerCompletedEventArgs e)
    //    {
    //        //int nrOfErrors = (int)e.Result;
    //        List<string> infoStatusMessages = e.Result as List<string>;

    //        if (infoStatusMessages != null && infoStatusMessages.Count == 1 && infoStatusMessages[0] == "Source dir no longer exists")
    //        {
    //            MessageBox.Show("The source dir no longer exists. Please start select source again", "Source dir no longer exists");
    //            this.moverModel.labelSourceDirContent = "";
    //        }

    //        this.moverModel.infoStatusMessagesLastRun = infoStatusMessages;
    //        this.moverModel.statusPercentage = 0;

    //        worker = null;
    //        this.moverModel.runningState = RunStates.Idle;
    //    }

    //}
}
