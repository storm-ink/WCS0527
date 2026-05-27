
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Wcs.App.Plugins.TwoForksCraneManualClient.TwoForksCraneService;

namespace Wcs.App.Plugins.TwoForksCraneManualClient
{
    [DesignTimeVisible(true)]
    public partial class ucAlarm : UserControl
    {
        public String CraneName { get; private set; }
        public ucAlarm(string craneName)
        {
            CraneName = craneName;
            InitializeComponent();

            groupBox1.Text = craneName;
            groupBox1.ForeColor = Color.White;
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
        private void btnHE_Click(object sender, EventArgs e)
        {
            try
            {
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (btnHE.Text.Trim() == "急停")
                    {
                        client.EmergencyStop(this.CraneName);
                    }
                    else
                    {
                        client.CancelEmergencyStop(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }
        private void btnHP_Click(object sender, EventArgs e)
        {
            try
            {
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    client.BackToTheOrigin(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
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
                    lblTaskInfo.Text =
                    lblErrorCode.Text =
                        lblErrorName.Text =
                        lblErrorDescription.Text =
                        lblErrorSolution.Text = "";

                    btnHE.Text = "急停";
                    btnLock.Text = "锁定";
                    btnLock.ForeColor = Color.White;
                }
                else
                {
                    lblTaskInfo.Text = la.TaskId;
                    lblErrorCode.Text = la.ErrorCode.ToString("0000");
                    lblErrorName.Text = la.ErrorName;
                    lblErrorDescription.Text = la.ErrorDescription;
                    lblErrorSolution.Text = la.ErrorSolution;

                    if (la.Event == CraneEvent.EmergencyStop)
                    {
                        btnHE.Text = "取消急停";
                    }
                    else
                    {
                        btnHE.Text = "急停";
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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void btnResetWarn_Click(object sender, EventArgs e)
        {
            try
            {
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    client.ResetWarn(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }
    }
}
