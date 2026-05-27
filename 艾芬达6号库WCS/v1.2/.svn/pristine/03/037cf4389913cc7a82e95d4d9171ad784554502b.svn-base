using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs;
using Wcs.Framework;
using NHibernate.Linq;
using System.Threading;

namespace WMSServices
{
    /// <summary>
    /// 任务完成处理程序
    /// </summary>
    public class WmsTaskCompeletedReportStartUp : Wcs.Framework.IApplicationStartup
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        static Thread _thread;

        public void Initialize(Wcs.Framework.Cfg.StartupElement element)
        {
        }

        public void Run(Wcs.IWcsApplication application)
        {
            _thread = new System.Threading.Thread(ReportWmsCompeleted);
            _thread.Name = "ReportWmsCompeleted";
            _thread.IsBackground = true;
            _thread.StartAndManaged();
            _logger.Debug1("ReportWmsCompeleted线程已启动", this);

            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<Wcs.Framework.Events.TaskFinishedEvent>(onTaskFinished);

            Task[] tasks;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                tasks = unitOfWork.session.Query<Task>().Where(x => x.Source == TaskSource.Wms && (x.Status == TaskStatus.Cancelled || x.Status == TaskStatus.Completed)).ToArray();
                unitOfWork.Commit();
            }
            foreach (var task in tasks)
            {
                PushWaitReportTaskList(task);
            }
        }

        void onTaskFinished(Wcs.Framework.Events.TaskFinishedEvent args)
        {
            _logger.Trace1(string.Format("收到一个 {0} 状态变为 {1} 的{2}任务的事件...", args.TaskCode, args.Status, args.TaskType), this, args.TaskCode);
            if (args.Source != Wcs.Framework.TaskSource.Wms)
                return;

            Task _task;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                _task = unitOfWork.session.Get<Task>(args.Id);
                unitOfWork.Commit();
            }
            if (_task == null)
                return;
            else
                PushWaitReportTaskList(_task);
        }

        /// <summary>
        /// 任务待归档列表
        /// </summary>
        public static List<Task> _waitReportTasks = new List<Task>();

        /// <summary>
        /// 是否已经存在于归档列表中
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static Boolean ExistWaitReportProcTaskList(Task task)
        {
            return _waitReportTasks.Any(x => x.TaskCode == task.TaskCode);
        }

        /// <summary>
        /// 将任务添加到归档列表
        /// </summary>
        /// <param name="task"></param>
        public static void PushWaitReportTaskList(Task task)
        {

            lock (_waitReportTasks)
            {
                if (_waitReportTasks.Any(x => x.TaskCode == task.TaskCode))
                {
                    _logger.Warn1(string.Format("{0}已存在于上报队列当中", task), typeof(WmsTaskCompeletedReportStartUp), task);
                    return;
                }

                _waitReportTasks.Add(task);

                _logger.Debug1(string.Format("{0}被加入上报队列", task), typeof(WmsTaskCompeletedReportStartUp), task);
            }
        }

        /// <summary>
        /// 将任务从归档列表中移除
        /// </summary>
        /// <param name="task"></param>
        public static void PopWaitReportTaskList(Task task)
        {
            if (task == null)
                return;

            lock (_waitReportTasks)
            {
                var item = _waitReportTasks.FirstOrDefault(x => x.TaskCode == task.TaskCode);
                if (item == null)
                {
                    _logger.Warn1(string.Format("{0}已被从上报队列移除", item), typeof(WmsTaskCompeletedReportStartUp), item);
                    return;
                }

                _waitReportTasks.Remove(item);

                _logger.Debug1(string.Format("{0}被移上报队列", item), typeof(WmsTaskCompeletedReportStartUp), item);
            }
        }

        private void ReportWmsCompeleted(object obj)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    Task[] tasksCloned;
                    lock (_waitReportTasks)
                    {
                        tasksCloned = _waitReportTasks.ToArray();
                    }

                    foreach (var _tsk in tasksCloned)
                    {

                        Task _task;

                        using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                        {
                            _task = unitOfWork.session.Get<Task>(_tsk.Id);
                            unitOfWork.Commit();
                        }

                        if (_task == null)
                        {
                            _logger.Trace1(string.Format("未找到任务{0}", _tsk.TaskCode), this, _tsk.TaskCode);
                            if (_tsk != null)
                                PopWaitReportTaskList(_tsk);
                            continue;
                        }

                        if (_task.Status != TaskStatus.Completed && _task.Status != TaskStatus.Cancelled)
                        {
                            _logger.Warn1(string.Format("任务{0}当前状态应为“{1}”或“{2}”，实际为“{3}”（这可能是因为在处理完成时上下文事务中的某一个操作失败造成的事务回滚引起的“脏事件”问题，你可以忽略该警告）。"
                                , _tsk.TaskCode
                                , TaskStatus.Completed.GetDescription()
                                , TaskStatus.Cancelled.GetDescription()
                                , _task.Status.GetDescription()
                                ), this, _tsk.TaskCode);
                            continue;
                        }

                        try
                        {
                            foreach (var item in Wcs.Framework.Cfg.WcsConfiguration.Instance.WMSTaskCompeletedExternalHandlersElement.WMSCompeletedExternalHandlers)
                            {
                                if (item.Allowed(_task))
                                    item.Hand(ref _task);
                            }

                            _logger.Trace1("准备通知Wms...", this, _task);
                            WmsServicesHelper.CompleteWmsTask(_task);
                        }
                        catch (Exception e)
                        {
                            _logger.Error1(e, typeof(WmsTaskCompeletedReportStartUp));
                            continue;
                        }

                        _logger.Trace1("通知Wms成功", this, _task);

                        PopWaitReportTaskList(_tsk);
                        Wcs.DefaultImpls.Business.BusinessHelper.PushDeleteTaskList(_tsk);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, typeof(WmsTaskCompeletedReportStartUp));
                }
            }
        }
    }
}
