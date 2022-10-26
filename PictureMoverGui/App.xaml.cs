using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PictureMoverGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MasterStore _masterStore;
        public App()
        {
            _masterStore = new MasterStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _masterStore.RunningStore.SetStatusPercentage(0); // Fix to make sure StatusMessage is sat after base.OnStartup

            MainWindow = new MainWindow();

            MainWindowViewModel viewModel = new MainWindowViewModel(_masterStore);

            MainWindow.DataContext = viewModel;
            MainWindow.Show();
        }
    }
}
