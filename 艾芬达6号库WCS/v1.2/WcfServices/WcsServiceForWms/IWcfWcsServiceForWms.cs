using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcsServiceForWms
{
    [ServiceContract]
    public partial interface IWcfWcsServiceForWms
    {
        [OperationContract]
        bool SendTask(DC2.WcsTaskInfo task);

        //[OperationContract]
        //DC2.PathStatus[] FindPathStatus(string startLocation, string[] endLocations);
    }
}
