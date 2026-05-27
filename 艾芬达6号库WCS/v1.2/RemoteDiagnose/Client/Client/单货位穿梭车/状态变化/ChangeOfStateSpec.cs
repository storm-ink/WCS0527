using Spiral.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.RailGuidedVehicle
{
    public class ChangeOfStatesSpec:Spiral.ISpecification
    {
        public ChangeOfStatesSpec()
        {
        }

        public String NameToMatch { get; set; }
        public RailGuidedVehicleStatus? StateA { get; set; }
        public RailGuidedVehicleStatus? StateB { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public Boolean? IsRunning { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.RailGuidedVehicle.ChangeOfStateSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = StringUtil.WhiteSpaceToNull(this.NameToMatch);
            this.Array = StringUtil.WhiteSpaceToNull(this.Array);
        }
    }
}
