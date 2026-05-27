using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Wcs;
using Wcs.DefaultImpls.Crane;
using Wcs.Framework;

namespace Wcs.App.Plugins.CranePriorToLoadLocation
{
    [WcsPluginInfo(typeof(CranePriorToLoadLocationPlugin), "堆垛机入库提前待命", "Sineva", "2014/4/10 16:24:05", "在堆垛机空闲的时候，如果有前往相关堆垛机的入库任务，在开启该功能的情况下允许堆垛机提前在取货点待命。\n\n可通过配置设置节点“提前待命时间间隔”的值来改变轮循间隔（单位：毫秒），默认为5000毫秒。", false)]
    public class CranePriorToLoadLocationPlugin : Wcs.WcsPlugin,IDeviceHolder
    {
        Logger _logger = LogManager.GetCurrentClassLogger();
        ToolStripMenuItem tsmiCranePriorToLoadLocation;
        Thread _thread;
        Boolean switcher;
        IEnumerable<CraneDevice> craneDevices;
        IEnumerable<TaskableDevice> taskableDevices;
        public override bool Initialization(WcsContext context)
        {
            craneDevices = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is CraneDevice)
                .Select(x => x.Device as CraneDevice)
                .ToList();

            taskableDevices = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is TaskableDevice)
                .Select(x => x.Device as TaskableDevice)
                .ToList();

            tsmiCranePriorToLoadLocation = new ToolStripMenuItem("堆垛机入库提前待命");
            tsmiCranePriorToLoadLocation.Click += tsmiCranePriorToLoadLocation_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Edit).DropDownItems.Add(tsmiCranePriorToLoadLocation);


            var v = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<Boolean>("堆垛机入库提前待命");
            tsmiCranePriorToLoadLocation.Checked = v;
            if (v)
            {
                _thread = new Thread(proc);
                _thread.Priority = ThreadPriority.BelowNormal;
                _thread.IsBackground = true;
                _thread.Name = "堆垛机提前待命";
                _thread.StartAndManaged();
            }
            return base.Initialization(context);
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\堆垛机入库提前待命")]
        void tsmiCranePriorToLoadLocation_Click(object sender, EventArgs e)
        {
            var v = !tsmiCranePriorToLoadLocation.Checked;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<Boolean>("堆垛机入库提前待命",v);
            tsmiCranePriorToLoadLocation.Checked = v;

            if (!v)
            {
                switcher = false;
            }
            else
            {
                if (switcher == false)
                {
                    _thread = new Thread(proc);
                    _thread.Priority = ThreadPriority.BelowNormal;
                    _thread.IsBackground = true;
                    _thread.Name = "堆垛机提前待命";
                    _thread.StartAndManaged();
                }
            }
        }

        Random rnd = new Random();
        void proc(object stat)
        {
            switcher = true;
            while (switcher)
            {
                Thread.Sleep(Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<Int32>("提前待命时间间隔",5000));
                try
                {
                    foreach (var craneDevice in craneDevices)
                    {
                        if (!craneDevice.IsConnected)
                        {
                            continue;
                        }

                        if (!craneDevice.IsIdle)
                        {
                            continue;
                        }

                        if (craneDevice.Holder != null)
                        {
                            continue;
                        }

                        if (craneDevice.EquipmentActionScheduler.CurrentAction != null)
                        {
                            continue;
                        }

                        if (craneDevice.EquipmentActionScheduler.Actions.Any(x=>x.Status!=EquipmentActionStatus.Completed && x.Status!=EquipmentActionStatus.Cancelled))
                        {
                            continue;
                        }

                        if (craneDevice.LastStatus == null)
                        {
                            continue;
                        }
                        
                        var 巷道口入库点 = Wcs.Framework.Cfg.WcsConfiguration.Instance.RouteCollection.Routes
                            .Where(x => x.Device == craneDevice && x.Direction == RouteDirection.In)
                            .Select(x => new LocationInfo(x.StartLocation.Device.Name,x.StartLocation.DeviceCode,x.StartLocation.UserCode))
                            .ToList();

                        var tasks = taskableDevices
                            .SelectMany(x => x.EquipmentActionScheduler.Actions)
                            .Where(act => act.Status == EquipmentActionStatus.Executing
                                    && 巷道口入库点.Any(loc => loc.Equals(act.Movement.EndLocation))
                            )
                            .ToList();

                        if (!tasks.Any())
                        {
                            continue;
                        }

                        var currLoc = craneDevice
                            .Locations
                            .FirstOrDefault(x =>
                                x.Level == craneDevice.LastStatus.Level
                                && x.Column == craneDevice.LastStatus.Column
                                );

                        if (currLoc == null)
                        {
                            continue;
                        }

                        //当前位置就有一个要到达的任务
                        if (tasks.Any(x => x.Movement
                            .EndLocation.Equals(new LocationInfo(currLoc.Device.Name, currLoc.DeviceCode, currLoc.UserCode))
                            )
                        )
                        {
                            continue;
                        }

                        var 提前待命任务 = tasks.OrderBy(x => getDistance(x))
                            .FirstOrDefault();
                        if (提前待命任务 == null)
                        {
                            continue;
                        }

                        Location toLocation = LocationConverter.ToLocation(提前待命任务.Movement.EndLocation);
                        RackLocation toRackLocation;
                        if (toLocation is RackLocation)
                        {
                            toRackLocation = (RackLocation)toLocation;
                        }
                        else
                        {
                            toRackLocation = craneDevice.Locations.Single(x => !(x is ILocationWildcard) && x.Equals(toLocation));
                        }

                        craneDevice.Hold(this);
                        try
                        {
                            int taskId = rnd.Next(10000000, 99999999);
                            CraneMoveAction moveAction = new CraneMoveAction(craneDevice, new EquipmentActionGroup(), taskId, toRackLocation,0);
                            _logger.Trace1(string.Format("准备向 {0} 发送一个入库任务提前待命指令 {1}", craneDevice, moveAction), this);
                            craneDevice.SendTask(moveAction);
                            _logger.Trace1(string.Format("指令发送成功", craneDevice, moveAction), this);

                        }
                        catch (Exception ex)
                        {
                            _logger.Error1(new Exception(string.Format("{0} 在发送入库提前待命任务时发生错误",craneDevice),ex),this);
                        }
                        finally
                        {
                            craneDevice.Unhold(this);
                        }
                    }        
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }
            }
        }

        int getDistance(EquipmentAction action)
        {
            Route route = Wcs.Framework.Cfg.WcsConfiguration.Instance.RouteCollection.Routes.SingleOrDefault(x => x.Id == action.Movement.RouteId);
            if (route == null)
            {
                return int.MaxValue;
            }

            Location startLoc = LocationConverter.ToLocation(action.Movement.StartLocation);
            Location endLoc = LocationConverter.ToLocation(action.Movement.EndLocation);
            Location curLoc = LocationConverter.ToLocation(action.Movement.Task.CurrentLocation);

            var curLocIndex = route.Locations.ToList().FindIndex(x => x.Equals(curLoc));
            if(curLocIndex<0)
            {
                return int.MaxValue;
            }

            var endLocIndex = route.Locations.ToList().FindIndex(x => x.Equals(endLoc));
            if(endLocIndex<0)
            {
                return int.MaxValue;
            }

            return endLocIndex - curLocIndex;
        }
    }
}
