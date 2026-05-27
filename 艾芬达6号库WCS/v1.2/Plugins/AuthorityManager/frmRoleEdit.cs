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
    public partial class frmRoleEdit : Form
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        static List<WcsPermissionAttribute> wcsPermissionAttributeList = new List<WcsPermissionAttribute>();
        Int32 _id;
        public frmRoleEdit(Int32 roleId)
        {
            InitializeComponent();

            _id = roleId;

            treeView1.Nodes.Clear();

            panel1.Enabled = false;
            label3.Visible = true;
        }

        private void frmRoleEdit_Load(object sender, EventArgs e)
        {
            this.WcsPermissionLoaded += frmRoleEdit_WcsPermissionLoaded;
            this.WcsPermissionLoadError += frmRoleEdit_WcsPermissionLoadError;

            if (wcsPermissionAttributeList == null || wcsPermissionAttributeList.Count == 0)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
                {
                    GetAllWcsPermissionAttributes();
                });
            }
            else
            {
                show(wcsPermissionAttributeList);
            }
        }

        void frmRoleEdit_WcsPermissionLoadError(object sender, EventArgs e)
        {
            MessageBox.Show("角色信息加载失败。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }

        void frmRoleEdit_WcsPermissionLoaded(List<WcsPermissionAttribute> list)
        {
            wcsPermissionAttributeList = list;

            MethodInvoker mi = new MethodInvoker(() =>
            {
                show(list);
            });

            this.Invoke(mi);
        }

        void show(List<WcsPermissionAttribute> list)
        {
            try
            {
                LoadTree(list);

                if (_id > 0)
                {
                    Role role = null;
                    using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                    {
                        role = unitOfWork.session.Get<Role>(_id);
                        unitOfWork.Commit();
                    }

                    if (role == null)
                    {
                        throw new Exception(string.Format("未找到角色 {0}", _id));
                    }

                    tbxName.Text = role.RoleName;
                    tbxComment.Text = role.Comments;

                    if (role.RoleName == "管理员")
                    {
                        treeView1.Enabled = false;
                    }
                    else
                    {
                        treeView1.Enabled = true;
                    }

                    var q = GetAllNodes(null).Where(x => x.Nodes.Count == 0 &&
                        role.Operations.Any(y => y == ((WcsPermissionAttribute)x.Tag).OperationName));
                    q.ForEach((node) =>
                    {
                        //node.Checked = true;
                        treeView1_NodeMouseClick(null, new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, MousePosition.X, MousePosition.Y));
                    });
                }

                panel1.Enabled = true;

                label3.Visible = false;
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
            }
        }

        void GetAllWcsPermissionAttributes()
        {
            List<WcsPermissionAttribute> list = new List<WcsPermissionAttribute>();

            try
            {
                var fps = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<string>("WcsPermissionDlls", "").Split(',');
                //foreach (var fp in System.IO.Directory.GetFiles(Application.StartupPath, "*.dll"))
                foreach (var fp in fps)
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
                                    if (attr != null && !list.Any(a => a.OperationName == attr.OperationName))
                                    {
                                        list.Add(attr);
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

                if (WcsPermissionLoaded != null)
                {
                    WcsPermissionLoaded.Invoke(list);
                }

            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);

                if (WcsPermissionLoadError != null)
                {
                    WcsPermissionLoadError.Invoke(this, EventArgs.Empty);
                }
            }
        }

        void LoadTree(List<WcsPermissionAttribute> list)
        {
            treeView1.Nodes.Clear();

            Dictionary<int, List<TreeNode>> dic = new Dictionary<int, List<TreeNode>>();
            var _list = list.Select(x => x.GetPath()).ToArray();
            var max = _list.Max(x => x.Length);
            for (int i = 0; i < max; i++)
            {
                foreach (var item in _list)
                {
                    TreeNode node;
                    if (item.Length <= i)
                        continue;
                    var text = item[i];
                    string key = "";
                    for (int j = 0; j < i; j++)
                    {
                        if (key == "")
                            key = item[j];
                        else
                            key += $"\\{item[j]}";
                    }

                    if (key == "")
                    {
                        node = new TreeNode();
                        node.Name = text;
                        node.Text = text;
                        if (i + 1 == item.Length)
                            node.Tag = list.First(x => x.OperationName == string.Join("\\", item));

                        if (dic.ContainsKey(i))
                        {
                            if (!dic[i].Any(x => x.Name == text))
                                dic[i].Add(node);
                        }
                        else
                            dic.Add(i, new List<TreeNode>() { node });
                    }
                    else
                    {
                        if (dic.ContainsKey(i))
                        {
                            if (!dic[i].Any(x => x.Name == $"{key}\\{text}"))
                            {
                                var parent = dic[i - 1].First(x => x.Name == key);
                                node = parent.Nodes.Add($"{key}\\{text}", text);
                                if (i + 1 == item.Length)
                                    node.Tag = list.First(x => x.OperationName == string.Join("\\", item));

                                dic[i].Add(node);
                            }
                        }
                        else
                        {
                            var parent = dic[i - 1].First(x => x.Name == key);
                            node = parent.Nodes.Add($"{key}\\{text}", text);
                            if (i + 1 == item.Length)
                                node.Tag = list.First(x => x.OperationName == string.Join("\\", item));
                            dic.Add(i, new List<TreeNode>() { node });
                        }
                    }
                }
            }

            treeView1.Nodes.AddRange(dic[0].ToArray());

            treeView1.ExpandAll();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (node != null)
                return;

            treeView1_NodeMouseClick(null, new TreeNodeMouseClickEventArgs(e.Node, MouseButtons.Left, 1, MousePosition.X, MousePosition.Y));
        }


        List<TreeNode> GetAllNodes(TreeNode parent)
        {
            List<TreeNode> result = new List<TreeNode>();
            if (parent == null)
            {
                foreach (TreeNode item in treeView1.Nodes)
                {
                    result.Add(item);
                    result.AddRange(GetAllNodes(item));
                }
            }
            else
            {
                if (parent.Nodes.Count > 0)
                {
                    foreach (TreeNode item in parent.Nodes)
                    {
                        result.Add(item);
                        result.AddRange(GetAllNodes(item));
                    }
                }
            }

            return result;
        }
        List<String> GetAllowedOptions()
        {
            var nodes = GetAllNodes(null);
            return nodes
                .Where(x => x.Nodes.Count == 0 && x.Checked)
                .Select(x => x.Tag as WcsPermissionAttribute)
                .Select(x => x.OperationName)
                .ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var name = tbxName.Text.Trim();
                var comment = tbxComment.Text.Trim();

                if (String.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("请输入角色名称。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbxName.Focus();
                    return;
                }

                var operations = GetAllowedOptions();

                Role role;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    if (_id <= 0)
                    {
                        role = new Role();
                        role.RoleName = name;
                        role.Comments = comment;
                        role.Operations.AddAll(operations);
                        unitOfWork.session.Save(role);
                    }
                    else
                    {
                        role = unitOfWork.session.Load<Role>(_id);
                        role.Operations.Clear();
                        role.Operations.AddAll(operations);
                        unitOfWork.session.Update(role);
                    }

                    unitOfWork.Commit();
                }


                MessageBox.Show("角色保存成功。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        event WcsPermissionLoadedDelegate WcsPermissionLoaded;
        event EventHandler WcsPermissionLoadError;

        delegate void WcsPermissionLoadedDelegate(List<WcsPermissionAttribute> list);

        TreeNode node = null;
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            node = e.Node;
            node.Checked = !node.Checked;
            foreach (TreeNode item in node.Nodes)
            {
                item.Checked = node.Checked;
                var _node = item;
                while (_node.Nodes != null && _node.Nodes.Count > 0)
                {
                    foreach (TreeNode _item in _node.Nodes)
                    {
                        _item.Checked = node.Checked;
                        _node = _item;
                    }
                }
            }

            var parent = node.Parent;
            while (parent != null)
            {
                if (node.Checked)
                {
                    var update = true;
                    foreach (TreeNode item in parent.Nodes)
                    {
                        if (!item.Checked)
                        {
                            update = false;
                            parent = null;
                            break;
                        }
                    }
                    if (update)
                    {
                        parent.Checked = true;
                        parent = parent.Parent;
                    }
                }
                else 
                {
                    if (parent.Checked)
                        parent.Checked = false;

                    parent = parent.Parent;
                }
            }

            node = null;
        }
    }
}