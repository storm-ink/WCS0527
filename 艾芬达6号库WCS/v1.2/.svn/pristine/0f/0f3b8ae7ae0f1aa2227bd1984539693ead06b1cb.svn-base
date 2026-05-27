using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NHibernate.Linq;
namespace Client.RailGuidedVehicle
{
    [DataContract]
    public class RailGuidedVehicleDiagnosisData : AbstractDiagnosisData,Spiral.IAggregateRoot<Int32>
    {
        /// <summary>
        /// 位置值
        /// </summary>
        [DataMember]
        public virtual Int32 Position { get;  set; }

        /// <summary>
        /// 当前站台号
        /// </summary>
        [DataMember]
        public virtual Int32 CurrentStation { get;  set; }

        /// <summary>
        /// 指示穿梭车当前是否在站点位置
        /// </summary>
        [DataMember]
        public virtual Boolean AtStation { get;  set; }

        /// <summary>
        /// 错误码（默认为 0）
        /// </summary>
        [DataMember]
        public virtual Int32 ErrorCode { get;  set; }

        /// <summary>
        /// 穿梭车状态
        /// </summary>
        [DataMember]
        public virtual RailGuidedVehicleStatus State { get;  set; }

        /// <summary>
        /// 穿梭车事件
        /// </summary>
        [DataMember]
        public virtual RailGuidedVehicleEvent Event { get;  set; }

        /// <summary>
        /// 当前任务号
        /// </summary>
        [DataMember]
        public virtual String TaskId { get;  set; }

        /// <summary>
        /// 托盘条码
        /// </summary>
        [DataMember]
        public virtual Int32 ContainerCode { get;  set; }

        /// <summary>
        /// 起点
        /// </summary>
        [DataMember]
        public virtual Int32 FromStation { get;  set; }

        /// <summary>
        /// 目的的
        /// </summary>
        [DataMember]
        public virtual Int32 ToStation { get;  set; }

        /// <summary>
        /// 任务模式
        /// </summary>
        [DataMember]
        public virtual RailGuidedVehicleTaskMode TaskMode { get;  set; }

        /// <summary>
        /// 原始报文信息
        /// </summary>
        [DataMember]
        public virtual String Telex { get; set; }


        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual Alarm Alarm { get; set; }

        public virtual Int32 Version { get; protected set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdatedAt { get; set; }


        public virtual Boolean IsRunning()
        {
            if (this.State == RailGuidedVehicle.RailGuidedVehicleStatus.输送线运行
                    || this.State == RailGuidedVehicle.RailGuidedVehicleStatus.无货运行
                    || this.State == RailGuidedVehicle.RailGuidedVehicleStatus.有货运行
                    )
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override bool IsWarnInfo()
        {
            return this.ErrorCode != 0 || this.State==RailGuidedVehicleStatus.报警停机;
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
            warning.DeviceType = "单货位穿梭车";

            return warning;
        }

        public override int GetErrorCode()
        {
            return this.ErrorCode;
        }

        
        static Alarm[] _alarms;

        public static Alarm[] GetAlarms()
        {
            lock (typeof(RailGuidedVehicleDiagnosisData))
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
    }
}
