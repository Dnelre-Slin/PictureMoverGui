using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PictureMoverGui
{
    /// <summary>
    /// Interaction logic for ExtensionWindow.xaml
    /// </summary>
    public partial class ExtensionWindow : Window
    {
        private PictureMoverModel moverModel;
        public ExtensionWindow(PictureMoverModel moverModel)
        {
            InitializeComponent();

            this.moverModel = moverModel;
            this.DataContext = this.moverModel;
        }

        public void ExtensionWindow_Closing(object sender, CancelEventArgs e)
        {
            this.moverModel.extensionInfoList = this.moverModel.extensionInfoList; // Trigger set property function
        }
    }
}
