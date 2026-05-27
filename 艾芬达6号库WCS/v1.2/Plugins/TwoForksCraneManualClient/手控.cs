
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Wcs.App.Plugins.TwoForksCraneManualClient.TwoForksCraneService;
using System.Collections.Generic;
using System.Linq;
namespace Wcs.App.Plugins.TwoForksCraneManualClient
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
            groupBox2.ForeColor = Color.White;
            lblUserCode.ForeColor = lblColRow.ForeColor = lblTaskId.ForeColor =
            lblForkTL1.ForeColor = lblForkTL2.ForeColor = lblAlarmCode.ForeColor = Color.Yellow;

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
                    new DropdownItem{ ForkDirection=ForkDirection.Left, Text = "叉1", AlignToFork1=true},
                    new DropdownItem{ ForkDirection=ForkDirection.Left2,Text = "叉2"},

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
                    new DropdownItem{ ForkDirection=ForkDirection.Left, Text = "叉1", AlignToFork1=true},
                    new DropdownItem{ ForkDirection=ForkDirection.Left2,Text = "叉2"},

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

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        private void btnCol_Click(object sender, EventArgs e)
        {
            //var result = frmInput.Show(btnUCol.Text,
            //        this.ParentForm.Left + 348, this.ParentForm.Top + 265, MaxColumn, MinColumn);

            String[] locations = null;

            try
            {
                locations = System.Configuration.ConfigurationManager.AppSettings[CraneName].Split(',')
                    .Cast<String>()
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            String result;
            var dr = frmInput1.Show(btnUCol.Text,
                    this.ParentForm.Left + 348, this.ParentForm.Top + 265, locations,out result);
            if (dr==DialogResult.OK)
            {
                btnUCol.Text = result;
            }
        }

        private void btnURow_Click(object sender, EventArgs e)
        {
            //var result = frmInput.Show(btnURow.Text,
            //        this.ParentForm.Left + 348, this.ParentForm.Top + 265, MaxLevel, MinLevel);

            //if (!string.IsNullOrWhiteSpace(result))
            //{
            //    btnURow.Text = result;
            //}
        }

        private void btnLGet_Click(object sender, EventArgs e)
        {
            try
            {
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left, null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), null, ForkDirection.Left);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left, 获取叉2方向(ForkDirection.Left));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");
                    }
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left2, null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), null, ForkDirection.Left2);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left2, 获取叉2方向(ForkDirection.Left2));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");
                    }
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left, null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), null, ForkDirection.Left);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left, 获取叉2方向(ForkDirection.Left));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");
                    }

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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left2, null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), null, ForkDirection.Left2);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Left2, 获取叉2方向(ForkDirection.Left2));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                frmMsgbox.Show(ex.Message);
            }
        }

        ForkDirection? 获取叉2方向(ForkDirection fork1Direction)
        {
            if (cbxHCFX.Text == "同向同深")
            {
                return fork1Direction;
            }
            else if(cbxHCFX.Text == "反向同深")
            {
                switch (fork1Direction)
                {
                    case ForkDirection.Left:
                        return ForkDirection.Right;
                    case ForkDirection.Right:
                        return ForkDirection.Left;
                    case ForkDirection.Left2:
                        return ForkDirection.Right2;
                    case ForkDirection.Right2:
                        return ForkDirection.Left2;
                    default:
                        throw new NotSupportedException(fork1Direction.ToString());
                }
            }
            else if(cbxHCFX.Text == "同向反深")
            {
                switch (fork1Direction)
                {
                    case ForkDirection.Left:
                        return ForkDirection.Left2;
                    case ForkDirection.Right:
                        return ForkDirection.Right2;
                    case ForkDirection.Left2:
                        return ForkDirection.Left;
                    case ForkDirection.Right2:
                        return ForkDirection.Right;
                    default:
                        throw new NotSupportedException(fork1Direction.ToString());
                }
            }
            else if (cbxHCFX.Text == "反向反深")
            {
                switch (fork1Direction)
                {
                    case ForkDirection.Left:
                        return ForkDirection.Right2;
                    case ForkDirection.Right:
                        return ForkDirection.Left2;
                    case ForkDirection.Left2:
                        return ForkDirection.Right;
                    case ForkDirection.Right2:
                        return ForkDirection.Left;
                    default:
                        throw new NotSupportedException(fork1Direction.ToString());
                }
            }
            else
            {
                throw new NotSupportedException(cbxHCFX.Text);
            }
        }

        private void btnRGet_Click(object sender, EventArgs e)
        {
            try
            {
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right, null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), null, ForkDirection.Right);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right, 获取叉2方向(ForkDirection.Right));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");
                    }
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right2, null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), null, ForkDirection.Right2);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Pick(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right2, 获取叉2方向(ForkDirection.Right2));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");
                    }
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(),ForkDirection.Right,null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(),null, ForkDirection.Right);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right, 获取叉2方向(ForkDirection.Right));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");
                    }
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    if (rbOnlyOne.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right2, null);
                    }
                    else if (rbOnlyTwo.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), null, ForkDirection.Right2);
                    }
                    else if (rbDouble.Checked)
                    {
                        client.Putdown(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), ForkDirection.Right2, 获取叉2方向(ForkDirection.Right2));
                    }
                    else
                    {
                        throw new Exception("请选择要使用的货叉.");
                    }
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
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
                using (TwoForksCraneServiceClient client = new TwoForksCraneServiceClient())
                {
                    var item = (DropdownItem)cbxForkDirection.SelectedItem;
                    //client.MoveByForkDirectionAndFork(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), Convert.ToInt32(btnUCol.Text), Convert.ToInt32(btnURow.Text), item.ForkDirection, item.AlignToFork1);
                    client.MoveByForkDirectionAndFork(this.CraneName, LockerHelper.GetUserName(), LockerHelper.GetIpAddress(), btnUCol.Text.Trim(), item.AlignToFork1);

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
                    //lblState.Text =
                        lblUserCode.Text =
                        lblColRow.Text =
                        lblTaskId.Text =
                        lblForkTL1.Text =
                        lblForkTL2.Text =
                        lblGDW.Text =
                        lblAlarmCode.Text = string.Empty;

                    btnHE.Text = "急停";
                    btnLock.Text = "锁定";
                    btnLock.ForeColor = Color.White;

                    //this.Enabled = false;
                }
                else
                {
                    lblForkTL1.Text = string.Format("{0} {1}", getForkHorizontalPositionName(la.Fork1HorizontalPosition), getForkStatusName(la.Fork1Status));
                    lblForkTL2.Text = string.Format("{0} {1}", getForkHorizontalPositionName(la.Fork2HorizontalPosition), getForkStatusName(la.Fork2Status));
                    
                    switch (la.ForkVerticalPosition)
                    {
                        case ForkVerticalPosition.Middle:
                            lblGDW.Text = "中位";
                            break;
                        case ForkVerticalPosition.Top:
                            lblGDW.Text = "高位";
                            break;
                        case ForkVerticalPosition.Bottom:
                            lblGDW.Text = "低位";
                            break;
                        default:
                            lblGDW.Text = la.ForkVerticalPosition.ToString();
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

                    lblUserCode.Text = la.UserCode;
                    lblColRow.Text = String.Format("{0}列{1}层",la.UserColumn,la.UserLevel);
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

        String getForkHorizontalPositionName(ForkHorizontalPosition forkHorizontalPosition)
        {
            switch (forkHorizontalPosition)
            {
                case ForkHorizontalPosition.Center:
                    return "中位";
                case ForkHorizontalPosition.Left:
                    return "左";
                case ForkHorizontalPosition.LeftLimit:
                    return "左极限";
                case ForkHorizontalPosition.Right:
                    return "右";
                case ForkHorizontalPosition.RightLimit:
                    return "右极限";
                case ForkHorizontalPosition.E6:
                    return "左（外）";
                case ForkHorizontalPosition.E7:
                    return "左（外）极限";
                case ForkHorizontalPosition.E8:
                    return "右（外）";
                case ForkHorizontalPosition.E9:
                    return "右（外）极限";
                default:
                    return forkHorizontalPosition.ToString();
            }
        }

        String getForkStatusName(ForkStatus forkStatus)
        {
            switch (forkStatus)
            {
                case  ForkStatus.故障:
                    return "故障";
                case ForkStatus.无货待命:
                    return "无货待命";
                case ForkStatus.有货待命:
                    return "有货待命";
                default:
                    return forkStatus.ToString();
            }
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


        class DropdownItem
        {
            public ForkDirection ForkDirection { get; set; }
            public String Text { get; set; }
            public bool AlignToFork1 { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }
    }
}