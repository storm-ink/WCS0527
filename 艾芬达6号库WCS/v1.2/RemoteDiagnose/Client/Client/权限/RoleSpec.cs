using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Spiral;
using Spiral.Utils;

namespace Client
{
    /// <summary>
    /// 表示角色的查询条件。
    /// </summary>
    [DataContract]
    public class RoleSpec : ISpecification
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public Int32? Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember]
        public String RoleName { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember]
        public String RoleNameToMatch { get; set; }

        /// <summary>
        /// 是否内置用户
        /// </summary>
        [DataMember]
        public Boolean? IsBuiltIn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public String CommentsToMatch { get; set; }

        /// <summary>
        /// 修剪此实例中不规范的值以适合查询。
        /// </summary>
        public void Trim()
        {
            this.RoleName = StringUtil.WhiteSpaceToNull(this.RoleName);
            this.RoleNameToMatch = StringUtil.WhiteSpaceToNull(this.RoleNameToMatch);
            this.CommentsToMatch = StringUtil.WhiteSpaceToNull(this.CommentsToMatch);
        }
        /// <summary>
        /// 获取翻译程序名称。
        /// </summary>
        /// <returns></returns>
        public string GetTranslatorTypeName()
        {
            return "Client.RoleSpecTranslator, Client";
        }
    }

}
