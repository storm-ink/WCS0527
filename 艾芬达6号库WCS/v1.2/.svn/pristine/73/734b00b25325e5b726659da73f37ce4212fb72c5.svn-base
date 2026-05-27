using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Wcs.DefaultImpls.Crane;

namespace CraneService
{

    [DataContract(Name = "LA")]
    public class LA
    {
        /// <summary>
        /// 堆垛机状态
        /// </summary>
        [DataMember]
        public CraneStatus State { get; set; }
        /// <summary>
        /// 当前所在列
        /// </summary>
        [DataMember]
        public String UserColumn { get; set; }
        /// <summary>
        /// 当前所在层
        /// </summary>
        [DataMember]
        public String UserLevel { get; set; }
        /// <summary>
        /// 货叉水平位置状态
        /// </summary>
        [DataMember]
        public ForkHorizontalPosition ForkHorizontalPosition { get; set; }
        /// <summary>
        /// 货叉上下位置状态
        /// </summary>
        [DataMember]
        public ForkVerticalPosition ForkVerticalPosition { get; set; }
        /// <summary>
        /// 指示堆垛机当前是否在站点位置
        /// </summary>
        [DataMember]
        public Boolean AtStation { get; set; }
        /// <summary>
        /// 错误码（默认为 0）
        /// </summary>
        [DataMember]
        public Int32 ErrorCode { get; set; }
        [DataMember]
        public String ErrorName { get; set; }
        [DataMember]
        public String ErrorDescription { get; set; }
        [DataMember]
        public String ErrorSolution { get; set; }
        [DataMember]
        public String LockerUser { get; set; }
        [DataMember]
        public String LockerIp { get; set; }
        /// <summary>
        /// 堆垛机事件
        /// </summary>
        [DataMember]
        public CraneEvent Event { get; set; }
        /// <summary>
        /// 当前任务号
        /// </summary>
        [DataMember]
        public String TaskId { get; set; }
    }
}
