using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CraneService.TwoForksCrane
{
    [DataContract]
    public class CraneInfo
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Int32 MinColumn { get; set; }
        [DataMember]
        public Int32 MaxColumn { get; set; }
        [DataMember]
        public Int32 MinLevel { get; set; }
        [DataMember]
        public Int32 MaxLevel { get; set; }
    }
}
