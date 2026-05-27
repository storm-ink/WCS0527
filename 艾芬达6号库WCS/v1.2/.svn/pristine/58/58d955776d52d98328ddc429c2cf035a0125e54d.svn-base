using Matedata;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MatedataServer.Services.WcsLog
{
    public class WcsLogsDiagnoseMatedataService : DiagnoseMatedataService
    {
        public override String GetDiagnoseMatedatas(int minDataId, int batchSize,string appInfo)
        {
            if (batchSize <= 0 || batchSize >= this.MaxBatchSize)
            {
                batchSize = this.MaxBatchSize;
            }

            String sql = this.SelectCommandText;

            sql = sql.Replace("{batchSize}", batchSize.ToString());

            object result;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        public override string SelectCommandText
        {
            get
            {
                var v = GetAppSetting<String>("selectCommandText");
                if (string.IsNullOrEmpty(v))
                {
                    return @"select top {batchSize} * from logs where id>=@id order by id asc";
                }
                else
                {
                    return v;
                }
            }
        }
    }
}
