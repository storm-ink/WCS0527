using Matedata;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MatedataServer.Services.WcsLog
{
    public class WcsLogsDiagnoseMatedataServiceProxy : Matedata.DiagnoseMatedataServiceProxy<Matedata.IMatedataService, WcsLogsDiagnoseMatedataService>
    {
        public override string EndpointConfigurationName
        {
            get { return typeof(WcsLogsDiagnoseMatedataService).Name; }
        }
    }
}
