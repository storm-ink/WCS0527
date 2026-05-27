using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcsServiceForClient
{
    [DataContract]
    public class JsonOperationResult:OperationResult
    {
        [DataMember]
        public String JsonString { get; set; }
        public JsonOperationResult(Boolean success, Object obj)
            :this(success,null,obj)
        {

        }
        public JsonOperationResult(Boolean success, String message, Object obj)
            :base(success,message)
        {
            string json;
            if (!(obj is string))
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            else
            {
                json = Convert.ToString(obj);
            }

            this.JsonString = json;
        }
    }

}
