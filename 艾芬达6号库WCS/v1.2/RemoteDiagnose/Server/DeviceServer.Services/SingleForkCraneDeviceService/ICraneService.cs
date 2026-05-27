using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DeviceServer.Services.SingleForkCraneDeviceService
{
    [ServiceContract]
    public interface ICraneService:Matedata.IMatedataService
    {
        /// <summary>
        /// 加载的有堆垛机信息，该信息中带有描述最大、小列层的数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CraneInfo> LoadCraneInfos();

        /// <summary>
        /// 读取所有堆垛机当前状态
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<String, LA> ReadStatus();

        /// <summary>
        /// 回原点
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <returns></returns>
        [OperationContract]
        void BackToTheOrigin(string craneName, string userName, string ipAddress);

        /// <summary>
        /// 急停
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <returns></returns>
        [OperationContract]
        void EmergencyStop(string craneName);

        /// <summary>
        /// 取消急停
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <returns></returns>
        [OperationContract]
        void CancelEmergencyStop(string craneName, string userName, string ipAddress);

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="userColumn">目的列位置(用户列)</param>
        /// <param name="userLevel">目的层位置(用户层)</param>
        /// <remarks>执行移动到指定位置的命令</remarks>
        [OperationContract]
        void Move(string craneName, string userName, string ipAddress, int userColumn, int userLevel);

        /// <summary>
        /// 取货
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="forkDirection">取货时货叉的运行方向</param>
        [OperationContract]
        void Pick(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection);

        /// <summary>
        /// 放货
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="forkDirection">放货时货叉的运行方向</param>
        [OperationContract]
        void Putdown(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection);

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="userName">执行锁定时使用的用户名（登记）</param>
        /// <param name="ipAddress">执行锁定时使用的 ip 地址</param>
        [OperationContract]
        void Lock(string craneName, string userName, string ipAddress);

        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="userName">执行锁定时使用的用户名（登记）</param>
        /// <param name="ipAddress">执行锁定时使用的 ip 地址</param>
        [OperationContract]
        void Unlock(string craneName, string userName, string ipAddress);

        /// <summary>
        /// 提升
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="userName">执行锁定时使用的用户名（登记）</param>
        /// <param name="ipAddress">执行锁定时使用的 ip 地址</param>
        [OperationContract]
        void Up(string craneName, string userName, string ipAddress);

        /// <summary>
        /// 下降
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="userName">执行锁定时使用的用户名（登记）</param>
        /// <param name="ipAddress">执行锁定时使用的 ip 地址</param>
        [OperationContract]
        void Down(string craneName, string userName, string ipAddress);

        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="userName">执行锁定时使用的用户名（登记）</param>
        /// <param name="ipAddress">执行锁定时使用的 ip 地址</param>
        [OperationContract]
        void Forward(string craneName, string userName, string ipAddress);

        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="craneName">堆垛机名称</param>
        /// <param name="userName">执行锁定时使用的用户名（登记）</param>
        /// <param name="ipAddress">执行锁定时使用的 ip 地址</param>
        [OperationContract]
        void Back(string craneName, string userName, string ipAddress);
    }
}
