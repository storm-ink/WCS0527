using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wcs.Framework;

namespace WcfForSubSystemConveyorOccupySingleServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“ConveyorService”。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
    ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ConveyorService : IConveyorService
    {
        /// <summary>
        /// 获取输送线站位数据数据
        /// </summary>
        /// <param name="ListConveyor">需要获取的输送线列表</param>
        /// <returns>输送线号和对应的输送线数据，对应的Dictionar集合</returns>
        public Dictionary<int, ConveyorDate> GetConveyorState(List<int> ListConveyor)
        {
            Dictionary<int, ConveyorDate> returnvalue = new Dictionary<int, ConveyorDate>();

            try
            {
                ConveryorDateContener conterner = ConveryorDateContener.GetInstance();
                foreach (var item in ListConveyor)
                {
                    if (!returnvalue.Keys.Contains(item))
                    {
                        returnvalue.Add(item, conterner.Get(item));
                    }
                }
            }
            catch (Exception ex)
            {

                DBGLog.WriteLog(DBGLog.LOG_LV_INFO, "服务异常{0} ", ex);
            }
            return returnvalue;

        }

        /// <summary>
        /// 获取所有输送线状态数据
        /// </summary>
        /// <returns>所有输送线数据状态</returns>
        public Dictionary<int, ConveyorDate> GetAllStat()
        {
            Dictionary<int, ConveyorDate> returnvalue = new Dictionary<int, ConveyorDate>();

            try
            {
                ConveryorDateContener conterner = ConveryorDateContener.GetInstance();
                //returnvalue = conterner.Maindate;

                //foreach (var item in conterner.Maindate.Keys)
                //{
                //    returnvalue.Add(item, conterner.Maindate[item]);
                //}
                var keys = conterner.Maindate.Keys.ToList();
                foreach (var item in keys)
                {
                    returnvalue.Add(item, conterner.Get(item));
                }


                //foreach (var item in ListConveyor)
                //{
                //    if (!returnvalue.Keys.Contains(item))
                //    {
                //        returnvalue.Add(item, conterner.Get(item));
                //    }
                //}
            }
            catch (Exception ex)
            {

                DBGLog.WriteLog(DBGLog.LOG_LV_INFO, "服务异常{0} ", ex);
            }
            return returnvalue;
        }
    }
}
