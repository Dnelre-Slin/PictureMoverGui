using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using PictureMoverGui.Views;
using System.Diagnostics;
using System.Windows;

namespace PictureMoverGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MasterStore _masterStore;
        private MainWindowViewModel _viewModel;
        public App()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("Error.log"));
            Trace.AutoFlush = true;

            _masterStore = new MasterStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _masterStore.RunningStore.SetStatusPercentage(0); // Fix to make sure StatusMessage is sat after base.OnStartup

            MainWindow = new MainWindow();

            _viewModel = new MainWindowViewModel(_masterStore);

            MainWindow.DataContext = _viewModel;
            MainWindow.Show();

            Helpers.UsbDeviceNotifier.Setup(MainWindow);

            //_masterStore.UsbDeviceStore.Setup(MainWindow);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _viewModel.Dispose();
            //_masterStore.UsbDeviceStore.Dispose();
        }
    }
}
