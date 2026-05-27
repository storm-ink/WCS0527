using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using NLog;
using System.Runtime.InteropServices;
namespace Wcs.Client.App
{
    public partial class frmMain : Form, IWcsApplication
    {
        public WcsContext Context;
        public frmMain()
        {
            this.Logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();

            this.Text = string.Format("仓库设备控制系统(v{0})", Assembly.GetExecutingAssembly().GetName().Version);

            var plugins = this.Context.FindPlugins(Application.StartupPath);

            this.Logger.Trace1(string.Format("找到 {0} 个插件，准备加载...", plugins.Count), this);

            foreach (var item in plugins)
            {
                try
                {
                    this.Logger.Trace1(string.Format("加载 {0}({1})...", item.Key.FullName, item.Value.Name), this, item);
                    this.Context.LoadPlugin(item.Key);
                }
                catch (Exception ex)
                {
                    this.Logger.Warn1(string.Format("加载 {0}({1})时发生错误：{2}", item.Key.FullName, item.Value.Name, ex.Message), this, item);
                }
            }

            this.Logger.Trace1("插件加载完成.", this);

            Context = new WcsContext(this);
        }

        public void AddToMenu(object o, WcsApplicationMenuType parent)
        {
            ToolStripMenuItem tsmi;
            switch (parent)
            {
                case WcsApplicationMenuType.File:
                    tsmi = tsmiFile;
                    break;
                case WcsApplicationMenuType.Edit:
                    tsmi = tsmiEdit;
                    break;
                case WcsApplicationMenuType.View:
                    tsmi = tsmiView;
                    break;
                case WcsApplicationMenuType.Tools:
                    tsmi = tsmiTools;
                    break;
                case WcsApplicationMenuType.Help:
                    tsmi = tsmiHelp;
                    break;
                default:
                    throw new NotSupportedException("未处理的菜单类型" + parent);
            }

            tsmi.DropDownItems.Add((ToolStripItem)o);
        }

        public void AddToWorkspace(object o,params object[] args)
        {
            throw new NotImplementedException();
        }

        public void AddToStaus(object o, params object[] args)
        {
            throw new NotImplementedException();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetBevel(false);
        }

        public Logger Logger { get; private set; }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出 " + Application.ProductName + " 吗？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public new MenuStrip Menu
        {
            get { return this.menuStrip1; }
        }

        public StatusStrip Status
        {
            get { return ssMain; }
        }

        public ToolStrip Tool
        {
            get { return mainTools; }
        }

        public Form MainForm
        {
            get
            {
                return this;
            }
        }


        public ToolStripMenuItem GetMenu(WcsApplicationMenuType menu)
        {
            switch (menu)
            {
                case WcsApplicationMenuType.File:
                    return tsmiFile;
                case WcsApplicationMenuType.Edit:
                    return tsmiEdit;
                case WcsApplicationMenuType.View:
                    return tsmiView;
                case WcsApplicationMenuType.Tools:
                    return tsmiTools;
                case WcsApplicationMenuType.Help:
                    return tsmiHelp;
                default:
                    throw new NotSupportedException("未处理的菜单类型" + menu.ToString());
            }
        }

        public Control ButtomDock
        {
            get
            {
                return null;
                //return this.pnlButtomDock;
            }
        }

        private void mainTools_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            mainTools.Visible = mainTools.Items.Count > 0;
        }

        private void mainTools_ItemRemoved(object sender, ToolStripItemEventArgs e)
        {
            mainTools.Visible = mainTools.Items.Count > 0;
        }
    }


    public static class MDIClientSupport
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_CLIENTEDGE = 0x200;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREDRAW = 0x0008;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;
        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_NOSENDCHANGING = 0x0400;

        /// <summary>
        /// 去掉MDI FORM 菜单栏下面的粗边框
        /// </summary>
        /// <param name="form"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        public static bool SetBevel(this Form form, bool show)
        {
            foreach (Control c in form.Controls)
            {
                MdiClient client = c as MdiClient;
                if (client != null)
                {
                    int windowLong = GetWindowLong(c.Handle, GWL_EXSTYLE);

                    if (show)
                    {
                        windowLong |= WS_EX_CLIENTEDGE;
                    }
                    else
                    {
                        windowLong &= ~WS_EX_CLIENTEDGE;
                    }

                    SetWindowLong(c.Handle, GWL_EXSTYLE, windowLong);

                    // Update the non-client area.
                    SetWindowPos(client.Handle, IntPtr.Zero, 0, 0, 0, 0,
                        SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER |
                        SWP_NOOWNERZORDER | SWP_FRAMECHANGED);

                    return true;
                }
            }
            return false;
        }

    }
}
