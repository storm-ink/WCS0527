using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Wcs.Security;
using NHibernate.Linq;

namespace Wcs.App.Plugins.AuthorityManager
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = tbxUserName.Text;
            string password = tbxUserPwd.Text;

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("用户名不能为空", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxUserName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("密码不能为空", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxUserPwd.Focus();
                return;
            }


            User user = null;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
            {
                user = unitOfWork.session.Query<User>()
                    .Fetch(x=>x.Roles)
                    .FirstOrDefault(x => x.UserName == userName);
                unitOfWork.Commit();
            }

            if (user == null)
            {
                MessageBox.Show("用户不存在，请重新输入！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxUserName.Focus();
                return;
            }

            if (UserService.HashPassword(password, user.PasswordSalt) != user.Password)
            {
                MessageBox.Show("密码错误，请重新输入！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxUserPwd.Focus();
                return;
            }

            UserService.Login(user);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
