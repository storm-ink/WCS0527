using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using NHibernate.Linq;
using NLog;
namespace Wcs.App.Plugins.AuthorityManager
{
    public partial class frmUsers : Form
    {
        static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public frmUsers()
        {
            InitializeComponent();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            cmiEdit.Enabled =
                cmiDelete.Enabled = dgvGrid.SelectedRows.Count == 1; 
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\用户管理\\编辑用户")]
        private void cmiEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var user = (User)dgvGrid.CurrentRow.DataBoundItem;
                using (frmUserEdit frm = new frmUserEdit(user.Id))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        load();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\用户管理\\删除用户")]
        private void cmiDelete_Click(object sender, EventArgs e)
        {
            if (dgvGrid.CurrentRow == null)
            {
                return;
            }

            try
            {
                var user = (User)dgvGrid.CurrentRow.DataBoundItem;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    user = unitOfWork.session.Get<User>(user.Id);

                    if (user.IsBuiltIn)
                    {
                        throw new NotSupportedException("无法删除内置用户。");
                    }

                    unitOfWork.session.Delete(user);

                    unitOfWork.Commit();
                }


                load();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmiRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void load()
        {
            List<User> list = new List<User>();
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
            {
                list = unitOfWork.session.Query<User>().ToList();

                unitOfWork.Commit();
            }

            dgvGrid.AutoGenerateColumns = false;
            dgvGrid.DataSource = list;
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\用户管理\\添加用户")]
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmUserEdit frm = new frmUserEdit(-1))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        load();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dgvGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgvGrid.CurrentRow == null)
            {
                return;
            }

            cmiEdit_Click(null, EventArgs.Empty);
        }

        private void frmUsers_SizeChanged(object sender, EventArgs e)
        {
            dgvGrid.Width = this.Width - dgvGrid.Left;
            btnAdd.Left = dgvGrid.Width - btnAdd.Width;
        }
    }
}
