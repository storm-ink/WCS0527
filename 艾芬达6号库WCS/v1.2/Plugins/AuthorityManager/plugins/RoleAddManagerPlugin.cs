using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Linq;
using Wcs;
using System.Runtime.InteropServices;
using DevExpress.XtraBars;
using System.Reflection;
using Wcs.Security;

namespace Wcs.App.Plugins.AuthorityManager
{
    [WcsPluginInfo(typeof(RoleAddManagerPlugin), "权限管理", "Sineva", "2022年6月", "", true, "权限管理", "角色管理", 4, 0, 0)]
    public class RoleAddManagerPlugin : Wcs.WcsPlugin
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public override bool Initialization(Wcs.WcsContext context)
        {
            //检查并创建初始化角色和用户信息
            init();

            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "添加新角色";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
            {
                GetAllWcsPermissionDlls();
            });

            return base.Initialization(context);
        }

        private void GetAllWcsPermissionDlls()
        {
            try
            {
                List<string> list = new List<string>();
                foreach (var fp in System.IO.Directory.GetFiles(Application.StartupPath, "*.dll"))
                {
                    try
                    {
                        foreach (var t in System.Reflection.Assembly.LoadFile(fp).GetTypes())
                        {
                            foreach (var m in t.GetMethods(
                                System.Reflection.BindingFlags.Instance
                                | System.Reflection.BindingFlags.Public
                                | System.Reflection.BindingFlags.NonPublic
                                | System.Reflection.BindingFlags.Static
                                | System.Reflection.BindingFlags.CreateInstance
                                ))
                            {
                                try
                                {
                                    var x = ((MethodInfo)m);
                                    var y = x.GetCustomAttributes(typeof(WcsPermissionAttribute), true);
                                    var attr = (WcsPermissionAttribute)y.FirstOrDefault();
                                    if (attr != null)
                                    {
                                        list.Add(fp);
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error1(ex, this);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error1(ex, this);
                    }
                }
                Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>("WcsPermissionDlls", string.Join(",", list));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\角色管理\\添加角色")]
        private void tsmi_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (frmRoleEdit frm = new frmRoleEdit(0))
                {
                    frm.ShowDialog();
                }
            }
            catch (System.Security.SecurityException securityException)
            {
                _logger.Error1(securityException, this);
                MessageBox.Show(securityException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is System.Security.SecurityException)
                {
                    MessageBox.Show(ex.InnerException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _logger.Error1(ex, this);
            }
        }

        void init()
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
            {
                Role adminRole = unitOfWork.session.Query<Role>().FirstOrDefault(x => x.RoleName == "管理员");
                if (adminRole == null)
                {
                    adminRole = new Role();
                    adminRole.RoleName = "管理员";
                    adminRole.IsBuiltIn = true;
                    adminRole.Comments = "内置角色";

                    unitOfWork.session.Save(adminRole);

                    _logger.Warn1("未找到内置“管理员”角色，系统自动创建了一个。", this);
                }

                User adminUser = unitOfWork.session.Query<User>().FirstOrDefault(x => x.UserName == "admin");
                if (adminUser == null)
                {
                    adminUser = new User();
                    adminUser.UserName = "admin";
                    adminUser.RealName = "管理员";
                    adminUser.Comments = "内置用户";
                    adminUser.IsBuiltIn = true;
                    adminUser.PasswordSalt = "0a030c30-925f-4e9b-abd9-34193d2770a6";
                    adminUser.Password = UserService.HashPassword("admin@123", adminUser.PasswordSalt);
                    adminUser.AddToRole(adminRole);
                    unitOfWork.session.Save(adminUser);

                    _logger.Warn1("未找到内置“Admin”用户，系统自动创建了一个，并加入了内置“管理员”角色。", this);
                }

                unitOfWork.Commit();
            }
        }
    }
}
