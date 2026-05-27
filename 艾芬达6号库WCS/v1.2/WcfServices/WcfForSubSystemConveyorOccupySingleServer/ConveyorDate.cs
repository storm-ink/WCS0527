using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfForSubSystemConveyorOccupySingleServer
{
    /// <summary>
    /// 输送线数据结构体
    /// </summary>
    [Serializable]
    [DataContract]
    public struct ConveyorDate
    {
        /// <summary>
        /// 货位号
        /// </summary>
        [DataMember]
        public Int16 PosNo;
        /// <summary>
        /// 货位状态：可以=0；不能放货=1；
        /// </summary>
        [DataMember]
        public Int16 Status;
        /// <summary>
        /// 预留
        /// </summary>
        [DataMember]
        public UInt16 Spare;
    }
}
