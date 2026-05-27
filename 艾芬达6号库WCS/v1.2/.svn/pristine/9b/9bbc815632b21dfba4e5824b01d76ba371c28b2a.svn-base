using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class AlarmWarningSpec:Spiral.ISpecification
    {
        public AlarmWarningSpec()
        {
        }
        public virtual String NameToMatch { get; set; }
        [DisplayName("货位号")]
        public virtual Int32? PosNo { get; set; }
        public virtual String WarningNameToMath { get; set; }

        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public Boolean? IsWarn { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.Conveyor.AlarmWarningSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = Spiral.Utils.StringUtil.WhiteSpaceToNull(this.NameToMatch);
            this.WarningNameToMath = Spiral.Utils.StringUtil.WhiteSpaceToNull(this.WarningNameToMath);
        }
    }
}
