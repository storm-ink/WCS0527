using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs.DefaultImpls.Crane;
using Wcs;
using Matedata;

namespace DeviceServer.Services.SingleForkCraneDeviceService
{
    public class CraneService : DeviceService,ICraneService
    {
        static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

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
                var formLocation = craneDevice.Locations.FirstOrDefault(x => x.Column == column && x.Level == level);


                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = craneDevice.Locations.Where(x => x.UserLevel == formLocation.UserLevel && x.UserColumn < formLocation.UserColumn)
                    .OrderByDescending(x => x.UserColumn)
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
                throw ex;
            }
        }

        public void BackToTheOrigin(string craneName, string userName, string ipAddress)
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

                craneDevice.BackToTheOrigin(new Wcs.Framework.LockerInfo(userName, ipAddress));

                _logger.Trace1("回原点命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ex;
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

                craneDevice.CancelEmergencyStop(new Wcs.Framework.LockerInfo(userName, ipAddress));

                _logger.Trace1("取消急停命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ex;
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
                var formLocation = craneDevice.Locations.FirstOrDefault(x => x.Column == column && x.Level == level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = craneDevice.Locations.Where(x => x.UserColumn == formLocation.UserColumn && x.UserLevel < formLocation.UserLevel)
                    .OrderByDescending(x => x.UserLevel)
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
                throw ex;
            }
        }

        public void EmergencyStop(string craneName)
        {
            _logger.Trace1(string.Format("收到了一个 {0} 急停命令", craneName), this);
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
                throw ex;
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
                var formLocation = craneDevice.Locations.FirstOrDefault(x => x.Column == column && x.Level == level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = craneDevice.Locations.Where(x => x.UserLevel == formLocation.UserLevel && x.UserColumn > formLocation.UserColumn)
                    .OrderBy(x => x.UserColumn)
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
                throw ex;
            }
        }

        public List<CraneInfo> LoadCraneInfos()
        {
            List<CraneInfo> result = new List<CraneInfo>();
            foreach (var craneDevice in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is Wcs.DefaultImpls.Crane.CraneDevice)
                .Select(x => x.Device as Wcs.DefaultImpls.Crane.CraneDevice))
            {
                var info = new CraneInfo();
                info.Name = craneDevice.Name;
                if (craneDevice.Locations.Length > 0)
                {
                    info.MinColumn = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Min(x => x.UserColumn);
                    info.MaxColumn = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Max(x => x.UserColumn);
                    info.MinLevel = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Min(x => x.UserLevel);
                    info.MaxLevel = craneDevice.Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard)).Max(x => x.UserLevel);
                }
                result.Add(info);
            }

            return result;
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
                throw ex;
            }
        }

        public void Move(string craneName, string userName, string ipAddress, int userColumn, int userLevel)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的移动到 {3}列{4}层 的命令", ipAddress, userName, craneName, userColumn, userLevel), this);
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
                var formLocation = craneDevice.Locations.FirstOrDefault(x => x.Column == currentColumn && x.Level == currentLevel);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层货这个位置。", craneDevice, currentColumn, currentLevel, userLevel));
                }

                var toLocation = craneDevice.Locations.FirstOrDefault(x => x.UserColumn == userColumn && x.UserLevel == userLevel);
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层 这个位置。", craneDevice, userColumn, userLevel));
                }

                _logger.Trace1(string.Format("本次移动的起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("移动命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ex;
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

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1}列{2}层为目的地", craneDevice, column, level), this);

                var formLocation = craneDevice.Locations.SingleOrDefault(x => x.Column == column && x.Level == level && x.ForkDirection == forkDirection);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层货位叉{3}这个位置。", craneDevice, column, level, forkDirection.GetDescription()));
                }

                _logger.Trace1(string.Format("本次取货目标位置为 {0}", formLocation), this);


                craneDevice.Pick(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation);

                _logger.Trace1("取货命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ex;
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

                var toLocation = craneDevice.Locations.SingleOrDefault(x => x.Column == column && x.Level == level && x.ForkDirection == forkDirection);
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层货位叉{3}这个位置。", craneDevice, column, level, forkDirection.GetDescription()));
                }

                _logger.Trace1(string.Format("本次放货目标位置为 {0}", toLocation), this);


                craneDevice.Putdown(new Wcs.Framework.LockerInfo(userName, ipAddress), toLocation);

                _logger.Trace1("放货命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ex;
            }
        }

        public Dictionary<String, LA> ReadStatus()
        {
            Dictionary<String, LA> result = new Dictionary<String, LA>();
            foreach (var craneDevice in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is Wcs.DefaultImpls.Crane.CraneDevice)
                .Select(x => x.Device as Wcs.DefaultImpls.Crane.CraneDevice))
            {

                if (craneDevice.LastStatus != null)
                {
                    var alarm = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection
                    .ParticularDeviceCollection.Where(x => x is CraneCollection)
                    .Select(x => x as CraneCollection)
                    .SelectMany(x => x.AlarmCollection.AlarmElements)
                    .Where(x => x.Code == craneDevice.LastStatus.ErrorCode.ToString("0000"))
                    .FirstOrDefault();

                    var errorDescription = "";
                    var errorSolution = "";
                    var errorName = "";
                    if (alarm != null)
                    {
                        errorName = alarm.Name;
                        errorDescription = alarm.Description;
                        errorSolution = alarm.Solution;
                    }

                    var location = craneDevice.Locations.FirstOrDefault(x => x.Level == craneDevice.LastStatus.Level && x.Column == craneDevice.LastStatus.Column);

                    LA la = new LA
                    {
                        AtStation = craneDevice.LastStatus.AtStation,
                        ErrorCode = craneDevice.LastStatus.ErrorCode,
                        ErrorName = errorName,
                        ErrorDescription = errorDescription,
                        ErrorSolution = errorSolution,
                        Event = craneDevice.LastStatus.Event,
                        ForkHorizontalPosition = craneDevice.LastStatus.ForkHorizontalPosition,
                        ForkVerticalPosition = craneDevice.LastStatus.ForkVerticalPosition,
                        UserColumn = location == null ? "?" : location.UserColumn.ToString("000"),
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
                    result.Add(craneDevice.Name, null);
                }
            }

            return result;
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
                throw ex;
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
                var formLocation = craneDevice.Locations.FirstOrDefault(x => x.Column == column && x.Level == level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);


                var toLocation = craneDevice.Locations.Where(x => x.UserColumn == formLocation.UserColumn && x.UserLevel > formLocation.UserLevel)
                    .OrderBy(x => x.UserLevel)
                    .FirstOrDefault();
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 已无法再提升层位置，请检查 {1} 是否已运行至提升极限", formLocation, craneDevice));
                }

                _logger.Trace1(string.Format("本次提升起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("提升命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw ex;
            }
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
        }

        CraneDevice getCraneDevice(string craneName)
        {
            var craneDevice = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
               .Where(x => x.Device is Wcs.DefaultImpls.Crane.CraneDevice && x.Name.Equals(craneName, StringComparison.CurrentCultureIgnoreCase))
               .Select(x => x.Device as Wcs.DefaultImpls.Crane.CraneDevice)
               .Single();

            return craneDevice;
        }

        public string GetDiagnoseMatedatas(int minDataId, int batchSize, string appInfo)
        {
            throw new NotImplementedException();
        }
    }
}
