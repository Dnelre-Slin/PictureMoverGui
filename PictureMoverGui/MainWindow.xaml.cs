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
        //PictureMoverModel moverModel;

        public MainWindow()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("MyTextFile.log"));
            Trace.AutoFlush = true;

            InitializeComponent();

            //this.moverModel = this.Resources["moverModel"] as PictureMoverModel;
            //this.DataContext = this.moverModel;

        }

    }
}
