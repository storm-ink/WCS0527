using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Linq;
using Wcs.Framework;

namespace Wcs.App.Plugins.TwoForksTaskViewer
{
    public partial class frmTasks : Form
    {
        public frmTasks()
        {
            InitializeComponent();

            cbxDeviceNames.Items.Clear();
            cbxDeviceNames.Items.Add("");
            foreach (var item in Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x=>x.DeviceElements)
                .Where(x=>x.Device is Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneDevice)
                )
            {
                cbxDeviceNames.Items.Add(item.Name);
            }

            dgvGrid.AutoGenerateColumns = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (Math.Abs(dtpEndDate.Value.Subtract(dtpStartDate.Value).TotalHours) > 12)
            {
                MessageBox.Show("最多支持时间区间最大为12小时，请修正后重试。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDate.Focus();
                return;
            }

            System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
            {
                load();
            });
        }

        void load()
        {

            try
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    groupBox1.Enabled = false;
                }));

                List<Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneJob> list = new List<DefaultImpls.TwoForksCrane.TwoForksCraneJob>();
                using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(IsolationLevel.ReadUncommitted))
                {
                    var q = from o in unitOfWork
                            .session
                            .Query<Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneJob>()
                            select o;

                    q = q.Where(x => x.CreatedAt >= dtpStartDate.Value && x.CreatedAt < dtpEndDate.Value);

                    int v;
                    if (int.TryParse(tbxEquipmentTaskID.Text, out v) && v > 0)
                    {
                        q = q.Where(x => x.EquipmentTaskId == v || x.Fork1EquipmentAction.EquipmentTaskId==v || x.Fork2EquipmentAction.EquipmentTaskId==v);
                    }

                    if (!String.IsNullOrWhiteSpace(cbxDeviceNames.Text))
                    {
                        q = q.Where(x => x.DeviceName == cbxDeviceNames.Text.Trim());
                    }

                    if (!String.IsNullOrWhiteSpace(tbxContainerCode.Text))
                    {
                        q = q.Where(x => x.Fork1EquipmentAction.Movement.Task.ContainerCodes.Contains(tbxContainerCode.Text.Trim())
                        || x.Fork2EquipmentAction.Movement.Task.ContainerCodes.Contains(tbxContainerCode.Text.Trim()));
                    }

                    if (cbxSingleFork.CheckState == CheckState.Checked)
                    {
                        q = q.Where(x =>
                            (x.Fork1EquipmentAction != null && x.Fork2EquipmentAction == null)
                            || (x.Fork1EquipmentAction == null && x.Fork2EquipmentAction != null)
                        );
                    }
                    else if (cbxSingleFork.CheckState == CheckState.Unchecked)
                    {
                        q = q.Where(x => x.Fork1EquipmentAction != null && x.Fork2EquipmentAction != null);
                    }

                    list = q.Take(Convert.ToInt32(numericUpDown1.Value))
                        .ToList();

                    unitOfWork.Commit();

                }

                this.Invoke(new MethodInvoker(() =>
                {
                    dgvGrid.DataSource = null;
                    dgvGrid.DataSource = list;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    groupBox1.Enabled = true;
                }));
            }
        }

        private void dgvGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                var act = (Wcs.DefaultImpls.TwoForksCrane.TwoForksCraneJob)dgvGrid.Rows[e.RowIndex].DataBoundItem;
                if (act == null)
                {
                    return;
                }

                var cell = dgvGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (e.ColumnIndex == colFork1.Index && act.Fork1EquipmentAction!=null)
                {
                    if (cell.Tag == null)
                    {
                        cell.Tag = string.Format("【{9}】{0}->{1},{2}#{3},{4}#{5}#{6}#{7}@{8:HH:mm:ss.fff}",
                                    act.Fork1EquipmentAction.Movement.StartLocation,
                                    act.Fork1EquipmentAction.Movement.EndLocation,
                                    act.Fork1EquipmentAction.Id,
                                    act.Fork1EquipmentAction.EquipmentTaskId,
                                    act.Fork1EquipmentAction.Movement.Task.Id,
                                    act.Fork1EquipmentAction.Movement.Task.TaskCode,
                                    act.Fork1EquipmentAction.Movement.Task.MasterTaskCode,
                                    act.Fork1EquipmentAction.Movement.Task.TaskType,
                                    act.Fork1EquipmentAction.Movement.Task.CreatedAt,
                                    String.Join(",", act.Fork1EquipmentAction.Movement.Task.ContainerCodes.ToArray())
                                    );
                    }

                    e.Value = cell.Tag;
                }
                else if(e.ColumnIndex == colFork2.Index && act.Fork2EquipmentAction != null)
                {
                    if (cell.Tag == null)
                    {
                        cell.Tag = string.Format("【{9}】{0}->{1},{2}#{3},{4}#{5}#{6}#{7}@{8:HH:mm:ss.fff}",
                        act.Fork2EquipmentAction.Movement.StartLocation,
                        act.Fork2EquipmentAction.Movement.EndLocation,
                        act.Fork2EquipmentAction.Id,
                        act.Fork2EquipmentAction.EquipmentTaskId,
                        act.Fork2EquipmentAction.Movement.Task.Id,
                        act.Fork2EquipmentAction.Movement.Task.TaskCode,
                        act.Fork2EquipmentAction.Movement.Task.MasterTaskCode,
                        act.Fork2EquipmentAction.Movement.Task.TaskType,
                        act.Fork2EquipmentAction.Movement.Task.CreatedAt,
                        String.Join(",", act.Fork2EquipmentAction.Movement.Task.ContainerCodes.ToArray())
                        );
                    }

                    e.Value = cell.Tag;
                }
            }
            catch (Exception)
            {
                
            }
        }


        private void dgvList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, new SolidBrush(Color.Black), e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }
    }
}
