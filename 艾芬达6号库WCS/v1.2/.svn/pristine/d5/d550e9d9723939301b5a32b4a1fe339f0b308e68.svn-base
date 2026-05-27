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
    /// UserSpec 的翻译程序。
    /// </summary>
    [DataContract]
    public class UserSpecTranslator : ISpecificationTranslator<User, Int32>
    {
        /// <summary>
        /// 在指定的 linq 查询上应用查询条件。
        /// </summary>
        /// <param name="q"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public IQueryable<User> ApplyWhere(IQueryable<User> q, ISpecification spec)
        {
            UserSpec specification = (UserSpec)spec;

            specification.Trim();

            if (specification.UserNameToMatch != null)
            {
                q = q.Where(x => x.UserName.Contains(specification.UserNameToMatch));
            }
            if (specification.RealNameToMatch != null)
            {
                q = q.Where(x => x.RealName.Contains(specification.RealNameToMatch));
            }

            if (specification.IsBuiltIn!=null)
            {
                q = q.Where(x => x.IsBuiltIn == specification.IsBuiltIn);
            }

            if (specification.IsBuiltIn != null)
            {
                q = q.Where(x => x.IsLocked == specification.IsBuiltIn);
            }


            return q;
        }

    }

}
