using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoForksRackLocations.asdf
{
    public class 双货叉货位生成器:IRackLocationBuilder
    {
        public string Create(CreateRackLocationSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in settings.UserLocationRelationships)
            {
                Int32 step = 0;
                List<Int32> 定位列 = new List<int>();
                if ((item.Step1 != null && item.Step1.定位)
                       || (item.Step2 != null && item.Step2.定位)
                       || (item.Step3 != null && item.Step3.定位)
                       )
                {
                    var interval = 0;
                    if (item.Step1 != null && item.Step1.定位)
                    {
                        interval = item.Step1.每组中隔几列;
                    }
                    if (item.Step2 != null && item.Step2.定位)
                    {
                        interval = item.Step2.每组中隔几列;
                    }
                    if (item.Step3 != null && item.Step3.定位)
                    {
                        interval = item.Step3.每组中隔几列;
                    }

                    for (int i = item.FromUserColumn; i <= item.ToUserColumn; i+=interval)
                    {
                        if (i == item.FromUserColumn)
                        {
                            continue;
                        }

                        定位列.Add(i);
                    }
                }

                Int32 lastDWL = 0;
                int column = item.FromColumn;
                int userCodeColumn = item.FromUserColumn;
                int userColumn = item.FromUserColumn;
                for (int i = 0; i <= Math.Abs((item.ToUserColumn-item.FromUserColumn)); i++)
                {
                    Func<int,bool> inArray = (uc) =>
                    {
                        if (item.列递减)
                        {

                            return uc >= item.ToUserColumn;
                        }
                        else
                        {

                            return uc <=item.ToUserColumn;
                        }
                    };
                    for (int userLevel = item.FromUserLevel; userLevel <= item.ToUserLevel; userLevel++)
                    {
                        //int column = item.FromColumn + (userColumn - item.FromUserColumn) * (item.列递减 ? -1 : 1);
                        int level = item.FromLevel + (userLevel - item.FromUserLevel);


                        if (item.Step1!=null && step==item.Step1.每组中隔几列)
                        {
                            addGroup(sb, settings.LanewayName, column, level, userColumn, userLevel, settings.LeftRackNo, settings.RightRackNo,settings.Left2RackNo, settings.Right2RackNo, item.Step1.叉1能用, item.Step1.叉2能用, item.Step1.定位, 定位列, inArray, userCodeColumn);
                        }
                        else if (item.Step2 != null && step == item.Step2.每组中隔几列)
                        {
                            addGroup(sb, settings.LanewayName, column, level, userColumn, userLevel, settings.LeftRackNo, settings.RightRackNo, settings.Left2RackNo, settings.Right2RackNo, item.Step2.叉1能用, item.Step2.叉2能用, item.Step1.定位, 定位列, inArray, userCodeColumn);
                        }
                        else if (item.Step3 != null && step == item.Step3.每组中隔几列)
                        {
                            addGroup(sb, settings.LanewayName, column, level, userColumn, userLevel, settings.LeftRackNo, settings.RightRackNo, settings.Left2RackNo, settings.Right2RackNo, item.Step3.叉1能用, item.Step3.叉2能用, item.Step1.定位, 定位列, inArray, userCodeColumn);
                        }else
                        {
                            addGroup(sb, settings.LanewayName, column, level, userColumn, userLevel, settings.LeftRackNo, settings.RightRackNo, settings.Left2RackNo, settings.Right2RackNo, true, true, false, 定位列, inArray, userCodeColumn);
                        }
                    }


                    if (item.列递减)
                    {
                        userColumn--;
                        column--;
                    }
                    else
                    {
                        column++;
                        userColumn++;
                    }

                    step++;

                    if (step % item.组大小 == 0)
                    {
                        step = 0;
                    }

                    if (定位列.Contains(userCodeColumn))
                    {
                        if (lastDWL == userCodeColumn)
                        {
                            if (item.列递减)
                            {
                                userCodeColumn--;
                            }
                            else
                            {
                                userCodeColumn++;
                            }
                            continue;
                        }
                        else
                        {
                            lastDWL = userCodeColumn;
                            i--;
                        }

                        //if (lastDWL == userColumn)
                        //{
                        //    continue;
                        //}
                        //else
                        //{
                        //    lastDWL = userColumn;
                        //    userColumn--;
                        //}
                    }
                    else
                    {
                        if (item.列递减)
                        {
                            userCodeColumn--;
                        }
                        else
                        {
                            userCodeColumn++;
                        }
                    }
                }
            }

            return sb.ToString();
        }

        void addGroup(StringBuilder sb, String lanewayName, Int32 column, Int32 level, Int32 userColumn, Int32 userLevel, Int32 leftRackNo, Int32 rightRackNo, Int32 left2RackNo, Int32 right2RackNo
            ,Boolean 叉1能用,Boolean 叉2能用,Boolean 定位货位
            , List<Int32> 定位列,Func<int,bool> inArray,Int32 userCodeColumn)
        {
            StringBuilder groupBuilder = new StringBuilder();

            if (定位列.Contains(userCodeColumn) && !叉1能用)
            {

                groupBuilder.AppendFormat("<group userCode=\"{0}{5}列{4}层定位\" column=\"{1}\" level=\"{2}\"  userColumn=\"{3}\" userLevel=\"{4}\" locate=\"true\">\r\n",
                    lanewayName,
                    column,
                    level,
                    userColumn,
                    userLevel,
                    userCodeColumn);
            }
            else
            {
                groupBuilder.AppendFormat("<group userCode=\"{0}{5}列{4}层\" column=\"{1}\" level=\"{2}\"  userColumn=\"{3}\" userLevel=\"{4}\">\r\n",
                    lanewayName,
                    column,
                    level,
                    userColumn,
                    userLevel,
                    userCodeColumn);
            }


            if (叉1能用)
            {
                //叉1
                groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork1\" forkDirection=\"Left\"/>\r\n",
                    leftRackNo,
                userColumn,
                userLevel,
                column,
                level,
                userCodeColumn);
                groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork1\" forkDirection=\"Right\"/>\r\n",
                    rightRackNo,
                userColumn,
                userLevel,
                column,
                level,
                userCodeColumn);

                if (left2RackNo > 0)
                {
                    groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork1\" forkDirection=\"Left2\"/>\r\n",
                       left2RackNo,
                   userColumn,
                   userLevel,
                   column,
                   level,
                   userCodeColumn);
                }

                if (right2RackNo > 0)
                {
                    groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork1\" forkDirection=\"Right2\"/>\r\n",
                        right2RackNo,
                    userColumn,
                    userLevel,
                    column,
                    level,
                    userCodeColumn);
                }
            }
            else
            {
                groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}定位\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork1\" forkDirection=\"Left\" forkAction=\"None\" locate=\"true\"/>\r\n",
                    rightRackNo,
                userColumn,
                userLevel,
                column,
                level,
                userCodeColumn);
            }

            if (叉2能用)
            {
                if (定位列.Contains(userCodeColumn) && 叉1能用==false)
                {
                    if (inArray(userCodeColumn))
                    {
                        //叉2
                        groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Left\"/>\r\n",
                            leftRackNo,
                        userColumn+1,
                        userLevel,
                        column + 1,
                        level,
                        userCodeColumn);
                        groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Right\"/>\r\n",
                            rightRackNo,
                        userColumn + 1,
                        userLevel,
                        column + 1,
                        level,
                        userCodeColumn);

                        if (left2RackNo > 0)
                        {
                            groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Left2\"/>\r\n",
                                left2RackNo,
                            userColumn + 1,
                            userLevel,
                            column + 1,
                            level,
                            userCodeColumn);
                        }

                        if (left2RackNo > 0)
                        {
                            groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Right2\"/>\r\n",
                                right2RackNo,
                            userColumn + 1,
                            userLevel,
                            column + 1,
                            level,
                            userCodeColumn);
                        }
                    }
                }
                else if (定位列.Contains(userCodeColumn + 1))
                {
                    //groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{1:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Left\"/>\r\n",
                    //       leftRackNo,
                    //   userColumn,
                    //   userLevel,
                    //   column + 1,
                    //   level);
                    //groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{1:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Right\"/>\r\n",
                    //    rightRackNo,
                    //userColumn,
                    //userLevel,
                    //column + 1,
                    //level);
                }
                else
                {
                    if (inArray(userCodeColumn + 1))
                    {
                        //叉2
                        groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Left\"/>\r\n",
                            leftRackNo,
                        userColumn + 1,
                        userLevel,
                        column + 1,
                        level,
                        userCodeColumn + 1);
                        groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Right\"/>\r\n",
                            rightRackNo,
                        userColumn + 1,
                        userLevel,
                        column + 1,
                        level,
                        userCodeColumn+1);

                        if (left2RackNo > 0)
                        {
                            groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Left2\"/>\r\n",
                                left2RackNo,
                            userColumn + 1,
                            userLevel,
                            column + 1,
                            level,
                            userCodeColumn + 1);
                        }
                        if (right2RackNo > 0)
                        {
                            groupBuilder.AppendFormat("\t<location userCode=\"{0:d2}-{5:d3}-{2:d3}\" column=\"{3}\" level=\"{4}\" userColumn=\"{1}\" userLevel=\"{2}\" fork=\"Fork2\" forkDirection=\"Right2\"/>\r\n",
                                right2RackNo,
                            userColumn + 1,
                            userLevel,
                            column + 1,
                            level,
                            userCodeColumn + 1);
                        }
                    }
                }
            }
            

            groupBuilder.Append("</group>");

            sb.AppendLine(groupBuilder.ToString());
            groupBuilder = null;
        }

    }
}
