using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Client.Logs
{
    [DataContract]
    public class WcsLogDiagnosisData : AbstractDiagnosisData,Spiral.IAggregateRoot<Int32>
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        [DataMember]
        public virtual String Level { get; set; }
        /// <summary>
        /// 机器名
        /// </summary>
        [DataMember]
        public virtual String MachineName { get; set; }
        /// <summary>
        /// 进程名
        /// </summary>
        [DataMember]
        public virtual String ProcessName { get; set; }
        /// <summary>
        /// 进程Id
        /// </summary>
        [DataMember]
        public virtual String ProcessId { get; set; }
        /// <summary>
        /// 程序集版本号
        /// </summary>
        [DataMember]
        public virtual String AssemblyVersion { get; set; }
        /// <summary>
        /// 线程Id
        /// </summary>
        [DataMember]
        public virtual Int32 ThreadId { get; set; }
        /// <summary>
        /// 日志名
        /// </summary>
        [DataMember]
        public virtual String Logger { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public virtual String Message { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [DataMember]
        public virtual String Exception { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public virtual String UserName { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        [DataMember]
        public virtual String IP { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        [DataMember]
        public virtual String TaskCode { get; set; }

        /// <summary>
        /// 设备任务号
        /// </summary>
        [DataMember]
        public virtual Int32? EquipmentTaskId { get; set; }
        /// <summary>
        /// 日志引发对象
        /// </summary>
        [DataMember]
        public virtual String Sender { get; set; }


        public virtual Int32 Version { get; protected set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdatedAt { get; set; }


        public override bool IsWarnInfo()
        {
            throw new NotImplementedException();
        }

        public override Warning NewWarning()
        {
            throw new NotImplementedException();
        }

        public override int GetErrorCode()
        {
            throw new NotImplementedException();
        }
    }
}
