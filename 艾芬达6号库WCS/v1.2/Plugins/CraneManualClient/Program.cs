using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace Wcs.App.Plugins.CraneManualClient
{
    static class Program
    {
        #region Win32 API 函数

        private const int SW_SHOWNORMAL = 1;
        private const int SW_RESTORE = 9;

        //该函数设置由不同线程产生的窗口的显示状态;
        //如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        //该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        //如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // IsIconic ------ 分别判断窗口是否已最小化
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        
        /// <summary>
        /// 启动控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        #endregion

        //同步基元变量
        static System.Threading.Mutex instance;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            #region 单实例检测
            Boolean createdNew;
            instance = new System.Threading.Mutex(true, "CHTS.exe", out createdNew);
            if (!createdNew)
            {
                //MessageBox.Show("已经有一个 Wcs 实例在运行了",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);

                try
                {
                    var process = System.Diagnostics.Process.GetProcessesByName("CHTS").SingleOrDefault();
                    if (process != null)
                    {
                        SetForegroundWindow(process.MainWindowHandle);
                        if (IsIconic(process.MainWindowHandle))
                        {
                            ShowWindowAsync(process.MainWindowHandle, SW_RESTORE);
                        }
                        else
                        {
                            ShowWindowAsync(process.MainWindowHandle, SW_SHOWNORMAL);
                        }
                    }

                }
                catch (Exception)
                {
                    //
                }

                Application.Exit();
                return;
            }
            #endregion

            if (args != null && args.Any(x=>x.Equals("Debug",StringComparison.CurrentCultureIgnoreCase)))
            {
                AllocConsole();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Application.Run(new frmMain());
        }
    }
}
