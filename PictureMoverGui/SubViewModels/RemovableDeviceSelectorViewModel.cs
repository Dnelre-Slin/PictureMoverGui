using PictureMoverGui.Commands;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PictureMoverGui.SubViewModels
{
    public class RemovableDeviceSelectorViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public IEnumerable<string> RemovableDeviceChoices => _masterStore.UsbDeviceStore.RemovableDeviceList.Select(rd => rd.Name);

        public string RemovableDeviceConnected => _masterStore.UsbDeviceStore.SelectedRemovableDevice.IsConnected ? "✔️" : "❌";
        public Brush RemovableDeviceConnectedColor => _masterStore.UsbDeviceStore.SelectedRemovableDevice.IsConnected ? Brushes.Green : Brushes.Red;
        public Visibility RemovableDevicePickerVisibility => _masterStore.UsbDeviceStore.RemovableDeviceList.Count() > 0 ? Visibility.Visible : Visibility.Hidden;
        public string RemovableDeviceChosenName
        {
            get => _masterStore.UsbDeviceStore.SelectedRemovableDevice.Name;
            set
            {
                if (value != null && value != _masterStore.UsbDeviceStore.SelectedRemovableDevice.Name)
                {
                    _masterStore.UsbDeviceStore.SetNewSelectedRemovableDevice(value);
                }
            }
        }
        public string RemovableMediaPath => RemovableDeviceChosenName + _masterStore.UsbDeviceStore.SelectedRemovableDevice.Path;

        public ICommand RefreshUsbDevices { get; }
        public ICommand OpenFolderBrowserDialog { get; }

        public RemovableDeviceSelectorViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            RefreshUsbDevices = new CallbackCommand(OnRefreshUsbDevices);
            OpenFolderBrowserDialog = new CallbackCommand(OnOpenFolderBrowserDialog);

            _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.UsbDeviceStore.DeviceInfoChanged -= UsbDeviceStore_DeviceInfoChanged;
        }

        private void UsbDeviceStore_DeviceInfoChanged(UsbDeviceStore usbDeviceStore)
        {
            System.Diagnostics.Debug.WriteLine($"Device change: RemovableDeviceConnected: {RemovableDeviceConnected}");
            OnPropertyChanged(nameof(RemovableDeviceChoices));

            OnPropertyChanged(nameof(RemovableDeviceConnected));
            OnPropertyChanged(nameof(RemovableDeviceConnectedColor));
            OnPropertyChanged(nameof(RemovableDevicePickerVisibility));
            OnPropertyChanged(nameof(RemovableDeviceChosenName));
            OnPropertyChanged(nameof(RemovableMediaPath));
        }

        protected void OnRefreshUsbDevices(object parameter)
        {
            _masterStore.UsbDeviceStore.RefreshUsbDevices();
        }

        protected void OnOpenFolderBrowserDialog(object parameter)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(openFileDlg.SelectedPath))
            {
                System.Diagnostics.Debug.WriteLine(openFileDlg.SelectedPath);
                string[] paths = openFileDlg.SelectedPath.Split(':');
                _masterStore.UsbDeviceStore.SetSelectedRemovableDevicePath(paths[1]);
            }
        }
    }
}
