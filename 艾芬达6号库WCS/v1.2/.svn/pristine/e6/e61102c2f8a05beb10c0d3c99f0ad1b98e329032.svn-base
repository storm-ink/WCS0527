using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoForksRackLocations.asdf
{
    public class 单货叉货位生成器:IRackLocationBuilder
    {
        public string Create(CreateRackLocationSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in settings.UserLocationRelationships)
            {
                for (int userColumn = item.FromUserColumn; userColumn <= item.ToUserColumn; userColumn++)
                {
                    for (int userLevel = item.FromUserLevel; userLevel <= item.ToUserLevel; userLevel++)
                    {
                        int column = item.FromColumn + (userColumn - item.FromUserColumn)*(item.列递减?-1:1);
                        int level = item.FromLevel + (userLevel - item.FromUserLevel);

                        addLocation(sb, settings.LanewayName, column, level, userColumn, userLevel, settings.LeftRackNo, settings.RightRackNo, settings.Left2RackNo, settings.Right2RackNo);
                    }
                }
            }

            return sb.ToString();
        }

        void addLocation(StringBuilder sb, String lanewayName, Int32 column, Int32 level, Int32 userColumn, Int32 userLevel, Int32 leftRackNo, Int32 rightRackNo, Int32 left2RackNo, Int32 right2RackNo)
        {
            sb.AppendFormat("<location userCode=\"{0:d2}-{1:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" forkDirection=\"Left\"/>\r\n",
                leftRackNo,
            userColumn,
            userLevel,
            column,
            level);

            sb.AppendFormat("<location userCode=\"{0:d2}-{1:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" forkDirection=\"Right\"/>\r\n",
                rightRackNo,
            userColumn,
            userLevel,
            column,
            level);

            if (left2RackNo > 0)
            {
                sb.AppendFormat("<location userCode=\"{0:d2}-{1:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" forkDirection=\"Left2\"/>\r\n",
                    left2RackNo,
                userColumn,
                userLevel,
                column,
                level);
            }

            if (right2RackNo > 0)
            {
                sb.AppendFormat("<location userCode=\"{0:d2}-{1:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" forkDirection=\"Right2\"/>\r\n",
                    right2RackNo,
                userColumn,
                userLevel,
                column,
                level);
            }
        }

    }
}
