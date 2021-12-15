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

        private void btnTestOutput_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(this.moverModel.nameCollisionAction);
            Trace.WriteLine(this.moverModel.compareFilesAction);
            EventData ed = new EventData("Test", new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4)), new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4)));
            EventDateTime edt = new EventDateTime(new DateTime(2017, 11, 03, 2, 3, 4));
            Trace.WriteLine(edt.DateTimePrettyString);
            Trace.WriteLine(edt.Hour);
            Trace.WriteLine(edt.Minute);
            Trace.WriteLine(edt.ListOfValidHours);

            foreach (var s in this.moverModel.extensionInfoList)
            {
                Trace.WriteLine($"{s.Name} : {s.Active}");
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
