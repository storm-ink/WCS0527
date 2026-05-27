using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportService
{
    /// <summary>
    /// 服务调用的结果
    /// </summary>
    public class CallResult
    {
        /// <summary>
        /// 指示调用是否成功
        /// </summary>
        public Boolean Success { get; set; }
        /// <summary>
        /// 服务端返回的消息
        /// </summary>
        public String Message { get; set; }
        /// <summary>
        /// 服务端返回的结果（返回结果被转换为json的表现形式，即json字符串）
        /// </summary>
        public String JsonResult { get; set; }
    }
}
