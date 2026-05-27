
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Wcs.App.Plugins.TwoForksCraneManualClient.TwoForksCraneService;
using System.Linq;
namespace Wcs.App.Plugins.TwoForksCraneManualClient
{
    [DesignTimeVisible(true)]
    public partial class ucCrane : UserControl
    {
        public String CraneName { get; private set; }
        public int MinColumn { get; private set; }
        public int MaxColumn { get; private set; }
        public int MinLevel { get; private set; }
        public int MaxLevel { get; private set; }
        public ucCrane(string sCraneName, int minColumn, int maxColumn, int minLevel, int maxLevel)
            : this()
        {
            CraneName = sCraneName;
            MinColumn = minColumn;
            MaxColumn = maxColumn;
            MinLevel = minLevel;
            MaxLevel = maxLevel;

            this.lblCraneName.Text = this.CraneName;
        }

        public ucCrane()
        {
            InitializeComponent();
        }

        private void ucCrane_Load(object sender, EventArgs e)
        {
            
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            try
            {
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (btnLock.Text == "锁定")
                    {
                        client.Lock(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                    }
                    else
                    {
                        client.Unlock(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }
        private void btnManual_Click(object sender, EventArgs e)
        {
            frmMain frm = (frmMain)this.ParentForm;
            foreach (Control item in frm.pnlInfo.Controls)
            {
                item.Dispose();
                frm.pnlInfo.Controls.Remove(item);
            }

            var ctl = new ucManual(this.CraneName,this.MinColumn,this.MaxColumn,this.MinLevel,this.MaxLevel);
            ctl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top; 
            frm.pnlInfo.Controls.Add(ctl);
        }
        private void btnAlarm_Click(object sender, EventArgs e)
        {
            frmMain frm = (frmMain)this.ParentForm;
            foreach (Control item in frm.pnlInfo.Controls)
            {
                item.Dispose();
                frm.pnlInfo.Controls.Remove(item);
            }

            var ctl = new ucAlarm(this.CraneName);
            ctl.Dock = DockStyle.Fill;
            frm.pnlInfo.Controls.Add(ctl);
        }

        public void UpdateStatus(LA la)
        {
            if (this.InvokeRequired)
            {
                Action<LA> act = (_la) =>
                {
                    UpdateStatus(_la);
                };

                this.Invoke(act, la);
            }
            else
            {
                if (la == null)
                {
                    lblState.Text =
                        lblEvent.Text =
                        lblColRow.Text =
                        lblUserCode.Text =
                        lblTaskId.Text =
                        lblAlarmCode.Text = string.Empty;

                    pbxOnline.Image = Properties.Resources.unline;
                    pbxError.Image = Properties.Resources.alarm;

                    btnLock.Text = "锁定";
                    btnLock.ForeColor = Color.White;

                    //this.Enabled = false;
                }
                else
                {
                    switch (la.State)
                    {
                        case CraneStatus.Initialized:
                            lblState.Text = "初始化";
                            break;
                        case CraneStatus.BackToTheOrigin:
                            lblState.Text = "回原点";
                            break;
                        case CraneStatus.无货待命:
                            lblState.Text = "无货待命";
                            break;
                        case CraneStatus.有货待命:
                            lblState.Text = "有货待命";
                            break;
                        case CraneStatus.无货运行:
                            lblState.Text = "无货运行";
                            break;
                        case CraneStatus.有货运行:
                            lblState.Text = "有货运行";
                            break;
                        case CraneStatus.Pickup:
                            lblState.Text = "取货";
                            break;
                        case CraneStatus.Putin:
                            lblState.Text = "放货";
                            break;
                        case CraneStatus.AlarmAndShutdown:
                            lblState.Text = "报警停机";
                            break;
                        case CraneStatus.ResetAlarm:
                            lblState.Text = "报警重置";
                            break;
                        case CraneStatus.奇怪的状态:
                            lblState.Text = "???";
                            break;
                        case CraneStatus.Disconnected:
                            lblState.Text = "已断开";
                            break;
                        case CraneStatus.ManualMode:
                            lblState.Text = "手动";
                            break;
                        default:
                            lblState.Text = la.State.ToString();
                            break;
                    }

                    switch (la.Event)
                    {
                        case CraneEvent.Initialized:
                            lblEvent.Text = "初始化";
                            break;
                        case CraneEvent.BeginRunning:
                            lblEvent.Text = "开始运行";
                            break;
                        case CraneEvent.BeginPickup:
                            lblEvent.Text = "开始取货";
                            break;
                        case CraneEvent.EndPickup:
                            lblEvent.Text = "取货完成";
                            break;
                        case CraneEvent.BeginPutin:
                            lblEvent.Text = "开始放货";
                            break;
                        case CraneEvent.EndPutin:
                            lblEvent.Text = "放货完成";
                            break;
                        case CraneEvent.Finished:
                            lblEvent.Text = "完成";
                            break;
                        case CraneEvent.EmergencyStop:
                            lblEvent.Text = "急停";
                            break;
                        case CraneEvent.CompletedWithError:
                            lblEvent.Text = "出错完成";
                            break;
                        case CraneEvent.BackToTheOrigin:
                            lblEvent.Text = "回原点";
                            break;
                        default:
                            lblEvent.Text = la.Event.ToString();
                            break;
                    }

                    lblColRow.Text = la.UserColumn + " " + la.UserLevel;
                    lblUserCode.Text = la.UserCode;
                    lblTaskId.Text = la.TaskId;
                    lblAlarmCode.Text = string.Format("{0:d4} {1}", la.ErrorCode, la.ErrorName);

                    pbxOnline.Image = Properties.Resources.online;

                    if (la.ErrorCode != 0)
                    {
                        pbxError.Image = Properties.Resources.alarm;
                    }
                    else
                    {
                        pbxError.Image = Properties.Resources.online;
                    }

                    //有锁定，但不是自己
                    if (!String.IsNullOrWhiteSpace(la.LockerUser) && !String.Equals(la.LockerUser, LockerHelper.GetUserName(), StringComparison.CurrentCultureIgnoreCase) && !String.Equals(la.LockerIp, LockerHelper.GetIpAddress(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        btnLock.Enabled = false;
                        btnLock.Text = "解锁";
                        btnLock.ForeColor = Color.Tan;
                    }
                    //有锁定是自己
                    else if (String.Equals(la.LockerUser, LockerHelper.GetUserName(), StringComparison.CurrentCultureIgnoreCase) && String.Equals(la.LockerIp, LockerHelper.GetIpAddress(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        btnLock.Enabled = true;
                        btnLock.Text = "解锁";
                        btnLock.ForeColor = Color.DeepPink;
                    }
                    //没锁定
                    else if (String.IsNullOrWhiteSpace(la.LockerUser))
                    {
                        btnLock.Enabled = true;
                        btnLock.Text = "锁定";
                        btnLock.ForeColor = Color.Wheat;
                    }
                    else
                    {
                        btnLock.Enabled = true;
                        btnLock.Text = "锁定";
                        btnLock.ForeColor = Color.Wheat;
                    }
                }
            }
        }

        public Boolean FillMode
        {
            set
            {
                if (value)
                {
                    this.gbxCrane.Width = 758 ;
                }
                else
                {
                    this.gbxCrane.Width = 218;
                }
            }
        }
    }
}
