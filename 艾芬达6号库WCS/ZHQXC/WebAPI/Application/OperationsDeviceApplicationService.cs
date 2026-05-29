using System;
using System.Collections.Generic;
using System.Linq;
using Wcs.Framework;

namespace ZHQXC
{
    internal class OperationsDeviceApplicationService
    {
        public IList<OperationsDeviceDto> GetDevices()
        {
            return GetConfiguredDevices()
                .Select(ToDeviceDto)
                .OrderBy(x => x.Name)
                .ToList();
        }

        public OperationsDeviceDto GetDevice(string deviceName)
        {
            Device device = FindDevice(deviceName);
            if (device == null)
            {
                return null;
            }

            return ToDeviceDto(device);
        }

        public DeviceLockerHand SetDeviceLock(string deviceName, bool lockDevice)
        {
            DeviceLockerHand result = new DeviceLockerHand
            {
                Device = deviceName,
                Lock = lockDevice
            };

            TaskableDevice device = FindTaskableDevice(deviceName);
            if (device == null)
            {
                result.Result = false;
                result.Message = "未找到对应可执行任务的设备，请确认后重试";
                return result;
            }

            if (lockDevice)
            {
                if (device.Locker.IsEmpty)
                {
                    device.Lock(new LockerInfo(Environment.MachineName, LockerInfo.GetIpAddress()));
                    result.Result = true;
                    result.Message = "锁定成功";
                }
                else
                {
                    result.Result = false;
                    result.Message = "已锁定";
                }
            }
            else
            {
                if (device.Locker.IsEmpty)
                {
                    result.Result = false;
                    result.Message = "已解锁";
                }
                else
                {
                    device.Unlock(LockerInfo.Adminstrator);
                    result.Result = true;
                    result.Message = "解锁成功";
                }
            }

            return result;
        }

        public OperationsDeviceCommandResult SetDeviceConnection(string deviceName, bool connectDevice)
        {
            OperationsDeviceCommandResult result = new OperationsDeviceCommandResult();
            Device device = FindDevice(deviceName);
            if (device == null)
            {
                result.Result = false;
                result.Message = "未找到对应设备，请确认后重试";
                return result;
            }

            if (connectDevice)
            {
                if (device.IsConnected)
                {
                    result.Result = true;
                    result.Message = "设备已连接";
                    return result;
                }

                result.Result = device.Connect();
                result.Message = result.Result ? "连接成功" : "连接失败";
                return result;
            }

            if (!device.IsConnected)
            {
                result.Result = true;
                result.Message = "设备已断开";
                return result;
            }

            device.Disconnect();
            result.Result = true;
            result.Message = "断开成功";
            return result;
        }

        static IEnumerable<Device> GetConfiguredDevices()
        {
            return Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Select(x => x.Device);
        }

        static Device FindDevice(string deviceName)
        {
            return GetConfiguredDevices()
                .FirstOrDefault(x => x.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));
        }

        static TaskableDevice FindTaskableDevice(string deviceName)
        {
            return GetConfiguredDevices()
                .OfType<TaskableDevice>()
                .FirstOrDefault(x => x.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));
        }

        static OperationsDeviceDto ToDeviceDto(Device device)
        {
            return new OperationsDeviceDto
            {
                Name = device.Name,
                DeviceType = device.GetDeviceType(),
                IsConnected = device.IsConnected,
                IsLocked = device.Locker != null && !device.Locker.IsEmpty,
                LockerUser = device.Locker == null ? string.Empty : device.Locker.UserName,
                LockerIp = device.Locker == null ? string.Empty : device.Locker.IPAddress,
                IsTaskable = device is TaskableDevice
            };
        }
    }
}
