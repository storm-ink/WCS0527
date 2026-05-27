using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spiral;
using Spiral.Utils;
namespace Client
{
    public class OperationSpec : ISpecification
    {
        public String CodeToMatch { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.OperationSpecTranslator, Client";
        }

        public void Trim()
        {
            this.CodeToMatch = StringUtil.WhiteSpaceToNull(this.CodeToMatch);
        }
    }

}