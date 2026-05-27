using Spiral.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class LocationTaskSpec:Spiral.ISpecification
    {
        public Int32? TaskNo { get; set; }
        public Int32? TUID { get; set; }
        public String NameToMatch { get; set; }
        public Int32? PosNo { get; set; }
        public LocationStatus? State { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.Conveyor.LocationTaskSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = StringUtil.WhiteSpaceToNull(this.NameToMatch);
            this.Array = StringUtil.WhiteSpaceToNull(this.Array);            
        }
    }
}
