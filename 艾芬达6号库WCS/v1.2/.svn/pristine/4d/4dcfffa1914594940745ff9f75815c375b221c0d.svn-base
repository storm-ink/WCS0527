using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Wcs;
using Wcs.DefaultImpls.Crane;

namespace WcsServiceForClient
{
    public class CraneServiceForClient : ICraneServiceForClient
    {
        public JsonOperationResult GetStatusByName(string deviceName)
        {
            try
            {
                var craneDevice = Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is CraneDevice)
                .Select(x => x.Device as CraneDevice)
                .Single(x => x.Name.Equals(deviceName, StringComparison.CurrentCultureIgnoreCase));

                if (craneDevice.LastStatus != null)
                {
                    var alarm = Wcs.Framework.DeviceErrorHelper.GetDeviceError(typeof(CraneDevice).GetDisplayName(), craneDevice.LastStatus.ErrorCode);

                    var errorDescription = "";
                    var errorSolution = "";
                    var errorName = "";
                    if (alarm != null)
                    {
                        errorName = alarm.ErrorName;
                        errorDescription = alarm.ErrorName;
                        errorSolution = alarm.Solution;
                    }

                    var location = craneDevice.Locations.Select(x=>(RackLocation)x).FirstOrDefault(x => x.Level == craneDevice.LastStatus.Level && x.Column == craneDevice.LastStatus.Column);

                    var status = new
                    {
                        Name = craneDevice.Name,
                        AtStation = craneDevice.LastStatus.AtStation,
                        ErrorCode = craneDevice.LastStatus.ErrorCode,
                        ErrorName = errorName,
                        ErrorDescription = errorDescription,
                        ErrorSolution = errorSolution,
                        Event = craneDevice.LastStatus.Event,
                        ForkHorizontalPosition = craneDevice.LastStatus.ForkHorizontalPosition,
                        ForkVerticalPosition = craneDevice.LastStatus.ForkVerticalPosition,
                        UserColumn = location == null ? 0 : location.UserColumn,
                        UserLevel = location == null ? 0 : location.UserLevel,
                        LockerIp = craneDevice.Locker.IPAddress,
                        LockerUser = craneDevice.Locker.UserName,
                        State = craneDevice.LastStatus.State,
                        TaskId = craneDevice.LastStatus.TaskId
                    };

                    return new JsonOperationResult(true, status);
                }
                else
                {
                    return new JsonOperationResult(false, "状态未同步", null);
                }
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false, ex.Message, null);
            }
        }

        public JsonOperationResult GetAllStatus()
        {
            try
            {
                List<dynamic> result = new List<dynamic>();
                foreach (var craneDevice in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                    .Where(x => x.Device is Wcs.DefaultImpls.Crane.CraneDevice)
                    .Select(x => x.Device as Wcs.DefaultImpls.Crane.CraneDevice))
                {

                    if (craneDevice.LastStatus != null)
                    {
                        var alarm = Wcs.Framework.DeviceErrorHelper.GetDeviceError(typeof(CraneDevice).GetDisplayName(), craneDevice.LastStatus.ErrorCode);

                        var errorDescription = "";
                        var errorSolution = "";
                        var errorName = "";
                        if (alarm != null)
                        {
                            errorName = alarm.ErrorName;
                            errorDescription = alarm.ErrorName;
                            errorSolution = alarm.Solution;
                        }

                        var location = craneDevice.Locations.Select(x => (RackLocation)x).FirstOrDefault(x => x.Level == craneDevice.LastStatus.Level && x.Column == craneDevice.LastStatus.Column);

                        var status = new
                        {
                            Name = craneDevice.Name,
                            AtStation = craneDevice.LastStatus.AtStation,
                            ErrorCode = craneDevice.LastStatus.ErrorCode,
                            ErrorName = errorName,
                            ErrorDescription = errorDescription,
                            ErrorSolution = errorSolution,
                            Event = craneDevice.LastStatus.Event,
                            ForkHorizontalPosition = craneDevice.LastStatus.ForkHorizontalPosition,
                            ForkVerticalPosition = craneDevice.LastStatus.ForkVerticalPosition,
                            UserColumn = location == null ? 0 : location.UserColumn,
                            UserLevel = location == null ? 0 : location.UserLevel,
                            LockerIp = craneDevice.Locker.IPAddress,
                            LockerUser = craneDevice.Locker.UserName,
                            State = craneDevice.LastStatus.State,
                            TaskId = craneDevice.LastStatus.TaskId
                        };

                        result.Add(status);
                    }
                    else
                    {
                        result.Add(new
                        {
                            Name = craneDevice.Name,
                            AtStation = false,
                            ErrorCode = 0,
                            ErrorName = "",
                            ErrorDescription = "",
                            ErrorSolution = "",
                            Event = 0,
                            ForkHorizontalPosition = 0,
                            ForkVerticalPosition = 0,
                            UserColumn = 0,
                            UserLevel = 0,
                            LockerIp = "",
                            LockerUser = "",
                            State = 0,
                            TaskId = "00000000"
                        });
                    }
                }

                return new JsonOperationResult(true, result);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false, ex.Message, null);
            }
        }
    }
}
