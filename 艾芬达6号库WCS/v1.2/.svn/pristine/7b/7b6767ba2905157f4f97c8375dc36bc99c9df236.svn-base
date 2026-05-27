using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoForksRackLocations.asdf
{
    public class UserLocationRelationship
    {
        public Int32 FromUserColumn { get; set; }
        public Int32 ToUserColumn { get; set; }
        public Int32 FromUserLevel { get; set; }
        public Int32 ToUserLevel { get; set; }

        public Int32 FromColumn { get; set; }
        public Int32 FromLevel { get; set; }

        public Boolean 列递减 { get; set; }

        public stepInfo Step1 { get; set; }
        public stepInfo Step2 { get; set; }
        public stepInfo Step3 { get; set; }
        public Int32 组大小 { get; set; }
    }

    public class stepInfo
    {
        public Int32 每组中隔几列 { get; set; }
        public Boolean 定位 { get; set; }
        public Boolean 叉1能用 { get; set; }
        public Boolean 叉2能用 { get; set; }
    }
}
