﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PictureMoverGui
{
    class PictureMoverUiHandler
    {
        private PictureMoverModel moverModel;

        private BackgroundWorker worker;

        public PictureMoverUiHandler(PictureMoverModel moverModel)
        {
            this.moverModel = moverModel;
            this.worker = null;
        }

        public void StartSorterButtonClick()
        {
            if (this.moverModel.AllowStartingMover)
            {
                this.moverModel.runningState = RunStates.RunningSorter;

                bool doCopy = false;
                bool doMakeStructures = false;
                bool doRename = false;
                string path_to_source = "";
                string path_to_destination = "";

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += (obj, e) => worker_PictureMoverDoWork(obj, e, doCopy, doMakeStructures, doRename, path_to_source, path_to_destination);
                worker.ProgressChanged += worker_PictureMoverProgressChanged;
                worker.RunWorkerCompleted += worker_PictureMoverWorkDone;

                worker.RunWorkerAsync();

                this.moverModel.statusPercentage = 0;
            }
        }

        public void StartSorterButtonCancelClick()
        {
            if (this.worker != null)
            {
                this.worker.CancelAsync();
            }
        }

        private void worker_PictureMoverDoWork(object sender, DoWorkEventArgs e, bool doCopy, bool doMakeStructures, bool doRename, string path_to_source, string path_to_destination)
        {
            try
            {
                PictureMover pictureMover = new PictureMover(this.moverModel, sender as BackgroundWorker);
                List<string> infoStatusMessages = pictureMover.Mover();
                e.Result = infoStatusMessages;
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError(err.Message);
            }
        }

        private void worker_PictureMoverProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.moverModel.statusPercentage = e.ProgressPercentage;
        }

        private void worker_PictureMoverWorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            List<string> infoStatusMessages = e.Result as List<string>;

            this.moverModel.infoStatusMessagesLastRun = infoStatusMessages;
            this.moverModel.statusPercentage = 0;

            worker = null;
            this.moverModel.runningState = RunStates.Idle;
        }

    }
}
