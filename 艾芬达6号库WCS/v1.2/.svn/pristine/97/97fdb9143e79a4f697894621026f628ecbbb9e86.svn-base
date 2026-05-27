using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;
using Wcs.Framework.EventBus;
using Wcs.Framework.Events;

namespace Wcs.App.Plugins.TaskManager
{
    [WcsPluginInfo(typeof(TaskManagerPlugin), "任务管理器", "aben", "2013年7月", "提供一个可视化界面用于管理任务，包括常用的删除、暂停、继续执行等操作。同时界面内还提供了一个跟踪任务状态变化日志显示的功能", true)]
    public class TaskManagerPlugin: Wcs.WcsPlugin
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        const string 自动归档非Wms任务_key = "自动归档Wcs任务";
        public override int Priority
        {
            get { return 1; }
        }


        public override bool Initialization(WcsContext context)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem("任务管理器");
            tsmi.Click += tsmi_Click;
            context.Application.GetMenu(WcsApplicationMenuType.View).DropDownItems.Add(tsmi);

            if (Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<Boolean>("隐藏自动归档Wcs任务菜单", false) == false)
            {
                ToolStripMenuItem tsmiAutoArchiveOtherSourceTasks = new ToolStripMenuItem(自动归档非Wms任务_key);
                tsmiAutoArchiveOtherSourceTasks.Click += tsmiAutoArchiveOtherSourceTasks_Click;
                context.Application.GetMenu(WcsApplicationMenuType.Tools).DropDownItems.Add(tsmiAutoArchiveOtherSourceTasks);

                bool v = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>(自动归档非Wms任务_key);
                tsmiAutoArchiveOtherSourceTasks.Checked = v;

                if (v)
                {
                    Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskFinishedEvent>(onTaskFinished);
                }
            }

            return base.Initialization(context);
        }
                
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\自动归档Wcs任务参数设置")]
        void tsmiAutoArchiveOtherSourceTasks_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            var v = !item.Checked;
            item.Checked = v;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<bool>(自动归档非Wms任务_key, v);

            if (v)
            {
                Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskFinishedEvent>(onTaskFinished);
            }
            else
            {
                Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskFinishedEvent>(onTaskFinished);
            }
        }

        void onTaskFinished(TaskFinishedEvent args)
        {
            if (args.Source != TaskSource.Wcs)
            {
                return;
            }

            Task task;
            List<IEvent> events = new List<IEvent>();
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                task = unitOfWork.session.Get<Task>(args.Id,NHibernate.LockMode.None);
                

                if (task != null)
                {
                    if (Wcs.Security.WcsPermission.IsAdministratorMode && task.AdditionalInfo.ContainsKey("_连续任务关键点集合") &&
                        !string.IsNullOrWhiteSpace(task.AdditionalInfo["_连续任务关键点集合"]))
                    {
                        var links = task.AdditionalInfo["_连续任务关键点集合"].Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries);
                        var from = LocationConverter.ToLocation(task.EndLocation);
                        var to= LocationConverter.UserCodeToLcation(links.First());
                        var newTask = new Task(Wcs.Framework.SerialNumberFactory.GenerateManualTaskCode(),
                            new LocationInfo(from.Device.Name, from.DeviceCode, from.UserCode),
                            new LocationInfo(to.Device.Name, to.DeviceCode, to.UserCode)
                            )
                        {
                            BizType = task.BizType,
                            Description = task.Description,
                            Direction = task.Direction,
                            Priority = task.Priority,
                            Source = task.Source,
                            TaskType = task.TaskType
                        };
                        foreach (var pro in task.AdditionalInfo)
                        {
                            newTask.AdditionalInfo.Add(pro.Key, pro.Value);
                        }

                        var newLinks = links.Skip(1).ToArray();
                        if (newLinks.Length == 0)
                        {
                            newTask.AdditionalInfo.Remove("_连续任务关键点集合");
                        }
                        else
                        {
                            newTask.AdditionalInfo["_连续任务关键点集合"] = string.Join(",", newLinks);
                        }

                        foreach (var item in task.ContainerCodes)
                        {
                            newTask.ContainerCodes.Add(item);
                        }

                        unitOfWork.session.Save(newTask);

                        events.Add(new Wcs.Framework.Events.TaskAddedEvent(newTask));
                        _logger.Info1(string.Format("{0} 有后续关键点 {1}，自动生成 {2}，后续关键点更新为 {3}", task, string.Format(",",links), newTask, string.Format(",", newLinks)), this, task);

                    }

                    unitOfWork.session.Delete(task);
                    events.Add(new TaskArchivedEvent(task.Id, task.TaskCode));

                    _logger.Info1(string.Format("{0} 被 {1} 自动归档", task, this), this, task);
                }

                unitOfWork.Commit();
            }

            Wcs.Framework.EventBus.EventBus.Instance.Publish(events.ToArray());
        }

        frmMain frmMain;
        void tsmi_Click(object sender, EventArgs e)
        {
            if (frmMain != null && !frmMain.IsDisposed && !frmMain.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == frmMain)
                    {
                        frmMain.WindowState = FormWindowState.Maximized;
                        frmMain.Focus();
                        frmMain.Activate();
                        return;
                    }
                }
            }

            frmMain = new frmMain(this.Context);
            frmMain.MdiParent = this.Context.Application.MainForm;
            frmMain.Show();
            frmMain.WindowState = FormWindowState.Maximized;
            frmMain.Activate();
        }
    }
}
