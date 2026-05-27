using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using Wcs;
using Wcs.DefaultImpls.Crane;
using Wcs.Framework;

namespace WMSServices
{
    public class WmsServicesDefaultDeviceStatusRequest : ApplicationStartup
    {
        static Thread _thread;

        Int32 _interval;

        public static Dictionary<String, String> _lastCraneStatus { get; set; }
        public const String IsOK = "正常";
        public const String IsError = "故障";

        public WmsServicesDefaultDeviceStatusRequest(Wcs.Framework.Cfg.StartupElement element)
            : base(element)
        {
            this._logger = LogManager.GetCurrentClassLogger();
            _lastCraneStatus = new Dictionary<string, string>();
        }

        public override void Run(IWcsApplication application)
        {
            ParameterizedThreadStart Start = new ParameterizedThreadStart(check);
            _thread = new Thread(Start);
            _thread.IsBackground = true;
            _thread.Start();
            this._logger.Debug1(String.Format("{0} 线程已经启动！", this), this);
        }

        private void check(object obj)
        {
            var interval = this.StartupElement.GetAttribute<String>("interval");
            if (interval == null)
            {
                throw new InvalidOperationException(String.Format("{0} 中的 hz 属性的值不能为空！", this.StartupElement.Node.GetXPath()));
            }
            _interval = Convert.ToInt32(interval);
            if (_interval == 0)
            {
                throw new InvalidOperationException(String.Format("{0} 中的 hz 属性的值不能为 0 ！", this.StartupElement.Node.GetXPath()));
            }

            var cranes = Wcs.Framework.Cfg.WcsConfiguration.Instance
                       .DeviceCollection.ParticularDeviceCollection
                       .SelectMany(x => x.DeviceElements).Where(x => x.Device is CraneDevice)
                       .Select(x => x.Device as CraneDevice);

            while (true)
            {
                Thread.Sleep(_interval);

                Dictionary<String,String> craneStatus = new Dictionary<string,string>();

                foreach (var item in cranes)
                {
                    #region 正常代码
                    if (!item.IsConnected || item.LastStatus == null || item.Warnings.Length != 0)
                    {
                        //1号巷道、2号巷道。。。。。
                        craneStatus.Add(item.Name, IsError);
                        continue;
                    }

                    var state = item.LastStatus.State;
                    switch (state)
                    {
                        case CraneStatus.AlarmAndShutdown:                              //报警停机
                        case CraneStatus.Disconnected:                                  //未连接
                        case CraneStatus.Initialized:                                   //初始化
                        case CraneStatus.ManualMode:                                    //手动操作
                        case CraneStatus.ResetAlarm:                                    //报警复位
                        case CraneStatus.奇怪的状态:
                            craneStatus.Add(item.Name, IsError);
                            continue;
                        case CraneStatus.BackToTheOrigin:                               //回原点
                        case CraneStatus.Pickup:                                        //取货
                        case CraneStatus.Putin:                                         //放货
                        case CraneStatus.有货运行:
                        case CraneStatus.无货待命:
                        case CraneStatus.无货运行:
                        case CraneStatus.有货待命:
                            if (!item.Locker.IsEmpty)
                            {
                                craneStatus.Add(item.Name, IsError);
                                continue;
                            }
                            craneStatus.Add(item.Name, IsOK);
                            continue;
                        default:
                            _logger.Error(String.Format("获取到 {0} 号堆垛机的未知状态 {1}", item.Name, item.LastStatus.State), this);
                            continue;
                    }
                    #endregion

                    //#region 测试代码
                    //craneStatus.Add(item.No.ToString().Trim() + "号巷道", IsOK);
                    //#endregion 
                }

                if (!comparer(craneStatus,_lastCraneStatus))
                {
                    WmsServices.WcfWmsServiceForWcsClient client = new WmsServices.WcfWmsServiceForWcsClient();
                    try
                    {
                        foreach (var item in craneStatus)
                        {
                            Console.WriteLine("{0}:{1}", item.Key, item.Value);
                        }

                        WmsServices.WcsRequestInfo requestInfo = new WmsServices.WcsRequestInfo()
                        {
                            AdditionalInfo = craneStatus,
                            RequestType = "巷道设备状态通知"
                        };
                        client.Request(requestInfo);
                        client.Close();
                        this._logger.Debug1("巷道设备状态 通知 WMS 成功。", this);
                    }
                    catch (Exception ex)
                    {
                        if (client != null)
                            client.Abort();
                        this._logger.Error1(ex, this);
                        this._logger.Warn1(String.Format("上报WMS设备状态时失败！"),this);
                        //通知失败不更新 _lastCraneStatus 的值
                        continue;
                    }
                }
                _lastCraneStatus = craneStatus;
               // _lastCraneStatus.Equals(craneStatus);
            }
        }

        private Boolean comparer(Dictionary<String, String> d1, Dictionary<String, String> d2)
        {
            if (d1.Count() == 0 || d2.Count() == 0)
            {
                return false;
            }

            foreach (var item in d1)
            {
                if (!d2.Keys.Contains(item.Key) || d1[item.Key] != d2[item.Key])
                {
                    return false;
                } 
            }
            return true;
        }

        public override string ToString()
        {
            return "堆垛机状态上报WMS线程";
        }
    }
}
