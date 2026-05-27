using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wcs.App.Plugins.ConveyorLocation
{
   public  class UsercodeBuilder
    {
       StringBuilder sb = new StringBuilder();
       public UsercodeBuilder(Location deviceCodeStart, Location deviceCodeEnd)
            {

                for (int decode = deviceCodeStart.deviceCode; decode <= deviceCodeEnd.deviceCode; decode++)
                {


                    string userCode = string.Format("00-001-{0:000}", decode);
                    string loc = string.Format(@"<location deviceCode=""{0}""  userCode=""{1}""/>", decode, userCode);
                    sb.AppendLine(loc);

                }


            }

            public override string ToString()
            {
                return sb.ToString();
            }
    }
}
