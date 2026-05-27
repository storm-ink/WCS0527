using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcsServiceForClient
{
    [DataContract]
    public class OperationResult
    {
        [DataMember]
        public Boolean Success { get; set; }
        [DataMember]
        public String Message { get; set; }
        public OperationResult(Boolean success):this(success,null)
        {

        }

        public OperationResult(Boolean success, String message)
        {
            this.Success = success;
            this.Message = message;
        }
    }
}
