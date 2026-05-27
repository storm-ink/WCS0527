using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Client
{
    /// <summary>
    /// 诊断数据
    /// </summary>
    [DataContract]
    public abstract class AbstractDiagnosisData
    {
        /// <summary>
        /// Id(主键)
        /// </summary>
        [DataMember]
        public virtual Int32 Id { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>
        [DataMember]
        public virtual Int32 ProjectId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// 日志Id
        /// </summary>
        [DataMember]
        public virtual Int32 LogId { get; set; }
        /// <summary>
        /// 日志创建时间
        /// </summary>
        [DataMember]
        public virtual DateTime LongDate { get; set; }
        /// <summary>
        /// 创建一个报警记录实例
        /// </summary>
        /// <returns></returns>
        public abstract Warning NewWarning();

        /// <summary>
        /// 获取一个值，指示当前状态是否是一条包含故障数据的信息
        /// </summary>
        /// <returns></returns>
        public abstract Boolean IsWarnInfo();
        /// <summary>
        /// 获取报警码
        /// </summary>
        /// <returns></returns>
        public abstract Int32 GetErrorCode();
    }
}
