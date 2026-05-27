using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class Helper
    {
        static Random rnd = new Random();

        public static string GetRandomColor()
        {
            //  为了在白色背景上显示，尽量生成深色
            int int_Red = rnd.Next(256);
            int int_Green = rnd.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            var color = Color.FromArgb(int_Red, int_Green, int_Blue);
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
