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
    }
}
