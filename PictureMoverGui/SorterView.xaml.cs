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
    /// Interaction logic for SorterView.xaml
    /// </summary>
    public partial class SorterView : UserControl
    {
        private PictureMoverModel moverModel;
        private DirectorySelector directorySelector;
        private PictureMoverUiHandler moverUiHandler;
        private DirectoryValidator directoryValidator;

        public SorterView()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(SorterView_DataContextChanged);
        }

        void SorterView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.moverModel = this.DataContext as PictureMoverModel;

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
    }
}
