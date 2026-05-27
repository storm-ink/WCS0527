using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wcs.App.Plugins.RackLocation
{
    public class RacklocationBuilder
    {
        StringBuilder sb = new StringBuilder();
        List<LocationItem> _items = new List<LocationItem>();
        public RacklocationBuilder(LocationInfo startColumn, LocationInfo endColumn, LocationInfo startLevel, LocationInfo endLevel, int line, string forkdirection, bool desc, int height, string linePre = "")
        {
            if (startColumn.userValue < endColumn.userValue)
            {
                for (int col = startColumn.userValue; col <= endColumn.userValue; col++)
                {
                    for (int lev = startLevel.userValue; lev <= endLevel.userValue; lev++)
                    {
                        int deviceColumn = startColumn.deviceValue + (desc ? -col + 1 : col);
                        int deviceLevel = startLevel.deviceValue + (lev - startLevel.userValue);
                        string userCode = string.Format("{0:00}-{1:000}-{2:000}", line, col, lev);
                        if (!string.IsNullOrWhiteSpace(linePre))
                            userCode = linePre + userCode;

                        string loc = string.Format(@"<location userCode=""{0}"" column=""{1}"" level=""{2}""  userLine=""{3:00}"" userColumn=""{4}"" userLevel=""{5}"" forkDirection=""{6}"" Height=""{7}""/>", userCode, deviceColumn, deviceLevel, line, col, lev, forkdirection, height);

                        sb.AppendLine(loc);

                        _items.Add(new LocationItem
                        {
                            Column = deviceColumn,
                            Level = deviceLevel,
                            UserColumn = col,
                            UserLevel = lev,
                            UserCode = userCode,
                            ElementOuterXml = loc
                        });
                    }
                }
            }
            else
            {
                for (int col = startColumn.userValue; col >= endColumn.userValue; col--)
                {
                    for (int lev = startLevel.userValue; lev <= endLevel.userValue; lev++)
                    {
                        int deviceColumn = Math.Abs(startColumn.deviceValue + (col - startColumn.userValue));
                        int deviceLevel = Math.Abs(startLevel.deviceValue + (lev - startLevel.userValue));
                        string userCode = string.Format("{0:00}-{1:000}-{2:000}", line, col, lev);
                        string loc = string.Format(@"<location userCode=""{0}"" column=""{1}"" level=""{2}""  userLine=""{3:00}"" userColumn=""{4}"" userLevel=""{5}"" forkDirection=""{6}"" Height=""{7}""/>", userCode, deviceColumn, deviceLevel, line, col, lev, forkdirection, height);

                        sb.AppendLine(loc);

                        _items.Add(new LocationItem
                        {
                            Column = deviceColumn,
                            Level = deviceLevel,
                            UserColumn = col,
                            UserLevel = lev,
                            UserCode = userCode,
                            ElementOuterXml = loc
                        });
                    }
                }
            }
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public List<LocationItem> Result
        {
            get
            {
                return _items;
            }
        }
    }

    public class LocationItem
    {
        public String ElementOuterXml { get; set; }
        public Int32 UserLine { get; set; }
        public Int32 UserLevel { get; set; }
        public Int32 UserColumn { get; set; }
        public Int32 Level { get; set; }
        public Int32 Column { get; set; }
        public String UserCode { get; set; }
    }
}
