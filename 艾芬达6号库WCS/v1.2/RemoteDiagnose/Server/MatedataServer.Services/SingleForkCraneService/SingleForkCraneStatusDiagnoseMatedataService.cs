using Matedata;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MatedataServer.Services.SingleForkCraneService
{
    public class SingleForkCraneStatusDiagnoseMatedataService : DiagnoseMatedataService
    {
        public override String GetDiagnoseMatedatas(int minDataId, int batchSize, String appInfo)
        {
            if (batchSize <= 0 || batchSize >= this.MaxBatchSize)
            {
                batchSize = this.MaxBatchSize;
            }

            if (string.IsNullOrEmpty((appInfo + "").Trim()))
            {
                throw new ArgumentNullException("appInfo 值不能为空，该值应该为子类的 discriminator-value 值，而查询语句也应该使用该值为键名配置。");
            }

            String sql = this.GetAppSetting<String>(appInfo);

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
                throw new NotImplementedException();
            }
        }
    }
}
