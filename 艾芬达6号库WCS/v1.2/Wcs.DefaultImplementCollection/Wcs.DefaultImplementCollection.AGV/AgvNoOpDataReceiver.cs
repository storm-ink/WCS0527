using System;
using Wcs.Framework;

namespace Wcs.DefaultImplementCollection.AGV
{
    /// <summary>
    /// AGV 走 HTTP/中间库，不解析 TCP 报文；占位满足配置中的 dataReceiver。
    /// </summary>
    public class AgvNoOpDataReceiver : IDataReceiver
    {
        public AgvNoOpDataReceiver(string name)
        {
            Name = name;
        }

        public string DeviceName { get; set; }
        public Device Device { get; private set; }
        public string Name { get; private set; }

#pragma warning disable 0067
        public event EventHandler<DataReceiverReceivedEventArgs> DataReceived;
#pragma warning restore 0067

        public void AddBytes(byte[] bytes) { }
        public void Clear() { }
        public void Log(string msg) { }

        public TNetTransferObject ConvertToNetTransferObject<TNetTransferObject>(NetPacket netPacket)
            where TNetTransferObject : NetTransferObject
        {
            throw new NotSupportedException("AGV 设备不使用 TCP 解码");
        }

        public NetPacket ConvertToNetPacket<TNetTransferObject>(TNetTransferObject netTransferObject)
            where TNetTransferObject : NetTransferObject
        {
            throw new NotSupportedException("AGV 设备不使用 TCP 解码");
        }
    }
}
