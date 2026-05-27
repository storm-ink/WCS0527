using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class AppearanceInspectionSpec:Spiral.ISpecification
    {

        public String NameToMatch { get; set; }
        public Int32? No { get; set; }
        public AppearanceInspectionResult? Result { get; set; }
        public Boolean? Left_Over { get; set; }
        public Boolean? Right_Over { get; set; }
        public Boolean? Front_Over { get; set; }
        public Boolean? Back_Over { get; set; }
        public Boolean? Too_High { get; set; }

        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.Conveyor.AppearanceInspectionSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = Spiral.Utils.StringUtil.WhiteSpaceToNull(this.NameToMatch);
        }

        
    }
}
