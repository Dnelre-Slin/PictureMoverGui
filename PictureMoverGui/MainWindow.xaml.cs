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
        PictureMoverModel moverModel;
        DirectorySelector directorySelector;
        PictureMoverUiHandler moverUiHandler;
        DirectoryValidator directoryValidator;

        public MainWindow()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("MyTextFile.log"));
            Trace.AutoFlush = true;

            InitializeComponent();
            this.moverModel = new PictureMoverModel();
            this.DataContext = this.moverModel;

            this.directorySelector = new DirectorySelector(this.moverModel);
            this.moverUiHandler = new PictureMoverUiHandler(this.moverModel);
            this.directoryValidator = new DirectoryValidator(this.moverModel, this.directorySelector, this.moverUiHandler);

        }

        private void btnChooseSourceDir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.directorySelector.ChooseSourceButtonClick();
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }
        private void btnChooseSourceDirCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.directorySelector.ChooseSourceButtonCancelClick();
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void btnChooseDestinationDir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.directorySelector.ChooseDestinationButtonClick();
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void btnSwapSourceDestination_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.directorySelector.SwapSourceDestinationButtonClick();
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.directoryValidator.Run();
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void btnStartCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.moverUiHandler.StartSorterButtonCancelClick();
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Trace.TraceInformation("TabControl changed");
            this.moverModel.extensionInfoList = this.moverModel.extensionInfoList; // Trigger set property function
        }

        private void btnTestOutput_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(this.moverModel.nameCollisionAction);
            Trace.WriteLine(this.moverModel.compareFilesAction);
        }
    }
}
