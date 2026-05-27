using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class DeviceDataHelper
    {
        public static DeviceStatusData Parse(DataRow row)
        {
            switch (Convert.ToString(row["discriminator"]))
            {
                case "LocationTaskDataLog":
                    return new LocationTaskData(row);
                case "AlarmDataLog":
                    return new AlarmData(row);
                case  "HoldSignalDataLog":
                    return new HoldSignalData(row);
                case "LocationDataLog":
                    return new LocationData(row);
                case "OccupyDataLog":
                    return new OccupyData(row);
                case "RequestStateCommandReplyDataLog":
                    return new RequestStateCommandReply(row);
                case "TaskDataLog":
                    return new TaskData(row);
                default:
                    return new ObjectData(row);
            }
        }
    }
}
