using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework;
using Wcs.Framework.Events;
using NHibernate.Linq;
using Wcs.Framework.EventBus;
using System.IO;
using Wcs.DefaultImplementCollection.Crane;

namespace Wcs.App.Plugins.CraneTest
{
    public partial class frmMain : Form
    {
        Logger _logger;
        BindingSource _bindingSource;
        public frmMain()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();

            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<Location>();
            dgvGrid.DataSource = _bindingSource;

            checkedListBox1.Items.Clear();
            var craneElements = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x is Wcs.DefaultImplementCollection.Crane.CraneElement)
                .Select(x => x as Wcs.DefaultImplementCollection.Crane.CraneElement);
            foreach (var item in craneElements)
            {
                checkedListBox1.Items.Add(item.Device);
            }

            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskStatusChangedEvent>(onTaskStatusChanged);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error1(ex, this);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskStatusChangedEvent>(onTaskStatusChanged);
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

        private void dgvGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var loc = (RackLocation)dgvGrid.Rows[e.RowIndex].DataBoundItem;
            if (e.ColumnIndex == colDeviceName.Index)
            {
                e.Value = loc.Device.Name;
            }
            else if (e.ColumnIndex == colLevel.Index)
            {
                e.Value = loc.Level.ToString();
            }
            else if (e.ColumnIndex == colColumn.Index)
            {
                e.Value = loc.Column.ToString();
            }
            else if (e.ColumnIndex == colUserCode.Index)
            {
                e.Value = loc.UserCode;
            }
        }

        void load()
        {
            var craneElements = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x is Wcs.DefaultImplementCollection.Crane.CraneElement)
                .Select(x => x as Wcs.DefaultImplementCollection.Crane.CraneElement);
            foreach (var item in craneElements)
            {
                var locations = LoadLocations(item.Device as CraneDevice);
                foreach (var loc in locations)
                {
                    _bindingSource.Add(loc);
                }
            }
        }

        Random rnd = new Random();
        void onTaskStatusChanged(TaskStatusChangedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<TaskStatusChangedEvent> act = (_args) =>
                {
                    onTaskStatusChanged(args);
                };

                this.Invoke(act, args);
            }
            else
            {
                if (args.Status == Framework.TaskStatus.Completed && args.TaskType == "CraneTest")
                {
                    if (cbx行走.Checked == false && cbx取放.Checked == false && cbx升降.Checked == false)
                    {
                        MessageBox.Show("未勾选 行走/去放/升降，无法继续生成任务");
                        return;
                    }

                    Task task;
                    using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        task = unitOfWork.session.Get<Task>(args.Id);

                        if (task != null && cbx自动归档完成的测试任务.Checked)
                        {
                            unitOfWork.session.Delete(task);
                        }

                        unitOfWork.Commit();
                    }

                    if (task == null)
                    {
                        return;
                    }

                    if (cbx自动归档完成的测试任务.Checked)
                    {
                        EventBus.Instance.Publish<TaskArchivedEvent>(new TaskArchivedEvent(task.Id, task.TaskCode));
                    }

                    var endLocation = LocationConverter.ToLocation(task.EndLocation);
                    if (!(endLocation.Device is CraneDevice))
                    {
                        return;
                    }

                    var crane = endLocation.Device as CraneDevice;

                    if (!checkedListBox1.CheckedItems.Cast<CraneDevice>().Any(x => x == crane))
                    {
                        return;
                    }

                    var location = LocationConverter.ToLocation(task.EndLocation);
                    if (!(location is RackLocation currentLocation))
                        return;
                    //if (crane.LastStatus == null)
                    //{
                    //    return;
                    //}

                    ////堆垛机当前停靠位置
                    //RackLocation currentLocation = crane.Locations.Select(x => (RackLocation)x).FirstOrDefault(x => x.Level == crane.LastStatus.YLevel && x.Column == crane.LastStatus.XColumn);
                    //if (currentLocation == null)
                    //{
                    //    MessageBox.Show(string.Format("{0} 当前位置(设备位置) {1} 列 {2} 层不可识别，请回原点后重新开始测试触发", crane, crane.LastStatus.XColumn, crane.LastStatus.YLevel), "堆垛机测试", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                    var testedLocation = LoadLocations(crane);
                    List<RackLocation> 可用的终点位置 = crane.Locations.Select(x => (RackLocation)x)
                        .Where(x => !(x is ILocationWildcard) && !(x is CraneDeviceOriginPointLocation))
                        .Where(x => x.Enabled == true && x.ForkAction == ForkAction.PickAndPut)
                        .ToList();

                    if (cbx忽略已测试过的位置.Checked)
                    {
                        可用的终点位置 = 可用的终点位置
                            .Except(testedLocation)
                            .ToList();
                    }

                    if (cbx取放.Checked)
                    {
                        可用的终点位置 = 可用的终点位置.Where(x =>
                                x.ForkDirection != null
                                && x.IsForkActionAllowed(ForkAction.Pickup)
                                && x.IsForkActionAllowed(ForkAction.Putdown)
                            ).ToList();
                    }

                    if (cbx行走.Checked == false) //如果不允许移动列位置
                    {
                        可用的终点位置 = 可用的终点位置.Where(x => x.Column == currentLocation.Column).ToList();
                    }

                    if (cbx升降.Checked == false) //如果不允许移动层位置
                    {
                        可用的终点位置 = 可用的终点位置.Where(x => x.Level == currentLocation.Level).ToList();
                    }

                    var startLocation = endLocation;

                    if (cbx取放.Checked == false)
                    {
                        startLocation = currentLocation;
                    }
                    RackLocation newEndLocation;
                    if (cbx随机.Checked == false)
                    {
                        newEndLocation = 可用的终点位置.FirstOrDefault();
                        if (cbx行走.Checked || cbx升降.Checked)
                        {
                            int index = 可用的终点位置.IndexOf(newEndLocation);
                            //防止起点位置和终点位置一样导致的任务不执行
                            while (newEndLocation.Column == ((RackLocation)endLocation).Column
                                && newEndLocation.Level == ((RackLocation)endLocation).Level)
                            {
                                index++;
                                if (index >= 可用的终点位置.Count)
                                {
                                    _logger.Info1(string.Format("{0} 所有货位已测试结束", crane), this);
                                    return;
                                }
                                newEndLocation = 可用的终点位置[index];
                            }
                        }
                    }
                    else
                    {
                        int index = rnd.Next(0, 可用的终点位置.Count);
                        newEndLocation = 可用的终点位置[index];

                        if (cbx行走.Checked || cbx升降.Checked)
                        {
                            //防止起点位置和终点位置一样导致的任务不执行
                            while (newEndLocation.Column == ((RackLocation)endLocation).Column
                                && newEndLocation.Level == ((RackLocation)endLocation).Level)
                            {
                                index = rnd.Next(0, 可用的终点位置.Count);
                                newEndLocation = 可用的终点位置[index];
                            }
                        }
                    }

                    if (newEndLocation == null)
                    {
                        _logger.Info1(string.Format("{0} 所有货位已测试结束", crane), this);
                        return;
                    }

                    Task newTask;
                    var taskCode = SerialNumberFactory.GenerateManualTaskCode();

                    newTask = new Task(taskCode, LocationConverter.ToLocationInfo(startLocation), LocationConverter.ToLocationInfo(newEndLocation));
                    newTask.BizType = TaskBizType.Normal;
                    newTask.Source = TaskSource.Unknow;
                    newTask.TaskType = "CraneTest";

                    if (cbx取放.Checked == false) //如果不允许取放货（货叉活动）
                    {
                        newTask.AdditionalInfo.Add("_禁止堆垛机取货", "true");
                    }
                    newTask.AdditionalInfo.Add("UnPreTask", "true");

                    using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                    {
                        unitOfWork.session.Save(newTask);

                        unitOfWork.Commit();
                    }

                    Wcs.Framework.EventBus.EventBus.Instance.Publish(new Wcs.Framework.Events.TaskAddedEvent(newTask));

                    _logger.Info1(string.Format("堆垛机测试程序添加了一个新任务 {0}", newTask), this, newTask);

                    AddLocation(newEndLocation);

                    _bindingSource.Add(newEndLocation);
                }
            }
        }

        void AddLocation(Location location)
        {
            var filePath = Path.Combine(Application.StartupPath, string.Format("{0}_已测试货位.txt", location.Device.Name));
            File.AppendAllLines(filePath, new string[] { location.UserCode });
        }

        List<RackLocation> LoadLocations(CraneDevice device)
        {
            var filePath = Path.Combine(Application.StartupPath, string.Format("{0}_已测试货位.txt", device.Name));
            if (!File.Exists(filePath))
            {
                return new List<RackLocation>();
            }

            var contents = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x));
            var q = from o in contents
                    join x in device.Locations.Select(x => (RackLocation)x) on o equals x.UserCode
                    select x;

            return q.ToList();
        }

    }
}
