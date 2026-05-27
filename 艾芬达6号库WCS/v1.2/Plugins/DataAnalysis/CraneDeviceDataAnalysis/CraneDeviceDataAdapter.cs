using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Wcs.App.Plugins.DataAnalysis.CraneDeviceDataAnalysis
{
    public class CraneDeviceChartDataAdapter:IChartDataAdapter
    {
        public IEnumerable<StateTime> FindStateTimes(string deviceName, DateTime? from, DateTime? to)
        {
            string sql = @"select [createdat],[RequestStateCommandReplyDataLog_State] as [state],deviceName
  from dbo.ReceivedDataLogs 
  where (devicename=@deviceName or @deviceName is null) and discriminator='RequestStateCommandReplyDataLog'
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
                        cmd.Parameters.AddWithValue("deviceName", string.IsNullOrWhiteSpace(deviceName) ? DBNull.Value : (object)deviceName);
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
                    DeviceName = Convert.ToString(x["deviceName"]),
                    State = Convert.ToString(x["state"]),
                    StateDisplayName=ConvertStateToDescription(Convert.ToString(x["state"])),
                    StartTime = Convert.ToDateTime(x["createdAt"])
                })
                .ToList();
        }
        
        public IEnumerable<StateExecutionTime> FindStateExecutionTimes(string deviceName, DateTime? from, DateTime? to)
        {
            List<StateExecutionTime> result = new List<StateExecutionTime>();
            var stateTimes = FindStateTimes(deviceName, from, to);
            stateTimes = stateTimes.OrderBy(x => x.StartTime);

            StateExecutionTime prevStateExecutionTime=null;
            foreach (var itemGrouping in stateTimes.GroupBy(x=>x.DeviceName))
            {
                foreach (var item in itemGrouping.OrderBy(x => x.StartTime))
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
                            if (item.StartTime > prevStateExecutionTime.FromState.StartTime)
                            {
                                prevStateExecutionTime.ToState = item;
                                result.Add(prevStateExecutionTime);
                            }
                            prevStateExecutionTime = null;
                        }
                    }                    
                }
            }

            return result.OrderBy(x=>x.FromState.StartTime);
        }

        public IEnumerable<ErrorTime> FindErrorTimes(string deviceName, DateTime? from, DateTime? to)
        {
            string sql = @"select devicename,[createdat],[RequestStateCommandReplyDataLog_errorCode] as [state]
  from dbo.ReceivedDataLogs 
  where (devicename=@deviceName or @deviceName is null) and discriminator='RequestStateCommandReplyDataLog'
  and CreatedAt between @fromDate and @endDate
  and [RequestStateCommandReplyDataLog_errorCode] is not null
  and [RequestStateCommandReplyDataLog_errorCode]<>0
  order by CreatedAt asc";

            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["wcs"].ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("deviceName", string.IsNullOrWhiteSpace(deviceName) ? DBNull.Value : (object)deviceName);
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
                .Select(x => new ErrorTime
                {
                    Code  = Convert.ToString(x["state"]),
                    Name = getAlarm(Convert.ToString(x["state"])),
                    StartTime = Convert.ToDateTime(x["createdAt"]),
                    DeviceName = Convert.ToString(x["deviceName"])
                })
                .ToList();
        }

        public IEnumerable<ErrorExecutionTime> FindErrorExecutionTimes(string deviceName, DateTime? from, DateTime? to)
        {
            List<ErrorExecutionTime> result = new List<ErrorExecutionTime>();
            var stateTimes = FindErrorTimes(deviceName, from, to);
            stateTimes = stateTimes.OrderBy(x => x.StartTime);

            ErrorExecutionTime prevStateExecutionTime = null;
            foreach (var itemGrouping in stateTimes.GroupBy(x => x.DeviceName))
            {
                foreach (var item in itemGrouping.OrderBy(x => x.StartTime))
                {
                    if (prevStateExecutionTime == null)
                    {
                        prevStateExecutionTime = new ErrorExecutionTime
                        {
                            FromState = item
                        };
                    }
                    else
                    {
                        if (item.Code != prevStateExecutionTime.FromState.Code)
                        {
                            if (item.StartTime > prevStateExecutionTime.FromState.StartTime)
                            {
                                prevStateExecutionTime.ToState = item;
                                result.Add(prevStateExecutionTime);
                            }
                            prevStateExecutionTime = null;
                        }
                    }
                }
            }

            return result.OrderBy(x => x.FromState.StartTime);
        }

        public IEnumerable<TaskTime> FindTaskTimes(string deviceName, DateTime? from, DateTime? to)
        {
            string sql = @"select [createdat],[RequestStateCommandReplyDataLog_Event] as [state],[RequestStateCommandReplyDataLog_TaskId] as taskId
  from dbo.ReceivedDataLogs 
  where devicename=@deviceName and discriminator='RequestStateCommandReplyDataLog'
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
                .Select(x => new TaskTime
                {
                    TaskId = Convert.ToString(x["taskid"]),
                    State = Convert.ToString(x["state"]),
                    StartTime = Convert.ToDateTime(x["createdAt"])
                })
                .ToList();
        }

        public IEnumerable<TaskExecutionTime> FindTaskExecutionTimes(string deviceName, DateTime? from, DateTime? to)
        {
            List<TaskExecutionTime> result = new List<TaskExecutionTime>();
            var stateTimes = FindTaskTimes(deviceName, from, to);
            stateTimes = stateTimes.OrderBy(x => x.StartTime);

            TaskExecutionTime prevStateExecutionTime = null;
            foreach (var item in stateTimes)
            {
                if (prevStateExecutionTime == null)
                {
                    prevStateExecutionTime = new TaskExecutionTime
                    {
                        FromState = item
                    };
                }
                else
                {
                    if (item.TaskId != prevStateExecutionTime.FromState.TaskId || item.State=="6")
                    {
                        prevStateExecutionTime.ToState = item;
                        result.Add(prevStateExecutionTime);
                        prevStateExecutionTime = null;
                    }
                }
            }

            return result.OrderBy(x => x.FromState.StartTime);
        }

        string ConvertStateToDescription(object state)
        {
            string v = Convert.ToString(state);
            switch (v)
            {
                case "0":
                    return "初始化";
                case "1":
                    return "回原点";
                case "2":
                    return "无货待命";
                case "3":
                    return "有货待命";
                case "4":
                    return "无货运行";
                case "5":
                    return "有货运行";
                case "6":
                    return "取货";
                case "7":
                    return "放货";
                case "8":
                    return "报警停机";
                case "9":
                    return "报警复位";
                case "10":
                    return "???";
                case "11":
                    return "未连接";
                case "12":
                    return "手动操作";
                default:
                    return "未协定";
            }
        }
        
        string getAlarm(string code)
        {
            if (code == "0")
            {
                return "正常";
            }

            var files = Directory.GetFiles(Application.StartupPath, "alarms.xml", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                return null;
            }

            if (files.Length > 1)
            {
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(files[0]);

            var node = doc.SelectSingleNode("/alarms/alarm[@code='" + code + "']");
            if (node == null)
            {
                return null;
            }

            if (node.Attributes["name"] == null)
            {
                return null;
            }

            return node.Attributes["name"].Value;
        }


    }
}
