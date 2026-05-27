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
using Wcs.Framework;
using Wcs.Framework.Cfg;
using System.IO;
using NLog;

namespace Wcs.App.Plugins.Tools
{
    public partial class frmRouteTest : Form
    {
        Logger _logger = LogManager.GetCurrentClassLogger();
        public frmRouteTest()
        {
            InitializeComponent();
            try
            {
                label4.Text = "";
                var locations = WcsConfiguration
                .Instance
                .LocationCollection
                .Locations.Where(x => !(x is Wcs.Framework.ILocationWildcard));

                AutoCompleteStringCollection locationsAutoCompleteStringCollection = locationsConvertToAutoCompleteStringCollection(locations);
                tbxStartLocation.AutoCompleteCustomSource = locationsAutoCompleteStringCollection;
                tbxEndLocation.AutoCompleteCustomSource = locationsAutoCompleteStringCollection;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        AutoCompleteStringCollection locationsConvertToAutoCompleteStringCollection(IEnumerable<Location> locations)
        {
            AutoCompleteStringCollection result = new AutoCompleteStringCollection();

            result.AddRange(locations.Where(x => x != null && x.UserCode != null).Select(x => Encoding.GetEncoding("gb2312").GetString(Encoding.Default.GetBytes(x.UserCode))).ToArray());
            //foreach (var variable in locations)
            //{
            //    System.Diagnostics.Debug.WriteLine(variable.UserCode);
            //    //将中文转码
            //    byte[] bytes = Encoding.Default.GetBytes(variable.UserCode);
            //    string s = Encoding.GetEncoding("gb2312").GetString(bytes);
            //    result.Add(s);
            //}

            return result;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Location start, end;
                if (Wcs.Framework.Cfg.WcsConfiguration.Instance.LocationCollection.Locations.Any(x => x.UserCode == tbxStartLocation.Text.Trim() && !(x is ILocationWildcard && !((ILocationWildcard)x).AbleAsOnlyLocation)))
                    start = LocationConverter.UserCodeToLcation(tbxStartLocation.Text.Trim());
                else
                    start = null;
                if (start == null)
                {
                    MessageBox.Show("起点位置不存在，请确认!");
                    return;
                }
                if (Wcs.Framework.Cfg.WcsConfiguration.Instance.LocationCollection.Locations.Any(x => x.UserCode == tbxEndLocation.Text.Trim() && !(x is ILocationWildcard && !((ILocationWildcard)x).AbleAsOnlyLocation)))
                    end = LocationConverter.UserCodeToLcation(tbxEndLocation.Text);
                else
                    end = null;
                if (end == null)
                {
                    MessageBox.Show("终点位置不存在，请确认!");
                    return;
                }
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                var result = Wcs.Framework.RouteHelper.AbleArrived(start, end);
                sw.Stop();
                label4.Text = DateTime.Now.ToString("HH:MM:ss.ffff") + (result ? " 联通" : " 不联通") + " 计算耗时:" + sw.ElapsedMilliseconds + "milliseconds";
            }
            catch (Exception ex)
            {
                label4.Text = DateTime.Now.ToString("HH:MM:ss.ffff") + "发生异常:" + ex.Message;
            }
        }
        Random _random = new Random();
        private void button3_Click(object sender, EventArgs e)
        {
            var time = _random.Next(0, Int32.MaxValue);
            Console.WriteLine(DateTime.Now.ToString("HH:MM:ss.ffff") + time + "Start");
            try
            {
                Location start, end;
                if (Wcs.Framework.Cfg.WcsConfiguration.Instance.LocationCollection.Locations.Any(x => x.UserCode == tbxStartLocation.Text.Trim() && !(x is ILocationWildcard && !((ILocationWildcard)x).AbleAsOnlyLocation)))
                    start = LocationConverter.UserCodeToLcation(tbxStartLocation.Text.Trim());
                else
                    start = null;
                if (start == null)
                {
                    MessageBox.Show("起点位置不存在，请确认!");
                    return;
                }
                if (Wcs.Framework.Cfg.WcsConfiguration.Instance.LocationCollection.Locations.Any(x => x.UserCode == tbxEndLocation.Text.Trim() && !(x is ILocationWildcard && !((ILocationWildcard)x).AbleAsOnlyLocation)))
                    end = LocationConverter.UserCodeToLcation(tbxEndLocation.Text);
                else
                    end = null;
                if (end == null)
                {
                    MessageBox.Show("终点位置不存在，请确认!");
                    return;
                }

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                var result = Wcs.Framework.RouteHelper.GetAllRouteIdSequences(start, end);
                sw.Stop();
                int RouteNO = 1;
                textBox3.Text = "";
                foreach (var item in result)
                {
                    if (String.IsNullOrWhiteSpace(textBox3.Text))
                        textBox3.Text = RouteNO.ToString() + ":" + string.Join(",", item) + "(详细信息：" + String.Join(",", RouteHelper.GetOneRouteLocationSequences(start, end, item)) + ")";
                    else
                        textBox3.Text += "\r\n" + RouteNO.ToString() + ":" + String.Join(",", item) + "(详细信息：" + String.Join(",", RouteHelper.GetOneRouteLocationSequences(start, end, item)) + ")";
                    RouteNO++;
                }
                label4.Text = DateTime.Now.ToString("HH:MM:ss.ffff") + " 可达路径总数：" + result.Count() + " 计算耗时:" + sw.ElapsedMilliseconds + "milliseconds";
            }
            catch (Exception ex)
            {
                label4.Text = DateTime.Now.ToString("HH:MM:ss.ffff") + "发生异常:" + ex.Message;
                MessageBox.Show("发生异常:" + ex.Message);
            }
            Console.WriteLine(DateTime.Now.ToString("HH:MM:ss.ffff") + time + "End");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Wcs.Framework.RouteHelper.Introduction();
                MessageBox.Show("导入成功");
            }
            catch (Exception ex)
            {
                button2_Click(this, null);
                MessageBox.Show("导入失败，异常消息：" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
            {
                var hql = "from DeviceErrorType;";
                unitOfWork.session.Delete(hql);
                hql = "from RouteDetail;";
                unitOfWork.session.Delete(hql);
                hql = "from RouteRelation;";
                unitOfWork.session.Delete(hql);
                unitOfWork.Commit();
            }
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
            {
                var hql = "from RouteHead;";
                unitOfWork.session.Delete(hql);
                unitOfWork.Commit();
            }
        }

        const string _path = "系统配置\\基本配置";
        const string _basicDataExcelName = "基础数据";
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string path = System.Windows.Forms.Application.StartupPath + "\\" + _path;
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                var file = directoryInfo.GetFiles().FirstOrDefault(x => x.FullName.Contains(_basicDataExcelName));
                if (file != null)
                    System.Diagnostics.Process.Start(file.FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开失败");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Wcs.Framework.RouteHelper.Introduction2();
                MessageBox.Show("导入成功");
            }
            catch (Exception ex)
            {
                button2_Click(this, null);
                MessageBox.Show("导入失败，异常消息：" + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                RouteHelper.ExportToExcel();
                MessageBox.Show("导出成功");
            }
            catch (Exception ex)
            {
                button2_Click(this, null);
                MessageBox.Show("导出失败，异常消息：" + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                var path = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<string>("FrmCraneInOutPortSetPath", "");
                if (string.IsNullOrWhiteSpace(path))
                {
                    MessageBox.Show("未配置设置窗口路径");
                    return;
                }

                var type = Type.GetType(path);
                if (type == null)
                {
                    MessageBox.Show(string.Format("未找到 {0} 节点 {1} 属性值 “{2}” 标识的类型", path, "type", path));
                    return;
                }

                if (type.IsAssignableFrom(typeof(Form)))
                {
                    MessageBox.Show(string.Format("{0} 节点 {1} 属性值 “{2}” 不是一个Form", path, "type", path));
                    return;
                }

                var frm = ReflectionHelper.CreateInstance<Form>(type);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开堆垛机出入口输送机方向设置时发生异常，异常消息：\r\n{ex}");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                var path = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<string>("FrmInOutPortSetPath", "");
                if (string.IsNullOrWhiteSpace(path))
                {
                    MessageBox.Show("未配置设置窗口路径");
                    return;
                }

                var type = Type.GetType(path);
                if (type == null)
                {
                    MessageBox.Show(string.Format("未找到 {0} 节点 {1} 属性值 “{2}” 标识的类型", path, "type", path));
                    return;
                }

                if (type.IsAssignableFrom(typeof(Form)))
                {
                    MessageBox.Show(string.Format("{0} 节点 {1} 属性值 “{2}” 不是一个Form", path, "type", path));
                    return;
                }

                var frm = ReflectionHelper.CreateInstance<Form>(type);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开Port口业务设置时发生异常，异常消息：\r\n{ex}");
            }
        }

        private void frmRouteTest_Load(object sender, EventArgs e)
        {
            loadButton();
        }

        Dictionary<string, Type> dic = new Dictionary<string, Type>();
        private void loadButton()
        {
            var buttons = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<string>("frmRouteTestExtendButtons", "");
            if (string.IsNullOrWhiteSpace(buttons))
                return;
            var _buttons = buttons.Replace("\r\n", "").Split(';').ToArray();
            foreach (var item in _buttons.Reverse())
            {
                var items = item.Split('|').Select(x=>x.Trim()).ToArray();
                if (items.Count() != 2)
                    continue;
                Type type;
                try
                {
                    type = Type.GetType(items[1]);

                    if (type.IsAssignableFrom(typeof(Form)))
                    {
                        MessageBox.Show(string.Format("settings.xml 中 frmRouteTestExtendButtons 配置的扩展按钮 {0} 对应的窗口 {1} 不是一个Form，本次不加载此按钮", items[0], items[1]));
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("未找到 settings.xml 中 frmRouteTestExtendButtons 配置的扩展按钮 {0} 对应的窗口 {1}，本次不加载未找到窗体的按钮", items[0], items[1]));
                    _logger.Error1(ex, this);
                    continue;
                }
                if (dic.ContainsKey(items[0]))
                {
                    MessageBox.Show($"settings.xml 中 frmRouteTestExtendButtons 配置的扩展按钮名 {items[0]}重复，本次不加载重复按钮名");
                    continue;
                }

                dic.Add(items[0], type);
                Button button = new Button();
                button.Text = items[0];
                //button.Size = new Size(93, panel1.Size.Height - 4);
                panel1.Controls.Add(button);
                button.Dock = DockStyle.Left;
                button.Click += ExpendButton_Click;
            }
        }

        private void ExpendButton_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                if (MessageBox.Show($"即将打开{btn.Text}，请确认！\r\n非专业人士请勿操作，否则可能造成系统异常！！！\r\n非专业人士请勿操作，否则可能造成系统异常！！！\r\n非专业人士请勿操作，否则可能造成系统异常！！！", "Tips", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    MessageBox.Show("用户取消了操作");
                    return;
                }
                _logger.Info1($"打开{btn.Text}", this);
                var frm = ReflectionHelper.CreateInstance<Form>(dic[btn.Text]);
                frm.ShowDialog();
                _logger.Info1("设置完成", this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开Port口业务设置时发生异常，异常消息：\r\n{ex}");
            }
        }
    }
}
