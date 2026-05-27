using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class Task : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public Task():base()
        {
            
        }
        /// <summary>
        /// 握手协议（为0时说明为地址为空，可以使用）
        /// </summary>
        [System.ComponentModel.DisplayName("握手变量")]
        public virtual TaskHandShake HandShake { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        [System.ComponentModel.DisplayName("任务号")]
        public virtual Int32 AssignmentID { get; set; }
        /// <summary>
        /// 托盘号
        /// </summary>
        [System.ComponentModel.DisplayName("托盘号")]
        public virtual Int32 TU_ID { get; set; }
        /// <summary>
        /// 托盘类型
        /// </summary>
        [System.ComponentModel.DisplayName("托盘类型")]
        public virtual Int32 TU_Type { get; set; }
        /// <summary>
        /// 业务数据
        /// </summary>
        [System.ComponentModel.DisplayName("业务数据")]
        public virtual Int32 IO_Data { get; set; }
        /// <summary>
        /// 路径号
        /// </summary>
        [System.ComponentModel.DisplayName("路径号")]
        public virtual Int32 RotingNo { get; set; }
        /// <summary>
        /// 起点位置
        /// </summary>
        [System.ComponentModel.DisplayName("起点位置")]
        public virtual Int32 StartMotorNo { get; set; }
        /// <summary>
        /// 终点位置
        /// </summary>
        [System.ComponentModel.DisplayName("终点位置")]
        public virtual Int32 DestinationNo { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        [System.ComponentModel.DisplayName("任务状态")]
        public virtual TaskStatus Status { get; set; }
        /// <summary>
        /// PLC读取任务后会填一下这个任务号，现在没用上好像
        /// </summary>
        [System.ComponentModel.DisplayName("读取号")]
        public virtual Int32 ReadTask { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        [System.ComponentModel.DisplayName("预留")]
        public virtual Int32 Spare { get; set; }
    }
}
