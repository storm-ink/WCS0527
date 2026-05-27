using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace Wcs.App.Plugins.Reports
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args == null || args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                Application.Run(new frmMain());
            }
            else
            {
                switch (args[0].Trim())
                {
                    case "堆垛机报警统计":
                        Application.Run(new frmCraneWarningReport());
                        break;
                    case "主任务执行时间统计":
                        Application.Run(new frmTaskReport());
                        break;
                    case "设备任务执行时间统计":
                        Application.Run(new frmEquipmentActionsReport());
                        break;
                    case "输送线报警统计":
                        Application.Run(new frmConveyorWarningReport());
                        break;
                    default:
                        MessageBox.Show("未处理的报表类型 " + args[0].Trim(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
        }
    }
}
