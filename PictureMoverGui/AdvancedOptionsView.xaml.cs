using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PictureMoverGui
{
    /// <summary>
    /// Interaction logic for AdvancedOptionsView.xaml
    /// </summary>
    public partial class AdvancedOptionsView : UserControl
    {
        private PictureMoverModel moverModel;

        public AdvancedOptionsView()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(AdvancedOptionsView_DataContextChanged);
        }

        void AdvancedOptionsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Trace.WriteLine("Context changed!!!!");
            this.moverModel = this.DataContext as PictureMoverModel;
        }

        private void btnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show($"{App.Current.FindResource("MessageBoxResetSettingsText")}", $"{App.Current.FindResource("MessageBoxResetSettingsTitle")}", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Properties.Settings.Default.Reset();
                    this.moverModel.SettingsRefresh();
                }
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void btnTestOutput_Click(object sender, RoutedEventArgs e)
        {
            //Trace.WriteLine(this.moverModel.nameCollisionAction);
            //Trace.WriteLine(this.moverModel.compareFilesAction);
            //EventData ed = new EventData("Test", new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4)), new EventDateTime(new DateTime(2017, 11, 03, 3, 3, 4)));
            //EventDateTime edt = new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4));
            //Trace.WriteLine(edt.DateTimePrettyString);
            //Trace.WriteLine(edt.Hour);
            //Trace.WriteLine(edt.Minute);
            //Trace.WriteLine(edt.ListOfValidHours);
            ////System.IO.DirectoryInfo d = new System.IO.DirectoryInfo("");

            //foreach (var s in this.moverModel.extensionInfoList)
            //{
            //    Trace.WriteLine($"{s.Name} : {s.Active}");
            //}

            ////DateTime eddt = DateTime.Parse(ed.StartDateTime.DateTimePrettyString);
            ////Trace.WriteLine(ed.StartDateTime.ToDateTime().DayOfWeek);            
            ////Trace.WriteLine(this.moverModel.eventData.ValidDateOrder);
            //Trace.WriteLine(DateTime.Now.Ticks);
            //Trace.WriteLine(DateTime.MinValue);
            //Trace.WriteLine(DateTime.MaxValue);

            //Trace.WriteLine(this.moverModel.nameCollisionAction);
            //Trace.WriteLine(this.moverModel.compareFilesAction);
            //Trace.WriteLine(this.moverModel.hashTypeAction);

            //System.Collections.ObjectModel.ObservableCollection<EventData> eventDataList = this.moverModel.eventDataList;
            //EventData newEvent = new EventData("Egypt", new EventDateTime(new DateTime(2010, 10, 10)), new EventDateTime(new DateTime(2011, 02, 02)));
            //eventDataList.Add(newEvent);

            System.Collections.ObjectModel.ObservableCollection<EventData> eventDataList = new System.Collections.ObjectModel.ObservableCollection<EventData>();

            Properties.Settings.Default.EventList = Simplifiers.EventListToSimpleList(eventDataList);
            Properties.Settings.Default.Save();

            System.Collections.ObjectModel.ObservableCollection<EventData> eventDataList2 = Simplifiers.SimpleListToEventList(Properties.Settings.Default.EventList);
            this.moverModel.eventDataList = eventDataList2;
            foreach (var ed in eventDataList2)
            {
                Trace.WriteLine($"Name: {ed.Name}  |  Start: {ed.StartDateTime}  |  End: {ed.EndDateTime}");
            }

            //Trace.WriteLine(this.moverModel.eventThing);
            //Trace.WriteLine(Properties.Settings.Default.EventList.ToString());
            //foreach(var s in Properties.Settings.Default.EventList)
            //{
            //    Trace.WriteLine(s);
            //}
            ////Trace.WriteLine(Properties.Settings.Default.TestList);
            ////Trace.WriteLine(Properties.Settings.Default.ExtList);
            //foreach (var s in Properties.Settings.Default.ExtList)
            //{
            //    Trace.WriteLine($"Name: {s.Name}, Amount: {s.Amount}, Active: {s.Active}");
            //}

            //List<PictureMoverModel.ExtensionInfo> pe = Properties.Settings.Default.ExtList;
            //Trace.WriteLine(pe);
            //Trace.WriteLine(pe.Count);
            //pe.Add(new PictureMoverModel.ExtensionInfo("Leo", 23, true));
            //List<List<string>> lls = new List<List<string>>();
            //Properties.Settings.Default.TestList = lls;


            //List<PictureMoverModel.ExtensionInfo> el = new List<PictureMoverModel.ExtensionInfo>();
            //el.Add(new PictureMoverModel.ExtensionInfo("Ole", 3, true));
            //el.Add(new PictureMoverModel.ExtensionInfo("Klara", 6, false));
            //Properties.Settings.Default.ExtList = el;
            //List<List<string>> tl = new List<List<string>>();
            //List<string> l1 = new List<string>();
            //l1.Add("Hello");
            //l1.Add("World");
            //List<string> l2 = new List<string>();
            //l2.Add("43");
            //tl.Add(l1);
            //tl.Add(l2);
            //Properties.Settings.Default.ExtList = pe;
            //Properties.Settings.Default.Save();
            //foreach(var s in Properties.Settings.Default.TestList)
            //{
            //    Trace.WriteLine(s);
            //}
        }
    }
}
