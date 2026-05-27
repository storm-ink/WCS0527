using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHibernate.Linq;
using Wcs;
using Wcs.Security;
using NLog;
using System.Security.Cryptography;

namespace Wcs.App.Plugins.AuthorityManager
{
    public partial class frmChangePassword : Form
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            try
            {
                if (WcsPrincipal.CurrentPrincipal.IsEmpty)
                {
                    MessageBox.Show("您当前未登录",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    this.Close();
                    return ;
                }

                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    User user = null;
                    user = unitOfWork.session.Query<User>().FirstOrDefault(x => x.UserName == WcsPrincipal.CurrentPrincipal.Identity.Name);

                    if (user == null)
                    {
                        throw new Exception(string.Format("未找到用户 {0}", WcsPrincipal.CurrentPrincipal.Identity.Name));
                    }

                    tbxUserName.Text = user.UserName;
                    tbxRealName.Text = user.RealName;
                    tbxEmail.Text = user.Email;

                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var password = tbxPassword.Text.Trim();
                var email = tbxEmail.Text.Trim();

                User user;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {

                    user = unitOfWork.session.Query<User>().Single(x => x.UserName == WcsPrincipal.CurrentPrincipal.Identity.Name);

                    if (!String.IsNullOrWhiteSpace(password))
                    {
                        user.PasswordSalt = Guid.NewGuid().ToString();
                        user.Password = UserService.HashPassword(password, user.PasswordSalt);
                    }
                    user.Email = email;

                    unitOfWork.session.Update(user);

                    unitOfWork.Commit();
                }


                MessageBox.Show("资料保存成功。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
