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
//using System.Windows.Forms;
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
        PictureMoverModel moverModel;
        //DirectorySelector directorySelector;
        //PictureMoverUiHandler moverUiHandler;
        //DirectoryValidator directoryValidator;

        public MainWindow()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("MyTextFile.log"));
            Trace.AutoFlush = true;

            InitializeComponent();

            this.moverModel = this.Resources["moverModel"] as PictureMoverModel;

            //this.moverModel = new PictureMoverModel();
            this.DataContext = this.moverModel;

            //this.directorySelector = new DirectorySelector(this.moverModel);
            //this.moverUiHandler = new PictureMoverUiHandler(this.moverModel);
            //this.directoryValidator = new DirectoryValidator(this.moverModel, this.directorySelector, this.moverUiHandler);

        }

        //private void btnChooseSourceDir_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.directorySelector.ChooseSourceButtonClick();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}
        //private void btnChooseSourceDirCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.directorySelector.ChooseSourceButtonCancelClick();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}

        //private void btnChooseDestinationDir_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.directorySelector.ChooseDestinationButtonClick();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}

        //private void btnSwapSourceDestination_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.directorySelector.SwapSourceDestinationButtonClick();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}

        //private void btnStart_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.directoryValidator.Run();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}

        //private void btnStartCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.moverUiHandler.StartSorterButtonCancelClick();
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}

        //private void btnEventData_Click(object sender, RoutedEventArgs e)
        //{
        //    //try
        //    //{
        //    Trace.WriteLine("Clicked");
        //        Button button = sender as Button;
        //    EventData edl = button.DataContext as EventData;
        //    edl.Edit = !edl.Edit;
        //    Trace.WriteLine(edl.Name);
        //    Trace.WriteLine(this.moverModel.eventDataList.IndexOf(edl));
        //    //}
        //    //catch (Exception err)
        //    //{
        //    //    Trace.TraceError(err.Message);
        //    //}
        //}

        //private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Trace.TraceInformation("TabControl changed");
        //    this.moverModel.extensionInfoList = this.moverModel.extensionInfoList; // Trigger set property function
        //}

        //private void btnTestOutput_Click(object sender, RoutedEventArgs e)
        //{
        //    Trace.WriteLine(this.moverModel.nameCollisionAction);
        //    Trace.WriteLine(this.moverModel.compareFilesAction);
        //    EventData ed = new EventData("Test", new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4)), new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4)));
        //    EventDateTime edt = new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4));
        //    Trace.WriteLine(edt.DateTimePrettyString);
        //    Trace.WriteLine(edt.Hour);
        //    Trace.WriteLine(edt.Minute);
        //    Trace.WriteLine(edt.ListOfValidHours);

        //    //Trace.WriteLine(this.moverModel.eventThing);
        //    //Trace.WriteLine(Properties.Settings.Default.EventList.ToString());
        //    //foreach(var s in Properties.Settings.Default.EventList)
        //    //{
        //    //    Trace.WriteLine(s);
        //    //}
        //    ////Trace.WriteLine(Properties.Settings.Default.TestList);
        //    ////Trace.WriteLine(Properties.Settings.Default.ExtList);
        //    //foreach (var s in Properties.Settings.Default.ExtList)
        //    //{
        //    //    Trace.WriteLine($"Name: {s.Name}, Amount: {s.Amount}, Active: {s.Active}");
        //    //}

        //    //List<PictureMoverModel.ExtensionInfo> pe = Properties.Settings.Default.ExtList;
        //    //Trace.WriteLine(pe);
        //    //Trace.WriteLine(pe.Count);
        //    //pe.Add(new PictureMoverModel.ExtensionInfo("Leo", 23, true));
        //    //List<List<string>> lls = new List<List<string>>();
        //    //Properties.Settings.Default.TestList = lls;


        //    //List<PictureMoverModel.ExtensionInfo> el = new List<PictureMoverModel.ExtensionInfo>();
        //    //el.Add(new PictureMoverModel.ExtensionInfo("Ole", 3, true));
        //    //el.Add(new PictureMoverModel.ExtensionInfo("Klara", 6, false));
        //    //Properties.Settings.Default.ExtList = el;
        //    //List<List<string>> tl = new List<List<string>>();
        //    //List<string> l1 = new List<string>();
        //    //l1.Add("Hello");
        //    //l1.Add("World");
        //    //List<string> l2 = new List<string>();
        //    //l2.Add("43");
        //    //tl.Add(l1);
        //    //tl.Add(l2);
        //    //Properties.Settings.Default.ExtList = pe;
        //    //Properties.Settings.Default.Save();
        //    //foreach(var s in Properties.Settings.Default.TestList)
        //    //{
        //    //    Trace.WriteLine(s);
        //    //}
        //}
    }
}
