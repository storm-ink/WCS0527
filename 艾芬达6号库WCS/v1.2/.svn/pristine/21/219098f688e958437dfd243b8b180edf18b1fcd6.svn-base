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
    public partial class frmRoles : Form
    {
        static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public frmRoles()
        {
            InitializeComponent();
        }

        private void frmRoles_Load(object sender, EventArgs e)
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

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\角色管理\\编辑角色")]
        private void cmiEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var role = (Role)dgvGrid.CurrentRow.DataBoundItem;
                using (frmRoleEdit frm = new frmRoleEdit(role.Id))
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

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\角色管理\\删除角色")]
        private void cmiDelete_Click(object sender, EventArgs e)
        {
            if (dgvGrid.CurrentRow == null)
            {
                return;
            }

            try
            {
                var role = (Role)dgvGrid.CurrentRow.DataBoundItem;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    if (unitOfWork.session.Query<User>().Any(x => x.Roles.Any(r => r.Id == role.Id)))
                    {
                        throw new Exception(string.Format("无法删除{0}，原因是已被用户引用。", role));
                    }
                    role = unitOfWork.session.Get<Role>(role.Id);


                    if (role.IsBuiltIn)
                    {
                        throw new NotSupportedException("无法删除内置角色。");
                    }


                    unitOfWork.session.Delete(role);

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
            List<Role> list = new List<Role>();
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
            {
                list = unitOfWork.session.Query<Role>().ToList();

                unitOfWork.Commit();
            }

            dgvGrid.AutoGenerateColumns = false;
            dgvGrid.DataSource = list;
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\角色管理\\添加角色")]
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmRoleEdit frm = new frmRoleEdit(-1))
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

        private void frmRoles_SizeChanged(object sender, EventArgs e)
        {
            dgvGrid.Width = this.Width - dgvGrid.Left;
            btnAdd.Left = dgvGrid.Width - btnAdd.Width;
        }
    }
}
