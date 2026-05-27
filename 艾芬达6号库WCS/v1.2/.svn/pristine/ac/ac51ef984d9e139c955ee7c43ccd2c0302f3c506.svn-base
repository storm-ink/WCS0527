using Spiral.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.SingleForkCrane
{
    public class ChangeOfStatesSpec:Spiral.ISpecification
    {
        public ChangeOfStatesSpec()
        {
        }

        public String NameToMatch { get; set; }
        public CraneStatus? StateA { get; set; }
        public CraneStatus? StateB { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public Boolean? IsRunning { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.SingleForkCrane.ChangeOfStateSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = StringUtil.WhiteSpaceToNull(this.NameToMatch);
            this.Array = StringUtil.WhiteSpaceToNull(this.Array);
        }
    }
}
