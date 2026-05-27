using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Spiral;
using Spiral.Utils;
using Spiral.Base;

namespace Client
{
    /// <summary>
    /// RoleSpec 的翻译程序。
    /// </summary>
    public class RoleSpecTranslator : ISpecificationTranslator<Role, Int32>
    {
        /// <summary>
        /// 在指定的 linq 查询上应用查询条件。
        /// </summary>
        /// <param name="q"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public IQueryable<Role> ApplyWhere(IQueryable<Role> q, ISpecification spec)
        {
            RoleSpec specification = (RoleSpec)spec;
            specification.Trim();

            if (specification.Id != null)
            {
                q = q.Where(x => x.Id == specification.Id);
            }
            if (specification.RoleName != null)
            {
                q = q.Where(x => x.RoleName.Contains(specification.RoleName));
            }

            if (specification.RoleNameToMatch != null)
            {
                q = q.Where(x => x.RoleName.Contains(specification.RoleNameToMatch));
            }

            if (specification.CommentsToMatch != null)
            {
                q = q.Where(x => x.RoleName.Contains(specification.CommentsToMatch));
            }

            return q;
        }

    }

}
