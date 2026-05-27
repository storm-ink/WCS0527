//using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.CraneManualClient
{
    public partial class frmMain : Form
    {
        /// <summary></summary>
        [DllImport("kernel32.dll ")]
        public static extern int Beep(int dwFreq, int dwDuration);

        /// <summary></summary>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        /// <summary></summary>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        //Logger _logger;
        public frmMain()
        {
            //_logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();
            this.gbxTitle.Click += (sender, e) => { try { this.Close(); } catch { } };

            this.KeyPreview = true;

            pbxLogo.Visible = Properties.Settings.Default.ShowLogo;
            lblTitle.Text = Properties.Settings.Default.CompanyName;
           
            this.MouseMove += new MouseEventHandler(mouseMove);
            gbxTitle.MouseMove += new MouseEventHandler(mouseMove);
            lblTitle.MouseMove += new MouseEventHandler(mouseMove);
            flpCrane.MouseMove += new MouseEventHandler(mouseMove);
            pbxLogo.MouseMove += new MouseEventHandler(mouseMove);
            gbxBack.MouseMove += new MouseEventHandler(mouseMove);
            pnlInfo.MouseMove += new MouseEventHandler(mouseMove);

            pnlInfo.ControlAdded += pnlInfo_ControlAddOrRemoved;
            pnlInfo.ControlRemoved += pnlInfo_ControlAddOrRemoved;
            pnlInfo.VisibleChanged += pnlInfo_VisibleChanged;

            pnlInfo_ControlAddOrRemoved(null, null);
            pnlInfo_VisibleChanged(null, null);
        }

        void mouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 274, 61458, 0);
            }
        }

        void pnlInfo_VisibleChanged(object sender, EventArgs e)
        {
            if (!pnlInfo.Visible)
            {
                flpCrane.Width = 764;
                pnlInfo.Width = 0;
                foreach (var item in flpCrane.Controls.Cast<ucCrane>())
                {
                    item.FillMode = true;
                }
            }
            else
            {
                flpCrane.Width = 224;
                pnlInfo.Width = 540;
                foreach (var item in flpCrane.Controls.Cast<ucCrane>())
                {
                    item.FillMode = false;
                }
            }
        }

        void pnlInfo_ControlAddOrRemoved(object sender, ControlEventArgs e)
        {
            if (pnlInfo.Controls.Count == 0)
            {
                pnlInfo.Visible = false;
            }
            else
            {
                pnlInfo.Visible = true;
            }
        }

        Thread _thread;
        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneService.CraneServiceClient())
                {
                    var infos = client.LoadCraneInfos();
                    if (infos == null)
                    {
                        frmMsgbox.Show("未找到任何堆垛机信息。");
                        Application.Exit();
                        Process.GetCurrentProcess().Kill();
                        return;
                    }

                    int i = 0;


                    var newHeight=this.Height + (56 + 5) * (infos.Length - 8);
                    this.Size = new Size(this.Width, newHeight);
                    foreach (var item in infos)
                    {
                        ucCrane ctl = new ucCrane(item.Name,item.MinColumn,item.MaxColumn,item.MinLevel,item.MaxLevel);
                        ctl.Left = 2;
                        ctl.Top = (ctl.Height - 5) * i;
                        ctl.FillMode = true;
                        //ctl.MouseMove += new MouseEventHandler(mouseMove);
                        flpCrane.Controls.Add(ctl);

                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show("堆垛机信息加载失败。\n"+ex.Message);
                Application.Exit();
                Process.GetCurrentProcess().Kill();
                return;
            }

            _thread = new Thread(timer1_Tick);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void timer1_Tick(object sender)
        {
            while (!this.IsDisposed && !this.Disposing)
            {
                Thread.Sleep(300);
                try
                {
                    Dictionary<string, Wcs.App.Plugins.CraneManualClient.CraneService.LA> status = null;
                    try
                    {
                        using (CraneService.CraneServiceClient client = new CraneService.CraneServiceClient())
                        {
                            status = client.ReadStatus();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    if (status != null && status.Count > 0)
                    {
                        var cranes = flpCrane.Controls.Cast<ucCrane>();
                        foreach (var item in status)
                        {
                            var crnCtl = cranes.SingleOrDefault(x => x.CraneName == item.Key);
                            if (crnCtl != null)
                            {
                                crnCtl.UpdateStatus(item.Value);
                            }

                            var manualCtl = this.pnlInfo.Controls.Cast<Control>().Where(x => x is ucManual).Select(x => x as ucManual)
                                .SingleOrDefault(x => x.CraneName == item.Key);
                            if (manualCtl != null)
                            {
                                manualCtl.UpdateStatus(item.Value);
                            }

                            var alarmCtl = this.pnlInfo.Controls.Cast<Control>().Where(x => x is ucAlarm).Select(x => x as ucAlarm)
                               .SingleOrDefault(x => x.CraneName == item.Key);
                            if (alarmCtl != null)
                            {
                                alarmCtl.UpdateStatus(item.Value);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in flpCrane.Controls.Cast<ucCrane>())
                        {
                            item.UpdateStatus(null);
                        }
                        foreach (var item in this.pnlInfo.Controls.Cast<Control>().Where(x => x is ucManual).Select(x => x as ucManual))
                        {
                             item.UpdateStatus(null);
                        }
                        foreach (var item in  this.pnlInfo.Controls.Cast<Control>().Where(x => x is ucAlarm).Select(x => x as ucAlarm))
                        {
                             item.UpdateStatus(null);
                        }
                    }
                }
                catch (ThreadAbortException threadAbortException)
                {
                    Console.WriteLine(threadAbortException);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                return;
            }

            if (frmMsgbox.Show("确定要退出系统吗？", Btn.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            _thread.Abort();
        }

    }
}
