using Spiral.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Client.RailGuidedVehicle
{
    public class RailGuidedVehicleDiagnosisDataSpec : Spiral.ISpecification
    {
        public RailGuidedVehicleDiagnosisDataSpec()
        {
        }
        public String NameToMatch { get; set; }
        public Int32? ErrorCode { get; set; }

        public String ErrorNameToMatch { get; set; }
        public RailGuidedVehicle.RailGuidedVehicleEvent? Event { get; set; }
        public RailGuidedVehicle.RailGuidedVehicleStatus? Status { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public Boolean? IsWarn { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.RailGuidedVehicle.RailGuidedVehicleDiagnosisDataSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = StringUtil.WhiteSpaceToNull(this.NameToMatch);
            this.ErrorNameToMatch = StringUtil.WhiteSpaceToNull(this.ErrorNameToMatch);
            this.Array = StringUtil.WhiteSpaceToNull(this.Array);

            if (this.DateMin == null)
            {
                this.DateMin = DateTime.Now.AddMinutes(-5);
            }

        }
    }
}
