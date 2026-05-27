using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
using Spiral.Utils;

namespace Client.Logs
{

    public class WcsLogSpec:ISpecification
    {
        public WcsLogSpec()
        {
        }
        public string Array { get; set; }

        public DateTime? DateMax { get; set; }

        public DateTime? DateMin { get; set; }

        public String[] ExcludedLoggers { get; set; }

        public int? IdMax { get; set; }

        public int? IdMin { get; set; }
        public String[] Level { get; set; }

        public String LoggerToMatch { get; set; }
        public String SenderToMatch { get; set; }

        public int? Seconds { get; set; }

        public int? ThreadId { get; set; }
        public String WordToMatch { get; set; }
        public String UserNameToMatch { get; set; }
        public String TaskCode { get; set; }
        public Int32? EquipmentTaskId { get; set; }
        public string GetTranslatorTypeName()
        {
            return "Client.Logs.WcsLogSpecTranslator, Client";
        }

        public void Trim()
        {
            this.WordToMatch = StringUtil.WhiteSpaceToNull(this.WordToMatch);
            this.LoggerToMatch = StringUtil.WhiteSpaceToNull(this.LoggerToMatch);
            this.SenderToMatch = StringUtil.WhiteSpaceToNull(this.SenderToMatch);
            this.UserNameToMatch = StringUtil.WhiteSpaceToNull(this.UserNameToMatch);
            this.Array = StringUtil.WhiteSpaceToNull(this.Array);
            this.TaskCode = StringUtil.WhiteSpaceToNull(this.TaskCode);

            if (this.DateMin == null)
            {
                this.DateMin = DateTime.Now.AddMinutes(-5);
            }

            if (this.Level == null)
            {
                this.Level = new string[0];
            }

            if (this.ExcludedLoggers == null)
            {
                this.ExcludedLoggers = new string[0];
            }

            if (this.Seconds != null)
            {
                this.DateMax = this.DateMin.Value.AddSeconds(this.Seconds.Value);
            }

        }
    }


}