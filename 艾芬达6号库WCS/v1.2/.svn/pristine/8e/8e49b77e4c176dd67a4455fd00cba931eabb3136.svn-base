using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matedata
{
    public abstract class DiagnoseMatedataService : MatedataService,IMatedataService
    {

        public virtual String ConnectionString
        {
            get
            {
                return GetConnectionString("diagnoseMatedata");
            }
        }

        public virtual Int32 MaxBatchSize
        {
            get
            {
                return GetAppSetting<Int32>("maxBatchSize", 1000);
            }
        }

        public abstract String SelectCommandText { get; }

        public abstract string GetDiagnoseMatedatas(int minDataId, int batchSize, String appInfo);
        public virtual List<Newtonsoft.Json.Linq.JObject> ToJObjects(System.Data.SqlClient.SqlDataReader reader)
        {
            List<Newtonsoft.Json.Linq.JObject> result = new List<Newtonsoft.Json.Linq.JObject>();

            while (reader.Read())
            {
                Newtonsoft.Json.Linq.JObject obj = new Newtonsoft.Json.Linq.JObject();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    obj.Add(reader.GetName(i), new Newtonsoft.Json.Linq.JValue(reader[i]));
                }

                result.Add(obj);
            }

            return result;
        }
    }
}
