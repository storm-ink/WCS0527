using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spiral;
using Spiral.Utils;
using Spiral.Base;

namespace Client
{
    public class OperationSpecTranslator : ISpecificationTranslator<Operation, Int32>
    {

        public IQueryable<Operation> ApplyWhere(IQueryable<Operation> q, ISpecification spec)
        {
            OperationSpec s = spec as OperationSpec;
            s.Trim();
            if (s.CodeToMatch != null)
	        {
		        q = q.Where(x =>x.Code.Contains(s.CodeToMatch));
	        }

            return q;

        }
    }
}