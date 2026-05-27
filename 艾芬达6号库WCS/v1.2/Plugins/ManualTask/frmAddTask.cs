using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework;
using Wcs.Framework.Cfg;
using Wcs.FrameworkExtend;

namespace Wcs.App.Plugins.ManualTask
{
    public partial class frmAddTask : Form
    {
        Logger _logger;
        public frmAddTask()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();

            //linkLabel1.Visible =
            //    linkLabel1.Enabled = Wcs.Security.WcsPermission.IsAdministratorMode;
            try
            {
                locations = WcsConfiguration
                .Instance
                .LocationCollection
                .Locations
                .Where(x => !(x is Wcs.Framework.ILocationWildcard));
                AutoCompleteStringCollection locationsAutoCompleteStringCollection = locationsConvertToAutoCompleteStringCollection(locations);
                tbxStartLocation.AutoCompleteCustomSource = locationsAutoCompleteStringCollection;
                tbxEndLocation.AutoCompleteCustomSource = locationsAutoCompleteStringCollection;

                cbxBizType.DisplayMember = "Value";
                cbxBizType.ValueMember = "Key";
                cbxBizType.DataSource = Wcs.EnumExtentions.ToKeyValueList<TaskBizType>();

                cbxSource.DisplayMember = "Value";
                cbxSource.ValueMember = "Key";
                cbxSource.DataSource = Wcs.EnumExtentions.ToKeyValueList<TaskSource>();

                cbxGiveTaskCode_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);

                throw ex;
            }
        }

        IEnumerable<Location> locations;

        AutoCompleteStringCollection locationsConvertToAutoCompleteStringCollection(IEnumerable<Location> locations)
        {
            AutoCompleteStringCollection result = new AutoCompleteStringCollection();

            result.AddRange(locations.Where(x => x != null && x.UserCode != null).Select(x => x.UserCode).ToArray());

            return result;
        }

        private void cbxGiveTaskCode_CheckedChanged(object sender, EventArgs e)
        {
            tbxTaskCode.Enabled = cbxGiveTaskCode.Checked;

            if (cbxGiveTaskCode.Checked)
            {
                tbxTaskCode.Text = "";
            }
            else
            {
                tbxTaskCode.Text = "<自动生成>";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (radio_running.Checked)
                {
                    Location startLocation = LocationConverter.UserCodeToLcation(tbxStartLocation.Text);
                    if (startLocation == null)
                    {
                        MessageBox.Show("请输入有效的任务起始位置");
                        tbxStartLocation.Focus();
                        return;
                    }

                    Location endLocation = LocationConverter.UserCodeToLcation(tbxEndLocation.Text);
                    if (endLocation == null)
                    {
                        MessageBox.Show("请输入有效的任务结束位置");
                        tbxEndLocation.Focus();
                        return;
                    }
                    TaskBizType bizType = (TaskBizType)Enum.Parse(typeof(TaskBizType), Convert.ToString(cbxBizType.SelectedValue));
                    TaskSource source = (TaskSource)Enum.Parse(typeof(TaskSource), Convert.ToString(cbxSource.SelectedValue));

                    var ableArrived = RouteHelper.AbleArrived(startLocation, endLocation);

                    if (!ableArrived)
                    {
                        MessageBox.Show(String.Format("{0} 到 {1} 无法连通", startLocation, endLocation), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (cbxGiveTaskCode.Checked && string.IsNullOrWhiteSpace(tbxTaskCode.Text))
                    {
                        MessageBox.Show("请输入有效的任务号");
                        tbxTaskCode.Focus();
                        return;
                    }

                    List<String> containerCodes = new List<string>();
                    if (!string.IsNullOrWhiteSpace(tbxContainerCodes.Text))
                    {
                        foreach (var item in System.Text.RegularExpressions.Regex.Split(tbxContainerCodes.Text, "\r\n"))
                        {
                            if (string.IsNullOrWhiteSpace(item)) continue;
                            containerCodes.Add(item);
                        }
                    }
                    Task newTask;

                    var taskCode = tbxTaskCode.Text;
                    if (!cbxGiveTaskCode.Checked)
                    {
                        taskCode = SerialNumberFactory.GenerateManualTaskCode();
                    }

                    newTask = new Task(taskCode, LocationConverter.ToLocationInfo(startLocation), LocationConverter.ToLocationInfo(endLocation));
                    newTask.BizType = bizType;
                    newTask.Source = source;
                    newTask.TaskType = cbxTaskType.Text.Trim();
                    newTask.Description = tbxDescription.Text.Trim();

                    var additionalInfo = tbxAdditionalInfo.Text.Trim()
                        .Replace("\\,", "●●☆");
                    foreach (var item in additionalInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var arr = item.Split('=');
                        if (arr.Length != 2)
                        {
                            continue;
                        }

                        if (String.IsNullOrWhiteSpace(arr[0]))
                        {
                            continue;
                        }

                        newTask.AdditionalInfo.Add(arr[0].Trim().Replace("●●☆", ","), arr[1].Trim().Replace("●●☆", ","));
                    }

                    if (Wcs.Security.WcsPermission.IsAdministratorMode)
                    {
                        if (_links.Count > 0)
                        {
                            newTask.AdditionalInfo.Add("_连续任务关键点集合", string.Join(",", _links));
                        }
                    }

                    if (checkBox1.Checked)
                    {
                        newTask.AdditionalInfo.Add("_禁止堆垛机取货", "true");
                    }

                    if (cbx_unPreTaskFlag.Checked)
                    {
                        newTask.AdditionalInfo.Add("UnPreTask", "true");
                    }


                    newTask.ContainerCodes.AddAll(containerCodes);

                    using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                    {

                        unitOfWork.session.Save(newTask);

                        unitOfWork.Commit();
                    }

                    Wcs.Framework.EventBus.EventBus.Instance.Publish(new Wcs.Framework.Events.TaskAddedEvent(newTask));

                    _logger.Info1(string.Format("添加了一个执行任务池手工任务 {0}", newTask), this, newTask);

                    tbxContainerCodes.Text = "";
                    if (cbxGiveTaskCode.Checked)
                    {
                        tbxTaskCode.Text = "";
                    }

                    MessageBox.Show(String.Format("{0} 添加成功", newTask), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    Location startLocation = LocationConverter.UserCodeToLcation(tbxStartLocation.Text);
                    if (startLocation == null)
                    {
                        MessageBox.Show("请输入有效的任务起始位置");
                        tbxStartLocation.Focus();
                        return;
                    }

                    Location endLocation = LocationConverter.UserCodeToLcation(tbxEndLocation.Text);
                    if (endLocation == null)
                    {
                        MessageBox.Show("请输入有效的任务结束位置");
                        tbxEndLocation.Focus();
                        return;
                    }
                    TaskBizType bizType = (TaskBizType)Enum.Parse(typeof(TaskBizType), Convert.ToString(cbxBizType.SelectedValue));
                    TaskSource source = (TaskSource)Enum.Parse(typeof(TaskSource), Convert.ToString(cbxSource.SelectedValue));

                    var ableArrived = RouteHelper.AbleArrived(startLocation, endLocation);

                    if (!ableArrived)
                    {
                        MessageBox.Show(String.Format("{0} 到 {1} 无法连通", startLocation, endLocation), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (cbxGiveTaskCode.Checked && string.IsNullOrWhiteSpace(tbxTaskCode.Text))
                    {
                        MessageBox.Show("请输入有效的任务号");
                        tbxTaskCode.Focus();
                        return;
                    }

                    List<String> containerCodes = new List<string>();
                    if (!string.IsNullOrWhiteSpace(tbxContainerCodes.Text))
                    {
                        foreach (var item in System.Text.RegularExpressions.Regex.Split(tbxContainerCodes.Text, "\r\n"))
                        {
                            if (string.IsNullOrWhiteSpace(item)) continue;
                            containerCodes.Add(item);
                        }
                    }

                    var taskCode = tbxTaskCode.Text;
                    if (!cbxGiveTaskCode.Checked)
                        taskCode = SerialNumberFactory.GenerateManualTaskCode();

                    PreTask newPreTask = new PreTask(taskCode, LocationConverter.ToLocationInfo(startLocation), LocationConverter.ToLocationInfo(endLocation));
                    newPreTask.BizType = bizType;
                    newPreTask.Source = source;
                    newPreTask.TaskType = cbxTaskType.Text.Trim();
                    newPreTask.Description = tbxDescription.Text.Trim();

                    Dictionary<string, string> additionalInfoDic = new Dictionary<string, string>();
                    var additionalInfo = tbxAdditionalInfo.Text.Trim()
                        .Replace("\\,", "●●☆");
                    foreach (var item in additionalInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var arr = item.Split('=');
                        if (arr.Length != 2)
                        {
                            continue;
                        }

                        if (String.IsNullOrWhiteSpace(arr[0]))
                        {
                            continue;
                        }

                        additionalInfoDic.Add(arr[0].Trim().Replace("●●☆", ","), arr[1].Trim().Replace("●●☆", ","));
                    }

                    if (checkBox1.Checked)
                    {
                        additionalInfoDic.Add("_禁止堆垛机取货", "true");
                    }

                    newPreTask.AdditionalInfo = JsonConvert.SerializeObject(additionalInfoDic);
                    newPreTask.ContainerCodes = JsonConvert.SerializeObject(containerCodes);
                    using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                    {

                        unitOfWork.session.Save(newPreTask);

                        unitOfWork.Commit();
                    }

                    //Wcs.Framework.EventBus.EventBus.Instance.Publish(new Wcs.Framework.Events.TaskAddedEvent(newPreTask));
                    Wcs.Framework.EventBus.EventBus.Instance.Publish<Wcs.FrameworkExtend.Events.PreTaskAddedEvent>(new Wcs.FrameworkExtend.Events.PreTaskAddedEvent(newPreTask));

                    _logger.Info1(string.Format("添加了一个手工任务 {0}", newPreTask), this, newPreTask);

                    tbxContainerCodes.Text = "";
                    if (cbxGiveTaskCode.Checked)
                    {
                        tbxTaskCode.Text = "";
                    }

                    MessageBox.Show(String.Format("{0} 添加成功", newPreTask), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);

                MessageBox.Show(string.Format("手工任务添加失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnCacnel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmAddTask_Load(object sender, EventArgs e)
        {
            if (Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection._settings.ContainsKey("ManualTaskTasktypes"))
                cbxTaskType.DataSource = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection._settings["ManualTaskTasktypes"].Split(',').Distinct().ToArray();

            //cbxStartLocation.Items.AddRange(locations.Where(x => x != null && x.UserCode != null).Select(x => x.UserCode).ToArray());
        }

        private List<string> _links = new List<string>();
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new frmNextLinks(tbxEndLocation.Text, _links, tbxStartLocation.AutoCompleteCustomSource))
            {
                frm.ShowDialog(this);
                _links.Clear();
                _links.AddRange(frm.Links);
            }
        }

        private void radio_running_CheckedChanged(object sender, EventArgs e)
        {
            if (!radio_running.Checked)
            {
                cbx_unPreTaskFlag.Visible = false;
                return;
            }

            if (MessageBox.Show("本次选择为手动下发 执行任务池 任务，执行任务池任务将在设备状态允许时直接执行，不会被计划任务过滤器过滤，此为特殊情况下恢复现场使用，非特殊情况请选择手动下发 计划任务池 任务，请确认是否选择手动下发执行任务池任务！", "tips:操作提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cbx_unPreTaskFlag.Visible = true;
                return;
            }
            else
            {
                radio_plan.Checked = true;
                radio_running.Checked = false;
                cbx_unPreTaskFlag.Visible = false;
            }
        }

        //[DllImport("user32.dll", EntryPoint = "SendMessage")]
        //private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        //private void cbxStartLocation_TextUpdate(object sender, EventArgs e)
        //{
        //    //清空combobox
        //    this.cbxStartLocation.Items.Clear();
        //    //自动弹出下拉框
        //    this.cbxStartLocation.DroppedDown = true;
        //    //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
        //    Cursor = Cursors.Default;
        //    //SendMessage(WM_SETCURSOR, 0, 0);

        //    List<string> list = new List<string>();
        //    var str = cbxStartLocation.Text;
        //    if (!string.IsNullOrWhiteSpace(str))
        //    {
        //        var _locations = locations.Where(x => x.UserCode.IndexOf(str) >= 0);
        //        if (_locations.Count() > 0)
        //            list.AddRange(_locations.Select(x => x.UserCode).ToArray());
        //    }
        //    else
        //        list.AddRange(locations.Select(x => x.UserCode).ToArray());

        //    cbxStartLocation.Items.AddRange(list.ToArray());
        //    ////防止默认选择第一项
        //    //cbxStartLocation.SelectedIndex = -1;
        //    //cbxStartLocation.Text = str;
        //    //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列
        //    this.cbxStartLocation.SelectionStart = str.Length;
        //}
    }
}
