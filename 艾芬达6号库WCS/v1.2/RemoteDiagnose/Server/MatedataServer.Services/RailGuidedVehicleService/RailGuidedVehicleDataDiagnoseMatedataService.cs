using Matedata;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace MatedataServer.Services.RailGuidedVehicleService
{
    /// <summary>
    /// 穿梭车诊断数据传输服务
    /// </summary>
    public class RailGuidedVehicleDataDiagnoseMatedataService : DiagnoseMatedataService
    {
        static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public override string GetDiagnoseMatedatas(int minDataId, int batchSize, String appInfo)
        {
            try
            {
                if (batchSize <= 0 || batchSize >= this.MaxBatchSize)
                {
                    batchSize = this.MaxBatchSize;
                }

                String sql = this.SelectCommandText;
                sql = sql.Replace("{batchSize}", batchSize.ToString());

                List<Newtonsoft.Json.Linq.JObject> result;
                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("id", minDataId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                result = ToJObjects(reader);
                            }
                        }
                        trans.Commit();
                    }
                }

                _logger.Info("minDataId = {0}, batchSize = {1}, appInfo = {2}, result = {3}", minDataId, batchSize, appInfo, result.Count);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _logger.Error("minDataId = {0}, batchSize = {1}, appInfo = {2}.{3}",minDataId,batchSize,appInfo,ex);

                throw;
            }
        }

        public override string SelectCommandText
        {
            get
            {
                var v = GetAppSetting<String>("selectCommandText");
                if (string.IsNullOrEmpty(v))
                {
                    return @"SELECT top {batchSize} [Id]
                              ,[DeviceName]
                              ,[CreatedAt]
                              ,[StateTelexTransferObjectDataLog_Position] AS [Position]
                              ,[StateTelexTransferObjectDataLog_CurrentStation] AS [CurrentStation]
                              ,[StateTelexTransferObjectDataLog_AtStation] AS [AtStation]
                              ,[StateTelexTransferObjectDataLog_ErrorCode] AS [ErrorCode]
                              ,[StateTelexTransferObjectDataLog_State] AS [State]
                              ,[StateTelexTransferObjectDataLog_Event] AS [Event]
                              ,[StateTelexTransferObjectDataLog_TaskId] AS [TaskId]
                              ,[StateTelexTransferObjectDataLog_ContainerCode] AS [ContainerCode]
                              ,[StateTelexTransferObjectDataLog_FromStation] AS [FromStation]
                              ,[StateTelexTransferObjectDataLog_ToStation] AS [ToStation]
                              ,[StateTelexTransferObjectDataLog_TaskMode] AS [TaskMode]
                              ,[StateTelexTransferObjectDataLog_Telex] AS [Telex]
                          FROM [ReceivedDataLogs] where [discriminator]='StateTelexTransferObjectDataLog' and id>=@id";
                }
                else
                {
                    return v;
                }
            }
        }
    }
}
