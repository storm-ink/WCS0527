using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs;
using Wcs.DefaultImpls.Crane;
using Wcs.Framework;
namespace CraneService.TwoForksCrane
{
    public class TwoForksCraneService:ITwoForksCraneService
    {
        static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public List<CraneInfo> LoadCraneInfos()
        {
            List<CraneInfo> result = new List<CraneInfo>();
            foreach (var craneDevice in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice)
                .Select(x => x.Device as Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice))
            {
                var info = new CraneInfo();
                info.Name = craneDevice.Name;
                var locations = craneDevice.Locations.Where(x => x is Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation);
                if (locations.Count() > 0)
                {
                    info.MinColumn = locations.Min(x => ((RackLocation)x).UserColumn);
                    info.MaxColumn = locations.Max(x => ((RackLocation)x).UserColumn);
                    info.MinLevel = locations.Min(x => ((RackLocation)x).UserLevel);
                    info.MaxLevel = locations.Max(x => ((RackLocation)x).UserLevel);
                }
                result.Add(info);
            }

            return result;
        }

        public Dictionary<string, LA> ReadStatus()
        {
            Dictionary<String, LA> result = new Dictionary<String, LA>();
            foreach (var craneDevice in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice)
                .Select(x => x.Device as Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice))
            {
                var lastStatus=(Wcs.DefaultImpls.TwoForksCrane.RequestStateCommandReplyTelexTransferObject)craneDevice.LastStatus;

                if (lastStatus != null)
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

                    var location = craneDevice.GetCurrentLocation();

                    LA la = new LA
                    {
                        AtStation = lastStatus.AtStation,
                        ErrorCode = lastStatus.ErrorCode,
                        ErrorName = errorName,
                        ErrorDescription = errorDescription,
                        ErrorSolution = errorSolution,
                        Event = lastStatus.Event,
                        Fork1HorizontalPosition = lastStatus.Fork1HorizontalPosition,
                        Fork2HorizontalPosition = lastStatus.Fork2HorizontalPosition,
                        Fork1Status = lastStatus.Fork1Status,
                        Fork2Status = lastStatus.Fork2Status,
                        ForkVerticalPosition = lastStatus.ForkVerticalPosition,
                        UserColumn = location == null ? "?" : location.UserColumn.ToString("000"),
                        UserLevel = location == null ? "?" : location.UserLevel.ToString("000"),
                        UserCode=location==null?"?":location.UserCode,
                        LockerIp = craneDevice.Locker.IPAddress,
                        LockerUser = craneDevice.Locker.UserName,
                        State = lastStatus.State,
                        TaskId = lastStatus.TaskId
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
                throw;
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

                craneDevice.CancelEmergencyStop(new Wcs.Framework.LockerInfo(userName, ipAddress));

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
                var formLocation = Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneHelper.GetGroup(craneDevice, currentColumn, currentLevel);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层这个位置组。", craneDevice, currentColumn, currentLevel));
                }

                var toLocation = Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneHelper.GetGroupByUserColumnAndLevel(craneDevice, userColumn, userLevel, Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1);
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层 这个位置。", craneDevice, userColumn, userLevel));
                }

                _logger.Trace1(string.Format("本次移动的起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                //craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

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
                var formLocation = Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneHelper.GetGroup(craneDevice, currentColumn, currentLevel);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层这个位置组。", craneDevice, currentColumn, currentLevel));
                }

                var toLocation = Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneHelper.GetGroupByUserColumnAndLevel(craneDevice, userColumn, userLevel, Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1,forkDirection);
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层 向 {3} 伸叉这个位置。", craneDevice, userColumn, userLevel,forkDirection));
                }

                _logger.Trace1(string.Format("本次移动的起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                //craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                _logger.Trace1("移动命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void Pick(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection? fork1Direction, Wcs.DefaultImpls.Crane.ForkDirection? fork2Direction)
        {
            if (fork1Direction != null && fork2Direction != null)
            {
                _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的使用叉1向{3}取货，使用叉2向{4}取货的命令", ipAddress, userName, craneName, fork1Direction.Value.GetDescription(), fork2Direction.Value.GetDescription()), this);
            }
            else if (fork1Direction != null && fork2Direction == null)
            {
                _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的使用叉1向{3}取货的命令", ipAddress, userName, craneName, fork1Direction.Value.GetDescription()), this);
            }
            else if (fork1Direction == null && fork2Direction != null)
            {
                _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的使用叉2向{3}取货的命令", ipAddress, userName, craneName, fork2Direction.Value.GetDescription()), this);
            }
            else
            {
                throw new InvalidOperationException("至少需要指定一个货叉动作");
            }

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

                var formLocation = (Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup)craneDevice.GetCurrentLocation();

                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层这个位置。", craneDevice, column, level));
                }

                _logger.Trace1(string.Format("本次取货目标位置为 {0}", formLocation), this);


                craneDevice.Pick(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation,fork1Direction,fork2Direction);

                _logger.Trace1("取货命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        public void Putdown(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection? fork1Direction, Wcs.DefaultImpls.Crane.ForkDirection? fork2Direction)
        {
            if (fork1Direction != null && fork2Direction != null)
            {
                _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的使用叉1向{3}放货，使用叉2向{4}放货的命令", ipAddress, userName, craneName, fork1Direction.Value.GetDescription(), fork2Direction.Value.GetDescription()), this);
            }
            else if (fork1Direction != null && fork2Direction == null)
            {
                _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的使用叉1向{3}放货的命令", ipAddress, userName, craneName, fork1Direction.Value.GetDescription()), this);
            }
            else if (fork1Direction == null && fork2Direction != null)
            {
                _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的使用叉2向{3}放货的命令", ipAddress, userName, craneName, fork2Direction.Value.GetDescription()), this);
            }
            else
            {
                throw new InvalidOperationException("至少需要指定一个货叉动作");
            }

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

                var toLocation = (Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup)craneDevice.GetCurrentLocation();
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}这个位置。", craneDevice, column, level));
                }

                _logger.Trace1(string.Format("本次放货目标位置为 {0}", toLocation), this);


                craneDevice.Putdown(new Wcs.Framework.LockerInfo(userName, ipAddress), toLocation,fork1Direction,fork2Direction);

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
                throw;
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

                var formLocation = GetGroup(craneDevice, column, level);
                
                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = GetToGroup(formLocation, "up");

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
                throw;
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
                var formLocation = GetGroup(craneDevice, column, level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = GetToGroup(formLocation, "down");

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
                throw;
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


                var formLocation = GetGroup(craneDevice, column, level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = GetToGroup(formLocation, "forward");

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
                throw;
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

                var formLocation = GetGroup(craneDevice, column, level);

                _logger.Trace1(string.Format("取 {0} 的当前位置 {1} 为依据", craneDevice, formLocation), this);

                var toLocation = GetToGroup(formLocation, "back");

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
                throw;
            }
        }

        Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice getCraneDevice(string craneName)
        {
            var craneDevice = Wcs.Framework.DeviceConverter.ToDevice<Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice>(craneName);

            return craneDevice;
        }

        void checkStatus(Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice craneDevice)
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

        public void MoveByForkDirectionAndFork(string craneName, string userName, string ipAddress, String userCode,Boolean alignToFork1)
        {
            _logger.Trace1(string.Format("收到了来自 {0} 的 {1} 向 {2} 发送的移动到 {3} 的命令", ipAddress, userName, craneName,userCode), this);
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
                var formLocation = Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneHelper.GetGroup(craneDevice, currentColumn, currentLevel);
                if (formLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}层这个位置组。", craneDevice, currentColumn, currentLevel));
                }

                var toLocation = LocationConverter.UserCodeToLcation(userCode);
                if (toLocation == null)
                {
                    throw new InvalidOperationException(string.Format("{0} 不存在 {1} 这个位置。", craneDevice, userCode));
                }

                if(toLocation.Device!=craneDevice)
                {
                    throw new InvalidOperationException(string.Format("{0} 不属于 {1}。", toLocation, craneDevice));
                }

                if (toLocation is Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                {
                    toLocation = Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneHelper.GetGroup(
                        (Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)toLocation,
                        alignToFork1?Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1:Wcs.DefaultImpls.TwoForksCrane.Forks.Fork2
                        );

                }
                else if (toLocation is Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup)
                {
                }
                else
                {
                    throw new InvalidOperationException(string.Format("{0} 不是 TwoForksRackLocation 或 RackLocationGroup 类型。", toLocation, craneDevice)); 
                }

                _logger.Trace1(string.Format("本次移动的起始位置为 {0}，目标位置为 {1}", formLocation, toLocation), this);

                //craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, toLocation);

                craneDevice.Move(new Wcs.Framework.LockerInfo(userName, ipAddress), formLocation, (Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup)toLocation);

                _logger.Trace1("移动命令发送成功", this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                throw;
            }
        }

        Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup GetGroup(Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice craneDevice, Int32 column, Int32 level)
        {
            var loc = craneDevice
                   .Locations
                   .Where(x => !(x is Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup))
                   .Where(x => x is Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                   .Select(x => x as Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                   .FirstOrDefault(x =>
                       x.Level == level
                       && x.Column == column
                       && x.Groups.Any(g => g.GetFork(x) == Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1)
                       );

            if (loc == null)
            {
                throw new InvalidOperationException(string.Format("{0} 不存在 {1}列{2}这个位置。", craneDevice, column, level));
            }

            var r = loc.Groups.First(x => x.GetFork(loc) == Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1);

            return r;
        }

        Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup GetToGroup(Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup fromGroup,String cmd)
        {
            var craneDevice = (Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice)fromGroup.Device;
            Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation loc;
            switch (cmd)
            {
                case "up":
                    {
                        loc = craneDevice
                          .Locations
                          .Where(x => !(x is Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup))
                          .Where(x => x is Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Select(x => x as Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Where(x =>
                              x.UserColumn == fromGroup.UserColumn
                              && x.UserLevel > fromGroup.UserLevel
                              && x.Groups.Any(g => g.GetFork(x) == Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1)
                              )
                          .OrderBy(x => x.UserLevel)
                          .FirstOrDefault();
                    }
                    break;
                case "down":
                    {
                        loc = craneDevice
                          .Locations
                          .Where(x => !(x is Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup))
                          .Where(x => x is Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Select(x => x as Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Where(x =>
                              x.UserColumn == fromGroup.UserColumn
                              && x.UserLevel < fromGroup.UserLevel
                              && x.Groups.Any(g => g.GetFork(x) == Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1)
                              )
                          .OrderByDescending(x => x.UserLevel)
                          .FirstOrDefault();
                    }
                    break;
                case "forward":
                    {
                        loc = craneDevice
                          .Locations
                          .Where(x => !(x is Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup))
                          .Where(x => x is Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Select(x => x as Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Where(x =>
                              x.UserLevel == fromGroup.UserLevel
                              && x.UserColumn > fromGroup.UserColumn
                              && x.Groups.Any(g => g.GetFork(x) == Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1)
                              )
                          .OrderBy(x => x.UserColumn)
                          .FirstOrDefault();
                    }
                    break;
                case "back":
                    {
                        loc = craneDevice
                          .Locations
                          .Where(x => !(x is Wcs.DefaultImpls.TwoForksCrane.RackLocationGroup))
                          .Where(x => x is Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Select(x => x as Wcs.DefaultImpls.TwoForksCrane.TwoForksRackLocation)
                          .Where(x =>
                              x.UserLevel == fromGroup.UserLevel
                              && x.UserColumn < fromGroup.UserColumn
                              && x.Groups.Any(g => g.GetFork(x) == Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1)
                              )
                          .OrderByDescending(x => x.UserColumn)
                          .FirstOrDefault();
                    }
                    break;
                default:
                    throw new NotSupportedException(string.Format("不支持的查询类型{0}",cmd));
            }

            if (loc == null)
            {
                return null;
            }

            var r = loc.Groups.First(x => x.GetFork(loc) == Wcs.DefaultImpls.TwoForksCrane.Forks.Fork1);

            return r;
        }
    }
}
