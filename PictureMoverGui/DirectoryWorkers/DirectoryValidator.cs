﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace PictureMoverGui
{
    class DirectoryValidator
    {
        private enum WorkResult
        {
            DirectoryNonExistent,
            DirectoryChanged,
            DirectoryNotChanged
        }

        private PictureMoverModel moverModel;
        private DirectorySelector directorySelector;
        private PictureMoverUiHandler pictureMoverUiHandler;

        public DirectoryValidator(PictureMoverModel moverModel, DirectorySelector directorySelector, PictureMoverUiHandler pictureMoverUiHandler)
        {
            this.moverModel = moverModel;
            this.directorySelector = directorySelector;
            this.pictureMoverUiHandler = pictureMoverUiHandler;
        }

        public void Run()
        {
            this.StartDirValidation();
        }

        private void StartDirValidation()
        {
            if (this.moverModel.sourceDirSat && this.moverModel.runningState == RunStates.Idle)
            {
                this.moverModel.runningState = RunStates.DirectoryValidation;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += worker_DirValidationDoWork;
                worker.RunWorkerCompleted += worker_DirValidationWorkDone;
                worker.RunWorkerAsync();
            }
        }

        private void worker_DirValidationDoWork(object sender, DoWorkEventArgs e)
        {
            //System.Threading.Thread.Sleep(4000);
            //e.Result = WorkResult.DirectoryChanged;

            try
            {
                DirectoryInfo d = new DirectoryInfo(this.moverModel.labelSourceDirContent);
                if (!d.Exists)
                {
                    e.Result = WorkResult.DirectoryNonExistent;
                }
                else
                {
                    bool sourceDirChanged = DirSearcher.DirLastWriteCompare(d, this.moverModel.lastSourceInfoGatherTime);
                    if (sourceDirChanged)
                    {
                        e.Result = WorkResult.DirectoryChanged;
                    }
                    else
                    {
                        e.Result = WorkResult.DirectoryNotChanged;
                    }
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.TraceError(err.Message);
            }
        }

        private void worker_DirValidationWorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            WorkResult result = (WorkResult)e.Result;

            this.moverModel.runningState = RunStates.Idle;

            switch(result)
            {
                case WorkResult.DirectoryNonExistent:
                    MessageBox.Show("The source dir no longer exists. Please start select source again", "Source dir no longer exists");
                    this.moverModel.labelSourceDirContent = "";
                    break;
                case WorkResult.DirectoryChanged:
                    this.directorySelector.RefreshSourceDir(DirSelectorCallback);
                    break;
                case WorkResult.DirectoryNotChanged:
                    this.pictureMoverUiHandler.StartSorterButtonClick();
                    break;
            }
        }

        private void DirSelectorCallback()
        {
            this.pictureMoverUiHandler.StartSorterButtonClick();
        }
    }
}