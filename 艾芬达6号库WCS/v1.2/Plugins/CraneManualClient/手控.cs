
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Wcs.App.Plugins.CraneManualClient.CraneService;
using System.Collections.Generic;

namespace Wcs.App.Plugins.CraneManualClient
{
    [DesignTimeVisible(true)]
    public partial class ucManual : UserControl
    {
        public String CraneName { get; private set; }
        public int MinColumn { get; private set; }
        public int MaxColumn { get; private set; }
        public int MinLevel { get; private set; }
        public int MaxLevel { get; private set; }

        public ucManual(String craneName, int minColumn, int maxColumn, int minLevel, int maxLevel)
        {
            CraneName = craneName;
            MinColumn = minColumn;
            MaxColumn = maxColumn;
            MinLevel = minLevel;
            MaxLevel = maxLevel;

            InitializeComponent();

            groupBox1.Text = craneName;
            groupBox1.ForeColor = Color.White;
            lblCol.ForeColor = lblRow.ForeColor = lblTaskId.ForeColor =
            lblForkTL.ForeColor = lblForkLR.ForeColor = lblAlarmCode.ForeColor = Color.Yellow;

            List<DropdownItem> items = new List<DropdownItem>();

            if (System.Configuration.ConfigurationManager.AppSettings["双伸位"] == "true"
                || System.Configuration.ConfigurationManager.AppSettings["双伸位"] == "1")
            {
                btnLPut2.Visible =
                    btnLGet2.Visible =
                    btnRPut2.Visible =
                    btnRGet2.Visible = true;

                items.AddRange(new DropdownItem[]
                {
                    new DropdownItem{ ForkDirection=ForkDirection.Left, Text = "左"},
                    new DropdownItem{ ForkDirection=ForkDirection.Left2,Text = "左外"},
                    new DropdownItem{ ForkDirection=ForkDirection.Right,Text = "右"},
                    new DropdownItem{ ForkDirection=ForkDirection.Right2,Text = "右外"},

                });

            }
            else
            {
                btnLPut2.Visible =
                    btnLGet2.Visible =
                    btnRPut2.Visible =
                    btnRGet2.Visible = false;

                items.AddRange(new DropdownItem[]
                {
                    new DropdownItem{ ForkDirection=ForkDirection.Left, Text = "左"},
                    new DropdownItem{ ForkDirection=ForkDirection.Right,Text = "右"}

                });
            }

            cbxForkDirection.Items.Clear();
            cbxForkDirection.Items.AddRange(items.ToArray());
            cbxForkDirection.SelectedIndex = 0;
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
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
        private void btnHP_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
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
        private void btnHE_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void btnCol_Click(object sender, EventArgs e)
        {
            var result = frmInput.Show(btnUCol.Text,
                    this.ParentForm.Left + 348, this.ParentForm.Top + 265, MaxColumn, MinColumn);

            if (!string.IsNullOrWhiteSpace(result))
            {
                btnUCol.Text = result;
            }
        }

        private void btnURow_Click(object sender, EventArgs e)
        {
            var result = frmInput.Show(btnURow.Text,
                    this.ParentForm.Left + 348, this.ParentForm.Top + 265, MaxLevel, MinLevel);

            if (!string.IsNullOrWhiteSpace(result))
            {
                btnURow.Text = result;
            }
        }

        private void btnLGet_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnLGet2_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnLPut_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnLPut2_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnRGet_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnRGet2_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnRPut_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnRPut2_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnCUp_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Up(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnCDown_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Down(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnCBack_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Back(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnCGo_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    client.Forward(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        private void btnRunRowCol_Click(object sender, EventArgs e)
        {
            try
            {
                using (CraneService.CraneServiceClient client = new CraneServiceClient())
                {
                    var item = (DropdownItem)cbxForkDirection.SelectedItem;
                    client.MoveByForkDirection(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), Convert.ToInt32(btnUCol.Text), Convert.ToInt32(btnURow.Text), item.ForkDirection);
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
                    lblState.Text =
                        lblCol.Text =
                        lblRow.Text =
                        lblTaskId.Text =
                        lblForkLR.Text =
                        lblForkTL.Text =
                        lblAlarmCode.Text = string.Empty;

                    btnHE.Text = "急停";
                    btnLock.Text = "锁定";
                    btnLock.ForeColor = Color.White;

                    //this.Enabled = false;
                }
                else
                {
                    switch (la.ForkHorizontalPosition)
                    {
                        case ForkHorizontalPosition.Center:
                            lblForkLR.Text = "中位";
                            break;
                        case ForkHorizontalPosition.Left:
                            lblForkLR.Text = "左";
                            break;
                        case ForkHorizontalPosition.LeftLimit:
                            lblForkLR.Text = "左极限";
                            break;
                        case ForkHorizontalPosition.Right:
                            lblForkLR.Text = "右";
                            break;
                        case ForkHorizontalPosition.RightLimit:
                            lblForkLR.Text = "右极限";
                            break;
                        case ForkHorizontalPosition.E6:
                            lblForkLR.Text = "左（外）";
                            break;
                        case ForkHorizontalPosition.E7:
                            lblForkLR.Text = "左（外）极限";
                            break;
                        case ForkHorizontalPosition.E8:
                            lblForkLR.Text = "右（外）";
                            break;
                        case ForkHorizontalPosition.E9:
                            lblForkLR.Text = "右（外）极限";
                            break;
                        default:
                            lblForkLR.Text = la.ForkHorizontalPosition.ToString();
                            break;
                    }

                    switch (la.ForkVerticalPosition)
                    {
                        case ForkVerticalPosition.Middle:
                            lblForkTL.Text = "中位";
                            break;
                        case ForkVerticalPosition.Top:
                            lblForkTL.Text = "高位";
                            break;
                        case ForkVerticalPosition.Bottom:
                            lblForkTL.Text = "低位";
                            break;
                        default:
                            lblForkTL.Text = la.ForkVerticalPosition.ToString();
                            break;
                    }

                    if (la.Event == CraneEvent.EmergencyStop)
                    {
                        btnHE.Text = "取消急停";
                    }
                    else
                    {
                        btnHE.Text = "急停";
                    }

                    lblCol.Text = la.UserColumn;
                    lblRow.Text = la.UserLevel;
                    lblTaskId.Text = la.TaskId;
                    lblAlarmCode.Text = la.ErrorCode.ToString("0000");

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

        class DropdownItem
        {
            public ForkDirection ForkDirection { get; set; }
            public String Text { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }
    }
}