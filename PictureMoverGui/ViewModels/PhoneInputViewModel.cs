using MediaDevices;
using PictureMoverGui.Commands;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
//using System.Management;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class PhoneInputViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public IEnumerable<string> ItemChoices => new List<string>() { "hello", "bye", "see" };
        public string ChosenString { get; set; }

        public ICommand RefreshDevices { get; }
        public ICommand ConnectDevice { get; }

        public PhoneInputViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            ChosenString = "hello";

            RefreshDevices = new CallbackCommand(OnRefreshDevices);
            ConnectDevice = new CallbackCommand(OnConnectDevice);
        }

        protected void OnRefreshDevices(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnRefreshDevices");
            //ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            //ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            //ManagementClass logicalDisks = new ManagementClass("Win32_LogicalDisk");
            //{
            //    foreach (ManagementObject ld in logicalDisks.GetInstances())
            //    {
            //        System.Diagnostics.Debug.WriteLine(ld["DeviceId"]?.ToString());
            //        //if (mo["SerialNumber"]?.ToString() == "AA00000000000489")
            //        {
            //            foreach (ManagementObject dp in ld.GetRelated("Win32_DiskPartition"))
            //            {
            //                foreach (ManagementObject dd in dp.GetRelated("Win32_Diskdrive"))
            //                {
            //                    System.Diagnostics.Debug.WriteLine(dd["SerialNumber"]?.ToString());
            //                }
            //            }
            //        }
            //        System.Diagnostics.Debug.WriteLine("------------------------------");
            //    }
            //}
            //ManagementClass devs = new ManagementClass("Win32_Diskdrive");
            //{
            //    foreach(ManagementObject mo in devs.GetInstances())
            //    {
            //        System.Diagnostics.Debug.WriteLine(mo["SerialNumber"]?.ToString());
            //        //if (mo["SerialNumber"]?.ToString() == "AA00000000000489")
            //        {
            //            foreach (ManagementObject b in mo.GetRelated("Win32_DiskPartition"))
            //            {
            //                foreach (ManagementObject c in b.GetRelated("Win32_LogicalDisk"))
            //                {
            //                    System.Diagnostics.Debug.WriteLine(c["VolumeName"]?.ToString());
            //                    System.Diagnostics.Debug.WriteLine(c["DeviceId"]?.ToString());
            //                }
            //            }
            //        }
            //        System.Diagnostics.Debug.WriteLine("------------------------------");
            //    }
            //}
            //foreach (ManagementObject wmi_HD in moSearcher.Get())
            //{
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["Model"].ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["InterfaceType"].ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["SerialNumber"].ToString());
            //    System.Diagnostics.Debug.WriteLine(wmi_HD["Name"].ToString());
            //    System.Diagnostics.Debug.WriteLine(wmi_HD["DeviceID"].ToString());
            //    System.Diagnostics.Debug.WriteLine(wmi_HD["PNPDeviceID"]?.ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["Description"].ToString());
            //    //System.Diagnostics.Debug.WriteLine(wmi_HD["SystemName"].ToString());
            //    System.Diagnostics.Debug.WriteLine("------------------------------");

            //}
            //foreach (var v in System.IO.DriveInfo.GetDrives())
            //{
            //    System.Diagnostics.Debug.WriteLine(v.Name);
            //    System.Diagnostics.Debug.WriteLine(v.DriveType);
            //    //System.Diagnostics.Debug.WriteLine(v.DriveFormat);
            //    System.Diagnostics.Debug.WriteLine(v.IsReady);
            //    System.Diagnostics.Debug.WriteLine("------------------------------");
            //}
            //foreach (MediaDevice mediaDevice in MediaDevice.GetDevices())
            //{
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.FriendlyName);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.DeviceId);
            //    //System.Diagnostics.Debug.WriteLine(mediaDevice.Protocol);
            //    //System.Diagnostics.Debug.WriteLine(mediaDevice.SerialNumber);                
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.IsConnected);
            //    mediaDevice.Connect();
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.Protocol);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.SerialNumber);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.Model);
            //    System.Diagnostics.Debug.WriteLine(mediaDevice.Manufacturer);

            //    mediaDevice.Disconnect();
            //    System.Diagnostics.Debug.WriteLine("------------------------------");
            //}
        }
        protected void OnConnectDevice(object parameter)
        {
            System.Diagnostics.Debug.WriteLine("OnConnectDevice");
        }
    }
}
