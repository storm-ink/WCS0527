using Spiral.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class WarningSpec:Spiral.ISpecification
    {
        public WarningSpec()
        {
        }
        public String DeviceType { get; set; }

        public String NameToMatch { get; set; }
        public Int32? ErrorCode { get; set; }

        public String ErrorNameToMatch { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public WarningLevel? Level { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.WarningSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = StringUtil.WhiteSpaceToNull(this.NameToMatch);
            this.ErrorNameToMatch = StringUtil.WhiteSpaceToNull(this.ErrorNameToMatch);
            this.Array = StringUtil.WhiteSpaceToNull(this.Array);
            this.DeviceType = StringUtil.WhiteSpaceToNull(this.DeviceType);

            if (this.DateMin == null)
            {
                this.DateMin = DateTime.Now.AddMinutes(-5);
            }

        }
    }
}
