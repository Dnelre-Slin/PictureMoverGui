using PictureMoverGui.Helpers;
using PictureMoverGui.Models;
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

namespace PictureMoverGui.Components
{
    /// <summary>
    /// Interaction logic for EventListView.xaml
    /// </summary>
    public partial class EventListView : UserControl
    {
        //private PictureMoverModel moverModel;
        public EventListView()
        {
            InitializeComponent();

            //this.DataContextChanged += new DependencyPropertyChangedEventHandler(EventListView_DataContextChanged);
        }

        //void EventListView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    //Trace.WriteLine("Context changed!!!!");
        //    this.moverModel = this.DataContext as PictureMoverModel;
        //}

        //private void btnEventData_Click(object sender, RoutedEventArgs e)
        //{
        //    //try
        //    //{
        //    Trace.WriteLine("Clicked");
        //    Button button = sender as Button;
        //    EventData edl = button.DataContext as EventData;
        //    //edl.Edit = !edl.Edit;
        //    Trace.WriteLine(edl.Name);
        //    Trace.WriteLine(this.moverModel.eventDataList.IndexOf(edl));
        //    this.moverModel.eventDataEdit = edl;
        //    //}
        //    //catch (Exception err)
        //    //{
        //    //    Trace.TraceError(err.Message);
        //    //}
        //}

        //private void btnEventListEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Button button = sender as Button;
        //        this.moverModel.eventDataEdit = button.DataContext as EventData; // Set this event data to the eventDataEdit.
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}

        //private void btnEventListDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Button button = sender as Button;
        //        EventData eventDataToRemove = button.DataContext as EventData;

        //        MessageBoxResult result = MessageBox.Show($"{App.Current.FindResource("MessageBoxDeleteEventText")} \"{eventDataToRemove.Name}\"?", $"{App.Current.FindResource("MessageBoxDeleteEventTitle")} \"{eventDataToRemove.Name}\"", MessageBoxButton.OKCancel);
        //        if (result == MessageBoxResult.OK)
        //        {
        //            this.moverModel.eventDataList.Remove(eventDataToRemove);

        //            this.moverModel.UpdateEventListInSettings();
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}

        //private void btnEventListCreate_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        EventData newEventData = new EventData(App.Current.FindResource("EventEditNameNewEvent").ToString(), new EventDateTime(DateTime.Now), new EventDateTime(DateTime.Now));
        //        this.moverModel.eventDataList.Add(newEventData);
        //        this.moverModel.eventDataEdit = newEventData;
        //    }
        //    catch (Exception err)
        //    {
        //        Trace.TraceError(err.Message);
        //    }
        //}
    }
}
