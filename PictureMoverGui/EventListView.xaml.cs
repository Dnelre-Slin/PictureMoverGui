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
    /// Interaction logic for EventListView.xaml
    /// </summary>
    public partial class EventListView : UserControl
    {
        private PictureMoverModel moverModel;
        public EventListView()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(EventListView_DataContextChanged);
        }

        void EventListView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Trace.WriteLine("Context changed!!!!");
            this.moverModel = this.DataContext as PictureMoverModel;
        }

        private void btnEventData_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            Trace.WriteLine("Clicked");
            Button button = sender as Button;
            EventData edl = button.DataContext as EventData;
            edl.Edit = !edl.Edit;
            Trace.WriteLine(edl.Name);
            Trace.WriteLine(this.moverModel.eventDataList.IndexOf(edl));
            //}
            //catch (Exception err)
            //{
            //    Trace.TraceError(err.Message);
            //}
        }
    }
}
