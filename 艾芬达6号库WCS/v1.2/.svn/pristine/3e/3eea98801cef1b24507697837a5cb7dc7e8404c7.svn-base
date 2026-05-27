using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class OccupyData:DeviceStatusData
    {
        public String PosNo { get; private set; }
        public String PhocllUseStatus { get; private set; }
        public String FroProPotocell { get; private set; }
        public String FroPosPotocell { get; private set; }
        public String FroSloPotocell { get; private set; }
        public String AftProPotocell { get; private set; }
        public String AftPosPotocell { get; private set; }
        public String AftSloPotocell { get; private set; }
        public String UpPotocell { get; private set; }
        public String DownPotocell { get; private set; }

        public OccupyData(DataRow row)
            : base(row)
        {
            if (Convert.ToString(row["discriminator"]) != "OccupyDataLog")
            {
                throw new InvalidOperationException("此行无法转换为 OccupyData 类型");
            }

            this.PosNo = Convert.ToString(row["OccupyDataLog_PosNo"]);
            this.PhocllUseStatus = Convert.ToString(row["OccupyDataLog_PhocllUseStatus"]);
            this.FroProPotocell = Convert.ToString(row["OccupyDataLog_FroProPotocell"]);
            this.FroPosPotocell = Convert.ToString(row["OccupyDataLog_FroPosPotocell"]);
            this.FroSloPotocell = Convert.ToString(row["OccupyDataLog_FroSloPotocell"]);
            this.AftProPotocell = Convert.ToString(row["OccupyDataLog_AftProPotocell"]);
            this.AftPosPotocell = Convert.ToString(row["OccupyDataLog_AftPosPotocell"]);
            this.AftSloPotocell = Convert.ToString(row["OccupyDataLog_AftSloPotocell"]);
            this.UpPotocell = Convert.ToString(row["OccupyDataLog_UpPotocell"]);
            this.DownPotocell = Convert.ToString(row["OccupyDataLog_DownPotocell"]);
        }

        public override string ToDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}：{1}\n", "货位号", this.PosNo);
            sb.AppendFormat("{0}：{1}\n", "前保护", this.FroProPotocell);
            sb.AppendFormat("{0}：{1}\n", "前到位", this.FroPosPotocell);
            sb.AppendFormat("{0}：{1}\n", "前减速", this.FroSloPotocell);
            sb.AppendFormat("{0}：{1}\n", "后保护", this.AftProPotocell);
            sb.AppendFormat("{0}：{1}\n", "后到位", this.AftPosPotocell);
            sb.AppendFormat("{0}：{1}\n", "后减速", this.AftSloPotocell);
            sb.AppendFormat("{0}：{1}\n", "高位", this.UpPotocell);
            sb.AppendFormat("{0}：{1}\n", "低位", this.DownPotocell);

            return sb.ToString();
        }
    }
}
