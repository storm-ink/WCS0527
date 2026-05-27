using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs;
using Wcs.Framework;
using Wcs.Framework.Cfg;
using NLog;
using System.Threading;
using System.Reflection;
using System.ComponentModel;

namespace Wcs.App.Plugins.HomePage
{
    public class CurrentDeviceAlarmReportStartup : IApplicationStartup
    {
        Logger _logger;
        static Thread _thread;
        public void Initialize(StartupElement element)
        {
        }

        public void Run(IWcsApplication application)
        {
            _logger = LogManager.GetCurrentClassLogger();

            _thread = new System.Threading.Thread(proc);
            _thread.Name = "RealTimeAlarmStatisticsThread";
            _thread.IsBackground = true;
            _thread.StartAndManaged();
            _logger.Debug1("CurrentDeviceAlarmReport线程已启动", this);
        }

        private void proc(object obj)
        {
            var deviceElements = Wcs.Framework.Cfg.WcsConfiguration.Instance
                  .DeviceCollection.ParticularDeviceCollection
                  .SelectMany(x => x.DeviceElements);

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                List<DeviceAlarm> _now = new List<DeviceAlarm>();
                try
                {
                    foreach (var item in deviceElements)
                    {
                        var device = item.Device;
                        if (!device.IsConnected || !device.Locker.IsEmpty || device.Warnings.Count() != 0)
                            _now.Add(new DeviceAlarm()
                            {
                                DeviceName = device.Name,
                                DeviceType = device.GetDeviceType(),
                                IsOK = false,
                                IsConnected = device.IsConnected ? "已连接" : "未连接",
                                IsLocked = device.Locker.IsEmpty ? "" : "锁定",
                                Locker = device.Locker.IsEmpty ? "" : device.Locker.IPAddress,
                                IsAlarm = device.Warnings.Count() != 0 ? "故障中" : "",
                                AlarmDescription = device.Warnings.Count() != 0 ? $"{string.Join("/", device.Warnings)}" : "",
                                UpDateAt = DateTime.Now
                            });
                        else
                            _now.Add(new DeviceAlarm()
                            {
                                DeviceName = device.Name,
                                DeviceType = device.GetDeviceType(),
                                IsOK = true,
                                IsConnected = "",
                                IsLocked = "",
                                Locker = "",
                                IsAlarm = "",
                                AlarmDescription = "",
                                UpDateAt = DateTime.Now
                            });

                        if (device is ConveyorDevice)
                        {
                            var conveyor = (ConveyorDevice)device;
                            foreach (var loc in conveyor.Locations)
                            {
                                var posNo = Convert.ToUInt16(loc.DeviceCode);
                                //var locState = conveyor.ConveyorLocationStates.FirstOrDefault(x => x.PosNo == posNo);
                                var locAlarm = conveyor.MachineAlarms.FirstOrDefault(x => x.PosNo == posNo);
                                var locGenerAlarm = conveyor.ReadStatus<GeneralAlarmNetTransferObject>().FirstOrDefault(x => x.PosNo == posNo);
                                //if (locState == null)
                                //    _now.Add(new DeviceAlarm()
                                //    {
                                //        DeviceName = loc.DeviceCode,
                                //        DeviceType = $"{device.Name}_{ device.GetDeviceType()}",
                                //        DeviceState = new DeviceStatus()
                                //        {
                                //            IsOK = false,
                                //            IsConnected = device.IsConnected ? "已连接" : "未连接",
                                //            IsLocked = "",
                                //            Locker = "",
                                //            IsAlarm = "故障中",
                                //            AlarmDescription = "未获取设备状态",
                                //            UpDateAt = DateTime.Now
                                //        }
                                //    });
                                if (locAlarm == null && locGenerAlarm == null)
                                    _now.Add(new DeviceAlarm()
                                    {
                                        DeviceName = $"{loc.DeviceCode}@{device.Name}",
                                        DeviceType = device.GetDeviceType(),
                                        IsOK = false,
                                        IsConnected = device.IsConnected ? "已连接" : "未连接",
                                        IsLocked = "",
                                        Locker = "",
                                        IsAlarm = "故障中",
                                        AlarmDescription = "未获取设备报警状态",
                                        UpDateAt = DateTime.Now
                                    });
                                else
                                {
                                    List<string> alarms = new List<string>();
                                    if (locAlarm != null)
                                    {
                                        Type t = locAlarm.GetType();
                                        PropertyInfo[] PropertyList = t.GetProperties();
                                        foreach (PropertyInfo proItem in PropertyList)
                                        {
                                            if (proItem.PropertyType.Name == typeof(bool).Name)
                                            {
                                                var _bool = (bool)proItem.GetValue(locAlarm);
                                                if (_bool)
                                                {
                                                    var displayName = proItem.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
                                                    if (displayName != null)
                                                        alarms.Add(((DisplayNameAttribute)displayName).DisplayName);
                                                    else
                                                        alarms.Add(proItem.Name);
                                                }
                                            }
                                        }

                                    }
                                    else if (locGenerAlarm != null)
                                        alarms = locGenerAlarm.GetAlarm().ToList();

                                    if (alarms.Count() == 0)
                                        _now.Add(new DeviceAlarm()
                                        {
                                            DeviceName = $"{loc.DeviceCode}@{device.Name}",
                                            DeviceType = device.GetDeviceType(),
                                            IsOK = true,
                                            IsConnected = device.IsConnected ? "已连接" : "未连接",
                                            IsLocked = "",
                                            Locker = "",
                                            IsAlarm = "",
                                            AlarmDescription = "",
                                            UpDateAt = DateTime.Now
                                        });
                                    else
                                        _now.Add(new DeviceAlarm()
                                        {
                                            DeviceName = $"{loc.DeviceCode}@{device.Name}",
                                            DeviceType = device.GetDeviceType(),
                                            IsOK = false,
                                            IsConnected = device.IsConnected ? "已连接" : "未连接",
                                            IsLocked = "",
                                            Locker = "",
                                            IsAlarm = "故障中",
                                            AlarmDescription = string.Join("/", alarms),
                                            UpDateAt = DateTime.Now
                                        });
                                }
                            }
                        }
                    }

                    if (string.Join("/", HomePageHelper.last.Select(x => x.ToString())) != string.Join("/", _now.Select(x => x.ToString())))
                        HomePageHelper.last = _now.ToArray();
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }
            }
        }
    }
}
