using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Wcs.DefaultImpls.Crane;
using Wcs;
namespace CraneService
{
    public class CraneService:ICraneService
    {
        static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public List<CraneInfo> LoadCraneInfos()
        {
            List<CraneInfo> result = new List<CraneInfo>();
            foreach (var craneDevice in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is Wcs.DefaultImpls.Crane.CraneDevice
                    && !(x.Device is Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice))
                .Select(x => x.Device as Wcs.DefaultImpls.Crane.CraneDevice))
            {
                var info = new CraneInfo();
                info.Name = craneDevice.Name;
                if (craneDevice.Locations.Length > 0)
                {
                    info.MinColumn = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Min(x => ((RackLocation)x).UserColumn);
                    info.MaxColumn = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Max(x => ((RackLocation)x).UserColumn);
                    info.MinLevel = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Min(x => ((RackLocation)x).UserLevel);
                    info.MaxLevel = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Max(x => ((RackLocation)x).UserLevel);
                }
                result.Add(info);
            }

            return result;
        }

        public Dictionary<String,LA> ReadStatus()
        {
            Dictionary<String, LA> result = new Dictionary<String,LA>();
            foreach (var craneDevice in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => 
                    x.Device is Wcs.DefaultImpls.Crane.CraneDevice 
                    && !(x.Device is Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice))
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

                    var location = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).Level == craneDevice.LastStatus.Level && ((RackLocation)x).Column == craneDevice.LastStatus.Column);

                    LA la = new LA
                    {
                        AtStation = craneDevice.LastStatus.AtStation,
                        ErrorCode = craneDevice.LastStatus.ErrorCode,
                        ErrorName= errorName, 
                        ErrorDescription=errorDescription,
                        ErrorSolution = errorSolution,
                        Event = craneDevice.LastStatus.Event,
                        ForkHorizontalPosition = craneDevice.LastStatus.ForkHorizontalPosition,
                        ForkVerticalPosition = craneDevice.LastStatus.ForkVerticalPosition,
                        UserColumn = location==null?"?":location.UserColumn.ToString("000"),
                        UserLevel = location == null ? "?" : location.UserLevel.ToString("000"),
                        LockerIp = craneDevice.Locker.IPAddress,
                        LockerUser = craneDevice.Locker.UserName,
                        State = craneDevice.LastStatus.State,
                        TaskId = craneDevice.LastStatus.TaskId
                    };

                    result.Add(craneDevice.Name, la);
                }
                else
                {
                    result.Add(craneDevice.Name,null);
                }
            }

            return result;
        }

        public void BackToTheOrigin(string craneName, string userName,string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的回原点命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);
                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送回原点命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                if (craneDevice.LastStatus.State == CraneStatus.ManualMode)
                {
                    throw new InvalidOperationException(string.Format("{0} 堆垛机正处于手动模式, 不能下发任务。", craneDevice));
                }
                
                if (craneDevice.LastStatus.ForkHorizontalPosition != ForkHorizontalPosition.Center)
                {
                    throw new InvalidOperationException(string.Format("{0} 货叉不在中位, 不能下发任务。", craneDevice));
                }

                if (craneDevice.WithRemoteController)
                {
                    if (!craneDevice.WithRemoteControllerOK)
                        throw new InvalidOperationException(string.Format(" {0} 远程控制盒处于 {1} 状态, 不能下发任务。", craneDevice, String.Join(",", craneDevice.WithRemoteControllerInfo.Select(x => x.Key + ":" + x.Value))));
                }
                
                craneDevice.BackToTheOrigin(new Wcs.Framework.LockerInfo(userName, ipAddress));

                _logger.Trace1("回原点命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void EmergencyStop(string craneName)
        {
            _logger.Trace1(string.Format("收到了一个 {0} 急停命令",craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);
                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送急停命令。", craneDevice));
                }

                craneDevice.EmergencyStop();

                _logger.Trace1("急停命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void CancelEmergencyStop(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的取消急停命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);
                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送取消急停命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                craneDevice.CancelEmergencyStop(new Wcs.Framework.LockerInfo(userName,ipAddress));

                _logger.Trace1("取消急停命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void Move(string craneName, string userName, string ipAddress, int userColumn, int userLevel)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的移动到 {3}列{4}层 的命令", ipAddress, userName, craneName,userColumn,userLevel), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);


                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送移动命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int currentLevel = craneDevice.LastStatus.Level;
                int currentColumn = craneDevice.LastStatus.Column;
                var formLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).Column == currentColumn && ((RackLocation)x).Level == currentLevel);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层这个位置。", craneDevice, currentColumn, currentLevel));
                }

                var toLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).UserColumn == userColumn && ((RackLocation)x).UserLevel == userLevel);
                if (toLocation==null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层 这个位置。", craneDevice,userColumn,userLevel));
                }

                _logger.Trace1(string.Format("本次移动的起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("移动命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void MoveByForkDirection(string craneName, string userName, string ipAddress, int userColumn, int userLevel, ForkDirection forkDirection)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的移动到 {5} {3}列{4}层 的命令", ipAddress, userName, craneName, userColumn, userLevel, forkDirection), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);


                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送移动命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int currentLevel = craneDevice.LastStatus.Level;
                int currentColumn = craneDevice.LastStatus.Column;
                var formLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).Column == currentColumn && ((RackLocation)x).Level == currentLevel);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层这个位置。", craneDevice, currentColumn, currentLevel));
                }

                var toLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).UserColumn == userColumn && ((RackLocation)x).UserLevel == userLevel && ((RackLocation)x).ForkDirection == forkDirection);
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层向 {3} 这个位置。", craneDevice, userColumn, userLevel,forkDirection));
                }

                _logger.Trace1(string.Format("本次移动的起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("移动命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void Pick(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的向 {0} 的取货命令", ipAddress, userName, craneName, forkDirection.GetDescription()), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送取货命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int level = craneDevice.LastStatus.Level;
                int column = craneDevice.LastStatus.Column;

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1}列{2}层为目的地",craneDevice,column,level), this);

                var formLocation = (RackLocation)craneDevice.Locations.SingleOrDefault(x => ((RackLocation)x).Column == column && ((RackLocation)x).Level == level && ((RackLocation)x).ForkDirection == forkDirection);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层货叉{3}这个位置。", craneDevice, column, level, forkDirection.GetDescription()));
                }

                _logger.Trace1(string.Format("本次取货目标位置为 {0}", formLocation), this);


                craneDevice.Pick(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation);

                _logger.Trace1("取货命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void Putdown(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的向 {0} 的放货命令", ipAddress, userName, craneName, forkDirection.GetDescription()), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送放货命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int level = craneDevice.LastStatus.Level;
                int column = craneDevice.LastStatus.Column;

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1}列{2}层为目的地", craneDevice, column, level), this);

                var toLocation = (RackLocation)craneDevice.Locations.SingleOrDefault(x => ((RackLocation)x).Column == column && ((RackLocation)x).Level == level && ((RackLocation)x).ForkDirection == forkDirection);
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层货叉{3}这个位置。", craneDevice, column, level, forkDirection.GetDescription()));
                }

                _logger.Trace1(string.Format("本次放货目标位置为 {0}", toLocation), this);


                craneDevice.Putdown(new Wcs.Framework.LockerInfo(userName, ipAddress), toLocation);

                _logger.Trace1("放货命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void Lock(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的向 {0} 的锁定命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                craneDevice.Lock(new Wcs.Framework.LockerInfo(userName, ipAddress));

                _logger.Trace1("锁定成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void Unlock(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的向 {0} 的解除锁定命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                craneDevice.Unlock(new Wcs.Framework.LockerInfo(userName, ipAddress));

                _logger.Trace1("解除锁定成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ;
            }
        }
        
        public void Up(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的提升命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送提升命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int level = craneDevice.LastStatus.Level;
                int column = craneDevice.LastStatus.Column;
                var formLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).Column == column && ((RackLocation)x).Level == level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);


                var toLocation = (RackLocation)craneDevice.Locations.Where(x => ((RackLocation)x).UserColumn == formLocation.UserColumn && ((RackLocation)x).UserLevel > formLocation.UserLevel)
                    .OrderBy(x => ((RackLocation)x).UserLevel)
                    .FirstOrDefault();
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 已无法再提升层位置，请检查 {1} 是否已运行至提升极限", formLocation,craneDevice));
                }

                _logger.Trace1(string.Format("本次提升起始位置为 {0}，目标位置为 {1}", formLocation,toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation,toLocation);

                _logger.Trace1("提升命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ;
            }
        }

        public void Down(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的下降命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送下降命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int level = craneDevice.LastStatus.Level;
                int column = craneDevice.LastStatus.Column;
                var formLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).Column == column && ((RackLocation)x).Level == level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = (RackLocation)craneDevice.Locations.Where(x => ((RackLocation)x).UserColumn == formLocation.UserColumn && ((RackLocation)x).UserLevel < formLocation.UserLevel)
                    .OrderByDescending(x => ((RackLocation)x).UserLevel)
                    .FirstOrDefault();
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 已无法再下降层位置，请检查 {1} 是否已运行至下降极限", formLocation, craneDevice));
                }

                _logger.Trace1(string.Format("本次下降起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("下降命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ;
            }
        }

        public void Forward(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的前进命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送前进命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int level = craneDevice.LastStatus.Level;
                int column = craneDevice.LastStatus.Column;
                var formLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).Column == column && ((RackLocation)x).Level == level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = (RackLocation)craneDevice.Locations.Where(x => ((RackLocation)x).UserLevel == formLocation.UserLevel && ((RackLocation)x).UserColumn > formLocation.UserColumn)
                    .OrderBy(x => ((RackLocation)x).UserColumn)
                    .FirstOrDefault();

                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 已无法再前进列位置，请检查 {1} 是否已运行至前进极限", formLocation, craneDevice));
                }

                _logger.Trace1(string.Format("本次前进起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("前进命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ;
            }
        }

        public void Back(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的后退命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);

                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送后退命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                checkStatus(craneDevice);

                int level = craneDevice.LastStatus.Level;
                int column = craneDevice.LastStatus.Column;
                var formLocation = (RackLocation)craneDevice.Locations.FirstOrDefault(x => ((RackLocation)x).Column == column && ((RackLocation)x).Level == level);


                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = (RackLocation)craneDevice.Locations.Where(x => ((RackLocation)x).UserLevel == formLocation.UserLevel && ((RackLocation)x).UserColumn < formLocation.UserColumn)
                    .OrderByDescending(x => ((RackLocation)x).UserColumn)
                    .FirstOrDefault();

                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 已无法再后退列位置，请检查 {1} 是否已运行至后退极限", formLocation, craneDevice));
                }

                _logger.Trace1(string.Format("本次后退起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("后退命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ;
            }
        }


        CraneDevice getCraneDevice(string craneName)
        {
            var craneDevice = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
               .Where(x => x.Device is Wcs.DefaultImpls.Crane.CraneDevice && x.Name.Equals(craneName, StringComparison.CurrentCultureIgnoreCase))
               .Select(x => x.Device as Wcs.DefaultImpls.Crane.CraneDevice)
               .Single();

            return craneDevice;
        }

        void checkStatus(CraneDevice craneDevice)
        {
            if (craneDevice.LastStatus == null)
            {
                throw new InvalidOperationException(string.Format("{0} 状态未同步。", craneDevice));
            }

            if (craneDevice.LastStatus.State == CraneStatus.ManualMode)
            {
                throw new InvalidOperationException(string.Format("{0} 堆垛机正处于手动模式, 不能下发任务。", craneDevice));
            }

            if (craneDevice.LastStatus.AtStation == false)
            {
                throw new InvalidOperationException(string.Format("{0} 堆垛机不在站点位置, 不能下发任务。", craneDevice));
            }

            if (craneDevice.LastStatus.ForkHorizontalPosition != ForkHorizontalPosition.Center)
            {
                throw new InvalidOperationException(string.Format("{0} 货叉不在中位, 不能下发任务。", craneDevice));
            }

            if (craneDevice.LastStatus.Event != CraneEvent.Initialized
                && craneDevice.LastStatus.Event != CraneEvent.Finished
                )
            {
                throw new InvalidOperationException(string.Format("{0} 堆垛机任务事件未完成, 不能下发任务。", craneDevice));
            }

            if (craneDevice.LastStatus.State != CraneStatus.Initialized
                && craneDevice.LastStatus.State != CraneStatus.无货待命
                && craneDevice.LastStatus.State != CraneStatus.有货待命
                )
            {
                throw new InvalidOperationException(string.Format("{0} 堆垛机处于非待命状态, 不能下发任务。", craneDevice));
            }

            if (craneDevice.WithRemoteController)
            {
                if (!craneDevice.WithRemoteControllerOK)
                    throw new InvalidOperationException(string.Format(" {0} 远程控制盒处于 {1} 状态, 不能下发任务。", craneDevice, String.Join(",", craneDevice.WithRemoteControllerInfo.Select(x => x.Key + ":" + x.Value))));
            }
        }


        public void ResetWarn(string craneName, string userName, string ipAddress)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的报警复位命令", ipAddress, userName, craneName), this);
            try
            {
                var craneDevice = getCraneDevice(craneName);
                if (craneDevice.LastStatus == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 状态未同步，当前无法发送报警复位命令。", craneDevice));
                }

                if (craneDevice.Locker.IsEmpty)
                {
                    throw new InvalidOperationException(string.Format("未获取锁。"));
                }

                if (craneDevice.LastStatus.State == CraneStatus.ManualMode)
                {
                    throw new InvalidOperationException(string.Format("{0} 堆垛机正处于手动模式, 当前无法发送报警复位命令。", craneDevice));
                }

                if (craneDevice.LastStatus.ForkHorizontalPosition != ForkHorizontalPosition.Center)
                {
                    throw new InvalidOperationException(string.Format("{0} 货叉不在中位, 当前无法发送报警复位命令。", craneDevice));
                }

                craneDevice.ResetWarn(new Wcs.Framework.LockerInfo(userName, ipAddress));

                _logger.Trace1("报警复位命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

    }
}
