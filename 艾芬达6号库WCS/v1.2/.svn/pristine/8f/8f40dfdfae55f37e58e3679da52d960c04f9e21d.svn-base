using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7Net40
{
    /// <summary>
    /// 西门子S7系列以太网通讯类
    /// </summary>
    public class Equip : BaseEquip
    {
        #region 字段定义

        private string _ip = "192.168.0.250";       //PLC IP Address
        private int _rack = 0;                      //机架号
        private int _slot = 0;                      //槽号,1200~1500设置为0,300系列设置为2

        private bool _isOpen = false;   //是否打开连接
        private int _errorCount = 0;    //记录读取失败次数
        private int _openErrorCount = 0;    //打开PLC失败的次数
        private S7Client _client = new S7Client();

        private int[] _area =
            {
                 S7Consts.S7AreaPE,
                 S7Consts.S7AreaPA,
                 S7Consts.S7AreaMK,
                 S7Consts.S7AreaDB,
                 S7Consts.S7AreaCT,
                 S7Consts.S7AreaTM
            };
        private int[] _wordLen =
            {
                 S7Consts.S7WLBit,
                 S7Consts.S7WLByte,
                 S7Consts.S7WLChar,
                 S7Consts.S7WLWord,
                 S7Consts.S7WLInt,
                 S7Consts.S7WLDWord,
                 S7Consts.S7WLDInt,
                 S7Consts.S7WLReal,
                 S7Consts.S7WLCounter,
                 S7Consts.S7WLTimer
            };

        #endregion

        public Equip(string ip, int rack, int slot)
        {
            this._ip = ip;
            this._rack = rack;
            this._slot = slot;
        }

        #region 属性定义

        /// <summary>
        /// IP
        /// </summary>
        private string IP
        {
            get
            {

                return this._ip;
            }
        }

        /// <summary>
        /// 机架号
        /// </summary>
        private int Rack
        {
            get
            {
                return this._rack;
            }
        }

        /// <summary>
        /// 插槽号
        /// </summary>
        private int Slot
        {
            get
            {
                return this._slot;
            }
        }

        public bool State { get; private set; }

        #endregion

        private int Swap(int a)
        {
            return (a >> 8 & 0xFF) + (a << 8 & 0xFF00);
        }

        #region 实现BaseEquip成员

        /// <summary>
        /// 建立与PLC的连接
        /// </summary>
        /// <returns></returns>
        public override bool Open()
        {
            lock (this)
            {
                if (this._isOpen == true)
                {
                    return true;
                }
                this.State = false;
                /////////////////////////////////////////////////////////////////////////////////
                int result = this._client.ConnectTo(this.IP, this.Rack, this.Slot);
                ///////////////////////////////////////////////////////////////////
                if (result != 0)
                {
                    Console.WriteLine("PLC连接失败：" + this.GetErrInfo(result));
                    this.State = false;
                    if (this._openErrorCount > 1)
                    {
                        System.Threading.Thread.Sleep(10000);   //2次连接不上暂停10秒
                        this._openErrorCount = 0;
                    }
                    this._openErrorCount++;
                    return this.State;
                }
                else
                {
                    Console.WriteLine("PLC连接成功!");
                    this.State = true;
                    this._isOpen = true;
                }
                return this.State;
            }
        }
        /// <summary>
        /// PLC数据读取方法
        /// </summary>
        /// <param name="block">要读取的块号</param>
        /// <param name="start">要读取的起始字</param>
        /// <param name="len">要读取的长度</param>
        /// <param name="buff"></param>
        /// <returns></returns>
        public override bool Read(string block, int start, int len, out object[] buff)
        {
            lock (this)
            {
                int sizeRead = 0;
                int iblock = Convert.ToInt32(block);
                buff = new object[len];
                for (int i = 0; i < len; i++)
                {
                    buff[i] = 0;
                }
                if (!this.Open())
                {
                    return false;
                }
                byte[] _buff = new byte[len * 2];
                int result = this._client.ReadArea(S7Consts.S7AreaDB, iblock, start * 2, len, S7Consts.S7WLInt, _buff, ref sizeRead);
                if (result != 0)
                {
                    Console.WriteLine(this.GetErrInfo(result) + "\t" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                    if (this._errorCount > 5)
                    {
                        this.Close();       //5次读取失败，关闭PLC，再次读取自动连接
                    }
                    this._errorCount++;
                }
                if (sizeRead != len * 2)
                {
                    Console.WriteLine(String.Format("block={0}, start={1}, len={2} 读取的字节数与设置的字数不成2倍关系!", block, start, len));
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        byte[] curr = new byte[2];
                        //高低位对调
                        //curr[0] = _buff[i * 2];
                        //curr[1] = _buff[i * 2 + 1];
                        //buff[i] = this.Swap(BitConverter.ToInt16(curr, 0));
                        curr[1] = _buff[i * 2];
                        curr[0] = _buff[i * 2 + 1];
                        buff[i] = BitConverter.ToInt16(curr, 0);
                    }
                    //ICSharpCode.Core.LoggingService<Equip>.Warn("内部处理完毕" + "\t" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                }
                if (result != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// PLC数据写入方法
        /// </summary>
        /// <param name="block">要写入的块号</param>
        /// <param name="start">起始字</param>
        /// <param name="buff">要写入PLC的数据</param>
        /// <returns>写入成功返回true，失败返回false</returns>
        public override bool Write(int block, int start, object[] buff)
        {
            lock (this)
            {
                if (!this.Open())
                {
                    return false;
                }
                int iblock = Convert.ToInt32(block);
                int sizeWrite = 0;
                byte[] _buff = new byte[buff.Length * 2];
                for (int i = 0; i < buff.Length; i++)
                {
                    System.Int16 value = 0;
                    System.Int16.TryParse(buff[i].ToString(), out value);
                    byte[] curr = BitConverter.GetBytes(value);
                    //高低位对调
                    _buff[i * 2] = curr[1];
                    _buff[i * 2 + 1] = curr[0];
                }
                int result = this._client.WriteArea(S7Consts.S7AreaDB, iblock, start * 2, buff.Length, S7Consts.S7WLInt, _buff, ref sizeWrite);
                if (result != 0)
                {
                    Console.WriteLine(this.GetErrInfo(result) + "\t" + String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                }

                if (sizeWrite != buff.Length * 2)
                {
                    Console.WriteLine(String.Format("block={0}, start={1}, len={2} 写入的字节数与设置的字数不成2倍关系!", block, start, buff.Length));
                }
                if (result != 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 断开与PLC的连接
        /// </summary>
        public override void Close()
        {
            lock (this)
            {
                try
                {
                    int result = this._client.Disconnect();
                    if (result != 0)
                    {
                        Console.WriteLine("PLC关闭失败：" + this.GetErrInfo(result));
                    }
                    else
                    {
                        Console.WriteLine("PLC已断开连接!");
                        this._errorCount = 0;
                        this.State = false;
                        this._isOpen = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PLC关闭失败：" + ex.Message);
                }
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>根据错误代码返回错误信息
        /// 例如int errCode=ActiveConn(1);  sring errInfo = GetErrInfo(err);
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns>错误信息</returns>
        public string GetErrInfo(int errCode)
        {
            if (errCode == 0)
                return " Success(" + this._client.ExecutionTime.ToString() + " ms)";
            else
                return this._client.ErrorText(errCode);
        }

        #endregion
    }
}
