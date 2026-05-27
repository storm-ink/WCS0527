using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S7Net40
{
    public abstract class BaseEquip
    {
        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public abstract bool Open();
        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="block">数据块</param>
        /// <param name="start">起始地址</param>
        /// <param name="len">长度</param>
        /// <param name="buff">读取返回信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public abstract bool Read(string block, int start, int len, out object[] buff);
        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="block">数据块</param>
        /// <param name="start">起始地址</param>
        /// <param name="buff">要写入的数据</param>
        /// <returns>成功返回true，失败返回false</returns>
        public abstract bool Write(int block, int start, object[] buff);
        /// <summary>
        /// 关闭设备
        /// </summary>
        public abstract void Close();
    }
}
