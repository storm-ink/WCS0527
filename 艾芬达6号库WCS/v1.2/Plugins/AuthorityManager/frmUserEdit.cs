using NLog;
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
using System.Security.Cryptography;

namespace Wcs.App.Plugins.AuthorityManager
{
    public partial class frmUserEdit : Form
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        Int32 _id;
        public frmUserEdit(Int32 userId)
        {
            InitializeComponent();

            _id = userId;
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
                var userName = tbxUserName.Text.Trim();
                var password = tbxPassword.Text.Trim();
                var realName = tbxRealName.Text.Trim();
                var email = tbxEmail.Text.Trim();
                var comments = tbxComments.Text.Trim();
                var roles = clbRoles.CheckedItems.Cast<Role>().ToArray();

                if (String.IsNullOrWhiteSpace(userName))
                {
                    MessageBox.Show("请输入用户名称。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbxUserName.Focus();
                    return;
                }

                if (_id<=0 && String.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("请输入密码。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbxPassword.Focus();
                    return;
                }

                if (String.IsNullOrWhiteSpace(realName))
                {
                    MessageBox.Show("请输入真实姓名。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbxRealName.Focus();
                    return;
                }

                User user;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    var roleIds=roles.Select(x=>x.Id).ToArray();
                    roles = unitOfWork.session.Query<Role>().Where(x => roleIds.Contains(x.Id))
                        .ToArray();

                    if (_id <= 0)
                    {
                        user = new User();
                        user.UserName = userName;
                        user.RealName = realName;
                        user.IsBuiltIn = false;
                        user.PasswordSalt = Guid.NewGuid().ToString();
                        user.Password = UserService.HashPassword(password, user.PasswordSalt);
                        user.Email = email;
                        user.Comments = comments;
                        user.AddToRoles(roles);

                        unitOfWork.session.Save(user);
                    }
                    else
                    {
                        user = unitOfWork.session.Load<User>(_id);
                        if (!user.IsBuiltIn)
                        {
                            user.UserName = userName;
                            user.RealName = realName;

                            user.Roles.Clear();
                            user.AddToRoles(roles);
                        }
                        if (!String.IsNullOrWhiteSpace(password))
                        {
                            user.PasswordSalt = Guid.NewGuid().ToString();
                            user.Password = UserService.HashPassword(password, user.PasswordSalt);
                        }
                        user.Email = email;
                        user.Comments = comments;

                        unitOfWork.session.Update(user);
                    }

                    unitOfWork.Commit();
                }


                MessageBox.Show("用户保存成功。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmUserEdit_Load(object sender, EventArgs e)
        {
            try
            {
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    var roles = unitOfWork.session.Query<Role>().ToArray();
                    clbRoles.Items.AddRange(roles);

                    if (_id > 0)
                    {
                        User user = null;
                        user = unitOfWork.session.Get<User>(_id);

                        if (user == null)
                        {
                            throw new Exception(string.Format("未找到用户 {0}", _id));
                        }

                        tbxUserName.Text = user.UserName;
                        tbxRealName.Text = user.RealName;
                        tbxEmail.Text = user.Email;
                        tbxComments.Text = user.Comments;

                        if (user.IsBuiltIn)
                        {
                            tbxUserName.Enabled = false;
                            tbxRealName.Enabled = false;
                            clbRoles.Enabled = false;
                            lblIsBuiltIn.Text = "是";
                        }
                        else
                        {
                            tbxUserName.Enabled = true;
                            tbxRealName.Enabled = true;
                            clbRoles.Enabled = true;
                            lblIsBuiltIn.Text = "否";
                        }
                        
                        for (int i = 0; i < clbRoles.Items.Count; i++)
			            {
                            var r = (Role)clbRoles.Items[i];
                            if (user.Roles.Any(x => x.Id == r.Id))
                            {
                                clbRoles.SetItemChecked(i, true);
                            }
			            }
                    }

                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
