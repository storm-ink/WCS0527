/*
 * ================================================
 * 创建人：王建军
 * 创建日期：2012/12/30
 * 备注：
 * 
 * 修改人：
 * 修改日期：
 * 备注：
 * ================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
using Spiral.Utils;
using System.Runtime.Serialization;

namespace Client
{
    /// <summary>
    /// 表示用户的查询条件。
    /// </summary>
    [DataContract]
    public class UserSpec : ISpecification
    {
        /// <summary>
        /// 要匹配的用户名条件。
        /// </summary>
        [DataMember]
        public String UserNameToMatch { get; set; }

        /// <summary>
        /// 要匹配的用户姓名条件。
        /// </summary>
        [DataMember]
        public String RealNameToMatch { get; set; }

        /// <summary>
        /// 是否内置用户
        /// </summary>
        [DataMember]
        public Boolean? IsBuiltIn { get; set; }

        /// <summary>
        /// 修剪此实例中不规范的值使之适合查询
        /// </summary>
        public void Trim()
        {
            this.UserNameToMatch = StringUtil.WhiteSpaceToNull(this.UserNameToMatch);
            this.RealNameToMatch = StringUtil.WhiteSpaceToNull(this.RealNameToMatch);
        }


        /// <summary>
        /// 获取翻译程序名称。
        /// </summary>
        /// <returns></returns>

        public string GetTranslatorTypeName()
        {
            return "Client.UserSpecTranslator, Client";
        }
    }

}
