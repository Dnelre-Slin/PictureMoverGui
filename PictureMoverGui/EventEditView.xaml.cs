using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for EventEditView.xaml
    /// </summary>
    public partial class EventEditView : UserControl
    {
        private PictureMoverModel moverModel;
        public EventEditView()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(EventEditView_DataContextChanged);
        }

        private void EventEditView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.moverModel = this.DataContext as PictureMoverModel;
        }

        private void btnOpenStartDateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    this.moverModel.eventDataEdit.StartDateTime = new EventDateTime(fileInfo.LastWriteTime);
                }
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void btnOpenEndDateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    this.moverModel.eventDataEdit.EndDateTime = new EventDateTime(fileInfo.LastWriteTime);
                }
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.moverModel.eventDataEdit = null;
            this.moverModel.UpdateEventListInSettings();
        }
    }
}
