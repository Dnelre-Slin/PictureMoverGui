using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace PictureMoverGui.Helpers
{
    public static class RemovableDeviceLookup
    {
        public static string GetDriveLetterFromSerialNumber(string serialNumber)
        {
            ManagementClass diskDrive = new ManagementClass("Win32_Diskdrive");
            {
                foreach (ManagementObject dd in diskDrive.GetInstances())
                {
                    if (dd["SerialNumber"]?.ToString() == serialNumber)
                    {
                        foreach (ManagementObject dp in dd.GetRelated("Win32_DiskPartition"))
                        {
                            foreach (ManagementObject ld in dp.GetRelated("Win32_LogicalDisk"))
                            {
                                return ld["DeviceId"]?.ToString(); // Return first found drive letter
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
