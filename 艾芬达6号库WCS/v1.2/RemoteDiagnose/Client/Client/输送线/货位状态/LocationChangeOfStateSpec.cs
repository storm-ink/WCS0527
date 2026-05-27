using Spiral.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class LocationChangeOfStateSpec:Spiral.ISpecification
    {
        public LocationChangeOfStateSpec()
        {
        }

        public String NameToMatch { get; set; }
        public Int32? PosNo { get; set; }
        public LocationStatus? StateA { get; set; }
        public LocationStatus? StateB { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public Boolean? IsRunning { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.Conveyor.LocationChangeOfStateSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = StringUtil.WhiteSpaceToNull(this.NameToMatch);
            this.Array = StringUtil.WhiteSpaceToNull(this.Array);
        }
    }
}
