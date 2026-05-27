using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatedataServer.Services.SingleForkCraneService
{
    public class SingleForkCraneStatusDiagnoseMatedataServiceProxy : Matedata.DiagnoseMatedataServiceProxy<Matedata.IMatedataService, SingleForkCraneStatusDiagnoseMatedataService>
    {
        public override string EndpointConfigurationName
        {
            get { return typeof(SingleForkCraneStatusDiagnoseMatedataService).Name; }
        }
    }
}
