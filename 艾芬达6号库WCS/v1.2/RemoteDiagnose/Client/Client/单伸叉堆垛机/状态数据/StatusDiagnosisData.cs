using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace Client.SingleForkCrane
{
    public class StatusDiagnosisData : AbstractDiagnosisData,Spiral.IAggregateRoot<Int32>
    {
        static Alarm[] _alarms;

        public static Alarm[] GetAlarms()
        {
            lock (typeof(StatusDiagnosisData))
            {
                if (_alarms == null)
                {
                    using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                    {
                        using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                        {
                            _alarms = context.Session.Query<Alarm>().ToArray();
                            unitOfWork.Commit();
                        }
                    }
                }

                return _alarms;
            }
        }

        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual Alarm Alarm { get; set; }

        /// <summary>
        /// 指示堆垛机当前是否在站点位置
        /// </summary>
        public virtual Boolean AtStation { get; set; }

        /// <summary>
        /// 当前所在列
        /// </summary>
        public virtual Int32 Column { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// 错误码（默认为 0）
        /// </summary>
        public virtual Int32 ErrorCode { get; set; }

        /// <summary>
        /// 堆垛机事件
        /// </summary>
        public virtual CraneEvent Event { get; set; }

        /// <summary>
        /// 货叉水平位置状态
        /// </summary>
        public virtual ForkHorizontalPosition ForkHorizontalPosition { get; set; }

        /// <summary>
        /// 货叉上下位置状态
        /// </summary>
        public virtual ForkVerticalPosition ForkVerticalPosition { get; set; }

        /// <summary>
        /// 当前所在层
        /// </summary>
        public virtual Int32 Level { get; set; }

        /// <summary>
        /// 堆垛机状态
        /// </summary>
        public virtual CraneStatus State { get; set; }
        /// <summary>
        /// 当前任务号
        /// </summary>
        public virtual String TaskId { get; set; }
        /// <summary>
        /// 原始报文信息
        /// </summary>
        public virtual String Telex { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdatedAt { get; set; }

        public virtual Int32 Version { get; protected set; }
        public override int GetErrorCode()
        {
            return this.ErrorCode;
        }

        public virtual Boolean IsRunning()
        {
            return this.State == CraneStatus.放货
                || this.State == CraneStatus.回原点
                || this.State == CraneStatus.取货
                || this.State == CraneStatus.无货运行
                || this.State == CraneStatus.有货运行;
        }

        public override bool IsWarnInfo()
        {
            return this.ErrorCode != 0 || this.State==CraneStatus.报警停机;
        }
        
        public override Warning NewWarning()
        {
            Warning warning = new Warning();
            warning.Name = this.Name;
            warning.ErrorCode = this.ErrorCode;
            warning.TaskId = this.TaskId;
            warning.StateAId = this.LogId;
            warning.StartTime = this.LongDate;
            if (this.ErrorCode == 0)
            {
                warning.AlarmName = this.State.ToString();
            }
            else
            {
                warning.AlarmName = GetAlarms().Where(x => x.Code == ErrorCode).Select(x => x.Name).FirstOrDefault();
            }
            warning.Level = WarningLevel.报警;
            warning.SourceType = this.GetType().Name;
            warning.DeviceType = "单货叉堆垛机";

            return warning;
        }
    }
}
