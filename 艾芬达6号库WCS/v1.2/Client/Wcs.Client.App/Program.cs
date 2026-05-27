using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.Client.App
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (frmLogin frm = new frmLogin())
            {
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            Application.Run(new frmMain());
        }
    }
}
