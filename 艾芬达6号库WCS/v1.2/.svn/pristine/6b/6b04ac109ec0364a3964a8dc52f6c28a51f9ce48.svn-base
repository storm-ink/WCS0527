using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
using Spiral.Utils;

namespace Client.SingleForkCrane
{

    public class StatusDiagnosisDataSpec:ISpecification
    {
        public StatusDiagnosisDataSpec()
        {
        }
        public String NameToMatch { get; set; }
        public Int32? ErrorCode { get; set; }

        public String TaskIdToMatch { get; set; }
        public String ErrorNameToMatch { get; set; }
        public Int32? Column { get; set; }
        public Int32? Level { get; set; }
        public SingleForkCrane.CraneEvent? Event { get; set; }
        public SingleForkCrane.CraneStatus? Status { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public Boolean? IsWarn { get; set; }
        public Boolean? HasErrorCode { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.SingleForkCrane.StatusDiagnosisDataSpecTranslator, Client";
        }

        public void Trim()
        {
            this.TaskIdToMatch = StringUtil.WhiteSpaceToNull(this.TaskIdToMatch);
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