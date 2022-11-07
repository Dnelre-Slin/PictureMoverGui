using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Threading;

namespace PictureMoverGui.Helpers
{
    //public class PhoneUnlockPoller
    //{
    //    private MasterStore _masterStore;
    //    private Action<bool> _callback;

    //    private Timer _checkLockTimer;

    //    //private bool _isLocked;
    //    //public bool IsLocked => _isLocked;
    //    public bool IsLocked => CheckIsLocked();

    //    public PhoneUnlockPoller(MasterStore masterStore, Action<bool> callback)
    //    {
    //        _masterStore = masterStore;
    //        _callback = callback;

    //        _checkLockTimer = null;
    //        //_isLocked = true;

    //        _masterStore.UsbDeviceStore.DeviceInfoChanged += UsbDeviceStore_DeviceInfoChanged;
    //        _masterStore.SorterConfigurationStore.SorterConfigurationChanged += SorterConfigurationStore_SorterConfigurationChanged;

    //        StartTimer();
    //    }

    //    public void Dispose()
    //    {
    //        _masterStore.UsbDeviceStore.DeviceInfoChanged -= UsbDeviceStore_DeviceInfoChanged;

    //        if (_checkLockTimer != null)
    //        {
    //            _checkLockTimer.Dispose();
    //        }
    //    }

    //    private void SorterConfigurationStore_SorterConfigurationChanged(SorterConfigurationModel sorterConfigurationModel)
    //    {
    //        if (sorterConfigurationModel.MediaType == MediaTypeEnum.MediaDevice)
    //        {
    //            StartTimer();
    //        }
    //    }

    //    private void UsbDeviceStore_DeviceInfoChanged(CollectiveDeviceInfoModel collectiveDeviceInfoModel)
    //    {
    //        StartTimer();
    //    }

    //    private void StartTimer()
    //    {
    //        if (_masterStore.UsbDeviceStore.SelectedMediaDevice == null ||
    //            _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType != MediaTypeEnum.MediaDevice)
    //        {
    //            //_isLocked = true;
    //            //_callback(_isLocked);                
    //            _callback(true);
    //        }
    //        //else if (_checkLockTimer == null && _isLocked)
    //        //else if (_checkLockTimer == null && CheckIsLocked())
    //        else if (_checkLockTimer == null)
    //        {
    //            _checkLockTimer = new Timer(TimedChecker, null, 0, 1000);
    //        }
    //    }

    //    private void TimedChecker(object stateInfo)
    //    {
    //        try
    //        {
    //            System.Diagnostics.Debug.WriteLine("TimedChecker");
    //            bool locked = CheckIsLocked();
    //            //if (locked != _isLocked)
    //            //{
    //            //    _isLocked = locked;
    //            //    _callback(_isLocked);
    //            //}
    //            _callback(locked);
    //            if (!locked || _masterStore.UsbDeviceStore.SelectedMediaDevice == null 
    //                || _masterStore.SorterConfigurationStore.SorterConfiguration.MediaType != MediaTypeEnum.MediaDevice)
    //            {
    //                _checkLockTimer.Dispose();
    //                _checkLockTimer = null;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            System.Diagnostics.Debug.WriteLine(ex.Message);
    //        }
    //    }

    //    private bool CheckIsLocked()
    //    {
    //        int count = 0;
    //        if (_masterStore.UsbDeviceStore.SelectedMediaDevice != null && _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice != null && !_masterStore.UsbDeviceStore.IsResolving)
    //        {
    //            try
    //            {
    //                _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice.Connect();
    //                count = _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice.GetDrives().Length;
    //                _masterStore.UsbDeviceStore.SelectedMediaDevice.MediaDevice.Disconnect();
    //            }
    //            catch (Exception ex)
    //            {
    //                System.Diagnostics.Debug.WriteLine(ex.Message);
    //            }
    //        }
    //        System.Diagnostics.Debug.WriteLine($"TimedChecker count : {count}");
    //        bool locked = count == 0;
    //        return locked;
    //    }
    //}
}
