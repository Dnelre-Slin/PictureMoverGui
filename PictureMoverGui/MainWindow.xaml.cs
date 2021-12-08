using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace PictureMoverGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public bool AllowSwapOperation
        //{
        //    get { return this.sourceDirSat && this.destinationDirSat; }
        //}
        PictureMoverModel moverModel;
        DirectorySelector directorySelector;
        PictureMoverUiHandler moverUiHandler;

        //DispatcherTimer statusMessageTimer = new DispatcherTimer();
        //int nrOfErrors = 0;
        //Dictionary<string, int> extensionMapInCurrentDir = new Dictionary<string, int>();
        //int nrOfFilesInCurrentDir = 0;

        //bool pictureMoverRunning = false;
        //bool gatherDirInfoRunning = false;

        //bool sourceDirSat = false;
        //bool destinationDirSat = false;

        //public bool sourceDirSat
        //{
        //    get { return _sourceDirSat; }
        //    set
        //    {
        //        _sourceDirSat = value;
        //        OnPropertyChanged("sourceDirSat");
        //        OnPropertyChanged("AllowSwapOperation");
        //    }
        //}

        public MainWindow()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("MyTextFile.log"));
            Trace.AutoFlush = true;

            InitializeComponent();
            this.moverModel = new PictureMoverModel();
            this.DataContext = this.moverModel;

            this.directorySelector = new DirectorySelector(this.moverModel, UnsortedDirLabel, SortedDirLabel);
            this.directorySelector.StartUp();

            this.moverUiHandler = new PictureMoverUiHandler(this.moverModel, MakeCopyCheckbox, ReverseCheckbox, RenameCheckbox, UnsortedDirLabel, SortedDirLabel, statusMessage, ProgressBarArc);
            //UnsortedDirLabel.Content = Properties.Settings.Default.UnsortedDir;
            //SortedDirLabel.Content = Properties.Settings.Default.SortedDir;
            //string start_source_dir = Properties.Settings.Default.UnsortedDir;
            //string start_destination_dir = Properties.Settings.Default.SortedDir;
            //Trace.TraceInformation(start_destination_dir);
            //if (!string.IsNullOrEmpty(start_source_dir) && new DirectoryInfo(start_source_dir).Exists)
            //{
            //    SetFromDir(start_source_dir);
            //}
            //if (!string.IsNullOrEmpty(start_destination_dir) && new DirectoryInfo(start_destination_dir).Exists)
            //{
            //    SetToDir(start_destination_dir);
            //}
            //if (!this.moverModel.sourceDirSat || !this.moverModel.destinationDirSat)
            //{
            //    btnStart.IsEnabled = false;
            //}
            //statusMessageTimer.Tick += UpdateStatusMessage;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            this.directorySelector.ChooseSourceButtonClick();
        }

        private void btnOpenFile2_Click(object sender, RoutedEventArgs e)
        {
            this.directorySelector.ChooseDestinationButtonClick();
        }

        private void btnSwapToFrom_Click(object sender, RoutedEventArgs e)
        {
            this.directorySelector.SwapSourceDestinationButtonClick();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.moverUiHandler.StartSorterButtonClick();
        }

        //private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Forms.FolderBrowserDialog openFileDlg  = new System.Windows.Forms.FolderBrowserDialog();
        //    var result = openFileDlg.ShowDialog();
        //    if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
        //    {
        //        this.SetFromDir(openFileDlg.SelectedPath);

        //        //UnsortedDirLabel.Content = openFileDlg.SelectedPath;
        //        //Properties.Settings.Default.UnsortedDir = openFileDlg.SelectedPath;
        //        //Properties.Settings.Default.Save();
        //        //this.StartDirGathering();

        //        //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        //        //txtEditor.Text = result.ToString();
        //        //txtEditor.Text = openFileDlg.SelectedPath;
        //    }
        //}

        //private void SetFromDir(string path_to_dir)
        //{
        //    UnsortedDirLabel.Content = path_to_dir;
        //    Properties.Settings.Default.UnsortedDir = path_to_dir;
        //    Properties.Settings.Default.Save();
        //    this.moverModel.sourceDirSat = true;
        //    this.StartDirGathering();
        //}

        //private void StartDirGathering()
        //{
        //    if (!this.gatherDirInfoRunning)
        //    {
        //        this.gatherDirInfoRunning = true;
        //        btnOpenFile.IsEnabled = false;

        //        //BtnSwapToFrom2.IsEnabled = false;

        //        this.pictureMoverRunning = true;
        //        btnStart.IsEnabled = false;

        //        string search_dir = Properties.Settings.Default.UnsortedDir;

        //        BackgroundWorker worker = new BackgroundWorker();
        //        //worker.WorkerReportsProgress = true;
        //        worker.DoWork += (obj, e) => worker_DirGathererDoWork(obj, e, search_dir);
        //        //worker.ProgressChanged += worker_PictureMoverProgressChanged;
        //        worker.RunWorkerCompleted += worker_DirGathererWorkDone;
        //        worker.RunWorkerAsync();
        //    }
        //}

        //private void worker_DirGathererDoWork(object sender, DoWorkEventArgs e, string search_dir)
        //{
        //    DirectoryInfoGatherer directoryInfoGatherer = new DirectoryInfoGatherer(search_dir);
        //    Dictionary<string ,int> extensionInfo = directoryInfoGatherer.GatherInfo();
        //    e.Result = extensionInfo;
        //}

        //private void worker_DirGathererWorkDone(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    Dictionary<string, int> extensionInfo = e.Result as Dictionary<string, int>;
        //    this.extensionMapInCurrentDir = extensionInfo;
        //    this.nrOfFilesInCurrentDir = 0;
        //    foreach (var item in extensionInfo)
        //    {
        //        this.nrOfFilesInCurrentDir += item.Value;
        //        //Trace.TraceInformation($"{item.Key}: {item.Value}");
        //    }

        //    if (this.moverModel.sourceDirSat && this.moverModel.destinationDirSat)
        //    {
        //        btnStart.IsEnabled = true;
        //    }
        //    this.pictureMoverRunning = false;

        //    //BtnSwapToFrom2.IsEnabled = true;

        //    btnOpenFile.IsEnabled = true;
        //    this.gatherDirInfoRunning = false;
        //}

        //private void btnOpenFile2_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Forms.FolderBrowserDialog openFileDlg  = new System.Windows.Forms.FolderBrowserDialog();
        //    var result = openFileDlg.ShowDialog();
        //    if (result.ToString() != string.Empty && openFileDlg.SelectedPath != string.Empty)
        //    {
        //        this.SetToDir(openFileDlg.SelectedPath);

        //        //SortedDirLabel.Content = openFileDlg.SelectedPath;
        //        //Properties.Settings.Default.SortedDir = openFileDlg.SelectedPath;
        //        //Properties.Settings.Default.Save();

        //        //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        //        //txtEditor.Text = result.ToString();
        //        //txtEditor.Text = openFileDlg.SelectedPath;
        //    }
        //}

        //private void SetToDir(string path_to_dir)
        //{
        //    SortedDirLabel.Content = path_to_dir;
        //    Properties.Settings.Default.SortedDir = path_to_dir;
        //    Properties.Settings.Default.Save();
        //    this.moverModel.destinationDirSat = true;
        //}

        //private void btnSwapToFrom_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (this.sourceDirSat && this.destinationDirSat)
        //    if (this.moverModel.AllowSwapOperation)
        //    {
        //        string new_from_dir = SortedDirLabel.Content.ToString();
        //        string new_to_dir = UnsortedDirLabel.Content.ToString();
        //        SetFromDir(new_from_dir);
        //        SetToDir(new_to_dir);
        //    }
        //}

        //private void btnStart_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!this.moverModel.sourceDirSat || !this.moverModel.destinationDirSat)
        //    {
        //        btnStart.IsEnabled = false;
        //        return;
        //    }
        //    if (!this.pictureMoverRunning)
        //    {
        //        this.pictureMoverRunning = true;
        //        btnStart.IsEnabled = false;

        //        bool isReversed = ReverseCheckbox.IsChecked.HasValue && ReverseCheckbox.IsChecked.Value;
        //        bool doCopy = MakeCopyCheckbox.IsChecked.HasValue && MakeCopyCheckbox.IsChecked.Value;
        //        bool doRename = RenameCheckbox.IsChecked.HasValue && RenameCheckbox.IsChecked.Value;
        //        string path_to_unsorted = UnsortedDirLabel.Content.ToString();
        //        string path_to_sorted = SortedDirLabel.Content.ToString();

        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.WorkerReportsProgress = true;
        //        worker.DoWork += (obj, e) => worker_PictureMoverDoWork(obj, e, isReversed, path_to_unsorted, path_to_sorted, doCopy, doRename);
        //        worker.ProgressChanged += worker_PictureMoverProgressChanged;
        //        worker.RunWorkerCompleted += worker_PictureMoverWorkDone;

        //        worker.RunWorkerAsync();

        //        statusMessage.Content = "0%";
        //        //if (ReverseCheckbox.IsChecked.HasValue && ReverseCheckbox.IsChecked.Value)
        //        //{
        //        //    this.FileSorterReversed();
        //        //}
        //        //else
        //        //{
        //        //    this.FileSorter();
        //        //}
        //        //if (nrOfErrors > 0)
        //        //{
        //        //    statusMessage.Content = App.Current.FindResource("ErrorStatusMessage").ToString() + " " + nrOfErrors;
        //        //    statusMessage.Foreground = Brushes.Red;
        //        //}
        //        //else
        //        //{
        //        //    statusMessage.Content = App.Current.FindResource("DoneStatusMessage");
        //        //    statusMessage.Foreground = Brushes.Green;
        //        //}

        //        //statusMessageTimer.Interval = TimeSpan.FromSeconds((double)App.Current.FindResource("DoneStatusMessageTime"));
        //        //statusMessageTimer.Start();
        //    }
        //}

        //private void worker_PictureMoverDoWork(object sender, DoWorkEventArgs e, bool doReverse, string path_to_unsorted, string path_to_sorted, bool doCopy, bool doRename)
        //{
        //    PictureMover pictureMover = new PictureMover(path_to_unsorted, path_to_sorted, doCopy, sender as BackgroundWorker, this.moverModel.nrOfFilesInCurrentDir, doReverse, doRename);
        //    pictureMover.Mover();
        //    //if (doReverse)
        //    //{
        //    //    pictureMover.FileSorterReversed();
        //    //}
        //    //else
        //    //{
        //    //    pictureMover.FileSorter();
        //    //}
        //}

        //private void worker_PictureMoverProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    int progress = e.ProgressPercentage;
        //    statusMessage.Content = $"{progress}%";
        //    ProgressBarArc.EndAngle = progress * 3.6;
        //}

        //private void worker_PictureMoverWorkDone(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (nrOfErrors > 0)
        //    {
        //        statusMessage.Content = App.Current.FindResource("ErrorStatusMessage").ToString() + " " + nrOfErrors;
        //        statusMessage.Foreground = Brushes.Red;
        //    }
        //    else
        //    {
        //        statusMessage.Content = App.Current.FindResource("DoneStatusMessage");
        //        statusMessage.Foreground = Brushes.Green;
        //    }

        //    ProgressBarArc.EndAngle = 0;

        //    statusMessageTimer.Interval = TimeSpan.FromSeconds((double)App.Current.FindResource("DoneStatusMessageTime"));
        //    statusMessageTimer.Start();

        //    btnStart.IsEnabled = true;
        //    this.pictureMoverRunning = false;
        //}

        //private void UpdateStatusMessage(object sender, EventArgs e)
        //{
        //    statusMessage.Content = App.Current.FindResource("ReadyStatusMessage");
        //    statusMessage.Foreground = Brushes.Black;

        //    statusMessageTimer.Stop();
        //}

        //    private void FileSorter()
        //    {
        //        string path_to_unsorted = UnsortedDirLabel.Content.ToString();
        //        string path_to_sorted = SortedDirLabel.Content.ToString();

        //        Directory.CreateDirectory(path_to_sorted);
        //        DirectoryInfo d = new DirectoryInfo(path_to_unsorted);
        //        foreach (FileInfo file in d.GetFiles())
        //        {
        //            DateTime dt = file.LastWriteTime;

        //            string monthName = char.ToUpper(dt.ToString("MMMM")[0]) + dt.ToString("MMMM").Substring(1);
        //            string monthNameAndDate = $"{dt.ToString("MM")} {monthName}";
        //            string thisDirectory = $"{path_to_sorted}\\{dt.Year}\\{monthNameAndDate}";

        //            Directory.CreateDirectory(thisDirectory);


        //            //if (MakeCopyCheckbox.IsChecked.HasValue && MakeCopyCheckbox.IsChecked.Value) // Copy file
        //            //{
        //            //    file.CopyTo($"{thisDirectory}\\{file.Name}");
        //            //}
        //            //else // Move it instead of copying
        //            //{
        //            //    file.MoveTo($"{thisDirectory}\\{file.Name}");
        //            //}
        //            this.DoCopyMoveOperation(file, thisDirectory, MakeCopyCheckbox.IsChecked.HasValue && MakeCopyCheckbox.IsChecked.Value);
        //        }
        //    }

        //    private void FileSorterReversed()
        //    {
        //        string path_to_unsorted = UnsortedDirLabel.Content.ToString();
        //        string path_to_sorted = SortedDirLabel.Content.ToString();

        //        Directory.CreateDirectory(path_to_unsorted);
        //        DirectoryInfo d = new DirectoryInfo(path_to_sorted);
        //        this.DirSearch(d, path_to_unsorted);
        //    }

        //    private void DirSearch(DirectoryInfo d, string path_to_unsorted)
        //    {
        //        foreach (FileInfo file in d.GetFiles())
        //        {
        //            //try
        //            //{
        //                if (file.Extension != ".ini" && file.Extension != ".db")
        //                {
        //                    //Console.WriteLine(file.Name);
        //                    //if (MakeCopyCheckbox.IsChecked.HasValue && MakeCopyCheckbox.IsChecked.Value) // Copy file
        //                    //{
        //                    //    file.CopyTo($"{path_to_unsorted}\\{file.Name}", false);
        //                    //}
        //                    //else // Move it instead of copying
        //                    //{
        //                    //    file.MoveTo($"{path_to_unsorted}\\{file.Name}", false);
        //                    //}
        //                    this.DoCopyMoveOperation(file, path_to_unsorted, MakeCopyCheckbox.IsChecked.HasValue && MakeCopyCheckbox.IsChecked.Value);
        //                }
        //            //}
        //            //catch (System.IO.IOException e)
        //            //{
        //            //    //Skip copy / move
        //            //    Trace.TraceError(e.Message);
        //            //}
        //        }
        //        foreach (DirectoryInfo subD in d.GetDirectories())
        //        {
        //            DirSearch(subD, path_to_unsorted);
        //        }
        //    }

        //    private void DoCopyMoveOperation(FileInfo file, string path_do_dir, bool doCopy = false)
        //    {
        //        try
        //        {
        //            if (doCopy) // Copy file
        //            {
        //                file.CopyTo($"{path_do_dir}\\{file.Name}", false);
        //            }
        //            else // Move it instead of copying
        //            {
        //                file.MoveTo($"{path_do_dir}\\{file.Name}", false);
        //            }
        //        }
        //        catch (System.IO.IOException e)
        //        {
        //            //Skip copy / move
        //            this.nrOfErrors++;
        //            Trace.TraceError($"Copy/Move error: \"{e.Message}\". File name: \"{file.Name}\"");
        //        }
        //        catch (Exception e)
        //        {
        //            this.nrOfErrors++;
        //            Trace.TraceError(e.Message);
        //        }
        //    }
    }
}
