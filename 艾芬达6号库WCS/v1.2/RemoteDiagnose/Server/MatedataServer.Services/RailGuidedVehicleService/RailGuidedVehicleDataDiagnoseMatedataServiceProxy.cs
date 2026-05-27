using Matedata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatedataServer.Services.RailGuidedVehicleService
{
    /// <summary>
    /// 穿梭车诊断数据传输服务代理服务类
    /// </summary>
    public class RailGuidedVehicleDataDiagnoseMatedataServiceProxy : DiagnoseMatedataServiceProxy<IMatedataService, RailGuidedVehicleDataDiagnoseMatedataService>
    {
        public override string EndpointConfigurationName
        {
            get { return typeof(RailGuidedVehicleDataDiagnoseMatedataService).Name; }
        }
    }
}
