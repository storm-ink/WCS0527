using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Wcs.Framework;

namespace Wcs.Services.Wcf
{
    /// <summary>
    /// 报警统计服务
    /// </summary>
    [ServiceContract]
    public interface IWarningReportService
    {
        /// <summary>
        /// 查找报警数据
        /// </summary>
        /// <param name="deviceType">设备类型</param>
        /// <param name="deviceName">设备名称</param>
        /// <param name="code">错误码</param>
        /// <param name="name">名称</param>
        /// <param name="userName">处理用户</param>
        /// <param name="isFault">是否是故障</param>
        /// <param name="repaired">是否已修复</param>
        /// <param name="fromDate">开始日期（发生的时间）</param>
        /// <param name="toDate">结束日期（发生的时间）</param>
        /// <returns></returns>
        [OperationContract]
        WarningRecord[] Find(String deviceType,
            String deviceName,
            String code,
            String name,
            String userName,
            Boolean? isFault,
            Boolean? repaired,
            DateTime? fromDate,
            DateTime? toDate,
            Int32 pageSize,
            Int32 currentPageNo,
            out long totals
            );


        /// <summary>
        /// 添加一个新的报警数据
        /// </summary>
        /// <param name="record">报警数据</param>
        [OperationContract]
        void Add(WarningRecord record);

        /// <summary>
        /// 更新指定的报警数据
        /// </summary>
        /// <param name="record">报警数据</param>
        [OperationContract]
        void Update(WarningRecord record);

        /// <summary>
        /// 删除指定的报警数据
        /// </summary>
        /// <param name="id">报警数据id</param>
        [OperationContract]
        void Delete(Int32 id);

        /// <summary>
        /// 获取指定的报警数据
        /// </summary>
        /// <param name="id">报警数据id</param>
        /// <returns></returns>
        [OperationContract]
        WarningRecord Get(Int32 id);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [OperationContract]
        Boolean Login(String userName, String password);

        /// <summary>
        /// 报表-按故障类型统计
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="deviceName"></param>
        /// <param name="includeWarning"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<String, Int32> Report_CountByWarningType(DateTime? fromDate, DateTime? toDate, String deviceName, Boolean includeWarning, Boolean groupByName);

        /// <summary>
        /// 报表-按设备名称统计
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="warningName"></param>
        /// <param name="includeWarning"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<String, Int32> Report_CountByDeviceName(DateTime? fromDate, DateTime? toDate, String warningName, Boolean includeWarning);

        /// <summary>
        /// 报表-故障解决比例
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="deviceName"></param>
        /// <param name="warningName"></param>
        /// <param name="includeWarning"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<Boolean, Int32> Report_CountByRepaird(DateTime? fromDate, DateTime? toDate, String[] deviceNames, String[] warningNames, Boolean includeWarning);

        /// <summary>
        /// 报表-故障比例
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="deviceNames"></param>
        /// <param name="warningNames"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<Boolean, Int32> Report_CountByFault(DateTime? fromDate, DateTime? toDate, String[] deviceNames, String[] warningNames);


        /// <summary>
        /// 报表-月走势图
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="deviceNames"></param>
        /// <param name="warningNames"></param>
        /// <param name="includeWarning"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string, List<int[]>> Report_TrendByMonth(Int32 year, Int32 month, String[] deviceNames, String[] warningNames, Boolean includeWarning);

        /// <summary>
        /// 报表-年走势图
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deviceNames"></param>
        /// <param name="warningNames"></param>
        /// <param name="includeWarning"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string, List<int[]>> Report_TrendByYear(Int32 year, String[] deviceNames, String[] warningNames, Boolean includeWarning);

        /// <summary>
        /// 报表-故障率
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deviceNames"></param>
        /// <param name="includeWarning"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string, List<double[]>> Report_FailureRate(int year, string[] deviceNames, bool includeWarning);
    }
}
