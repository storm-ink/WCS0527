using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.DataAnalysis.CraneDeviceDataAnalysis
{
    public class ConveyorDeviceDataAdapter:IChartDataAdapter
    {
        public IEnumerable<StateTime> FindStateTimes(string deviceName, DateTime? from, DateTime? to)
        {
            string sql = @"select [createdat],LocationDataLog_PosNo as posNo,LocationDataLog_Status as [state]
  from dbo.ReceivedDataLogs 
  where devicename=@deviceName and discriminator='LocationDataLog'
  and CreatedAt between @fromDate and @endDate
  order by CreatedAt asc";

            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["wcs"].ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("deviceName", deviceName);
                        cmd.Parameters.AddWithValue("fromDate", from ?? DateTime.Parse("1900-01-01"));
                        cmd.Parameters.AddWithValue("endDate", to ?? DateTime.MaxValue);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    trans.Commit();
                }
            }

            return dataTable.Rows.Cast<DataRow>()
                .Select(x => new StateTime
                {
                    DeviceName = Convert.ToString("posNo"),
                    State = Convert.ToString(x["state"]),
                    StartTime = Convert.ToDateTime(x["createdAt"])
                })
                .ToList();
        }


        public IEnumerable<StateExecutionTime> FindStateExecutionTimes(string deviceName, DateTime? from, DateTime? to)
        {
            List<StateExecutionTime> result = new List<StateExecutionTime>();
            var stateTimes = FindStateTimes(deviceName, from, to);
            stateTimes = stateTimes.OrderBy(x => x.StartTime);

            var locations = stateTimes
                .GroupBy(x => x.DeviceName)
                .Select(x => x.Key);

            foreach (var loc in locations)
            {
                var locationStateTimes = stateTimes
                    .Where(x => x.DeviceName == loc)
                    .OrderBy(x => x.StartTime);

                StateExecutionTime prevStateExecutionTime = null;
                foreach (var item in locationStateTimes)
                {
                    if (prevStateExecutionTime == null)
                    {
                        prevStateExecutionTime = new StateExecutionTime
                        {
                            FromState = item
                        };
                    }
                    else
                    {
                        if (item.State != prevStateExecutionTime.FromState.State)
                        {
                            prevStateExecutionTime.ToState = item;
                            result.Add(prevStateExecutionTime);
                            prevStateExecutionTime = null;
                        }
                    }
                }
            }

            return result;
        }

        public string ConvertStateToDescription(object state)
        {
            string v = Convert.ToString(state);
            switch (v)
            {
                case "0":
                    return "初始化";
                case "1":
                    return "报警";
                case "2":
                    return "离线";
                case "3":
                    return "手动";
                case "4":
                    return "停止";
                case "5":
                    return "运行";
                default:
                    return "未协定";
            }
        }


        public Dictionary<object, string> GetAllStatus()
        {
            Dictionary<object, string> result = new Dictionary<object, string>();
            result.Add("0", "初始化");
            result.Add("1", "报警");
            result.Add("2", "离线");
            result.Add("3", "手动");
            result.Add("4", "停止");
            result.Add("5", "运行");
            return result;
        }


        public IEnumerable<StateTime> FindErrorTimes(string deviceName, DateTime? from, DateTime? to)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StateExecutionTime> FindErrorExecutionTimes(string deviceName, DateTime? from, DateTime? to)
        {
            throw new NotImplementedException();
        }
    }
}
