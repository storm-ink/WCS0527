using Matedata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DeviceServer.Services.SingleForkCraneDeviceService
{
    public class CraneServiceProxy : DiagnoseMatedataServiceProxy<ICraneService, CraneService>,ICraneService
    {
        public List<CraneInfo> LoadCraneInfos()
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                List<CraneInfo> reuslt = new List<CraneInfo>();
                foreach (var item in client.LoadCraneInfos())
                {
                    reuslt.Add(new CraneInfo
                    {
                        Name=item.Name,
                        MaxColumn=item.MaxColumn,
                        MaxLevel=item.MaxLevel,
                        MinColumn=item.MinColumn,
                        MinLevel=item.MinLevel
                    });
                }

                return reuslt;
            }
        }

        public Dictionary<string, LA> ReadStatus()
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                Dictionary<string, LA> result = new Dictionary<string, LA>();
                foreach (var item in client.ReadStatus())
                {
                    if (item.Value == null)
                    {
                        result.Add(item.Key, null);
                    }
                    else
                    {
                        result.Add(item.Key, new LA
                        {
                            AtStation = item.Value.AtStation,
                            ErrorCode = item.Value.ErrorCode,
                            ErrorDescription = item.Value.ErrorDescription,
                            ErrorName = item.Value.ErrorName,
                            ErrorSolution = item.Value.ErrorSolution,
                            Event = (Wcs.DefaultImpls.Crane.CraneEvent)((int)item.Value.Event),
                            ForkHorizontalPosition = (Wcs.DefaultImpls.Crane.ForkHorizontalPosition)((int)item.Value.ForkHorizontalPosition),
                            ForkVerticalPosition = (Wcs.DefaultImpls.Crane.ForkVerticalPosition)((int)item.Value.ForkVerticalPosition),
                            LockerIp = item.Value.LockerIp,
                            LockerUser = item.Value.LockerUser,
                            State = (Wcs.DefaultImpls.Crane.CraneStatus)((int)item.Value.State),
                            TaskId = item.Value.TaskId,
                            UserColumn = item.Value.UserColumn,
                            UserLevel = item.Value.UserLevel
                        });
                    }
                }

                return result;
            }
        }

        public void BackToTheOrigin(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.BackToTheOrigin(craneName, userName, ipAddress);
            }
        }

        public void EmergencyStop(string craneName)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.EmergencyStop(craneName);
            }
        }

        public void CancelEmergencyStop(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.CancelEmergencyStop(craneName, userName, ipAddress);
            }
        }

        public void Move(string craneName, string userName, string ipAddress, int userColumn, int userLevel)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Move(craneName, userName, ipAddress, userColumn, userLevel);
            }
        }

        public void Pick(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Pick(craneName, userName, ipAddress, forkDirection);
            }
        }

        public void Putdown(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Putdown(craneName, userName, ipAddress, forkDirection);
            }
        }

        public void Lock(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Lock(craneName, userName, ipAddress);
            }
        }

        public void Unlock(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Unlock(craneName, userName, ipAddress);
            }
        }

        public void Up(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Up(craneName, userName, ipAddress);
            }
        }

        public void Down(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Down(craneName, userName, ipAddress);
            }
        }

        public void Forward(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Forward(craneName, userName, ipAddress);
            }
        }

        public void Back(string craneName, string userName, string ipAddress)
        {
            using (OldCraneService.CraneServiceClient client = new OldCraneService.CraneServiceClient())
            {
                client.Back(craneName, userName, ipAddress);
            }
        }

        public override string EndpointConfigurationName
        {
            get { return "*"; }
        }
    }
}
