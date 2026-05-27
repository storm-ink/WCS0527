using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NHibernate.Linq;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.Tools
{
    public partial class frmAutoArchiveTask : Form
    {
        public frmAutoArchiveTask()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            TaskSoursecheckedListBox.Items.Clear();
            TaskTypecheckedListBox.Items.Clear();

            String[] array = System.Enum.GetNames(typeof(TaskSource));
            foreach (var o in array)
            {
                TaskSoursecheckedListBox.Items.Add(o);
            }

            Task[] _tasks;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                _tasks = unitOfWork.session.Query<Task>().Where(x => x.TaskType != null).ToArray();

                unitOfWork.Commit();
            }

            if (!_tasks.Any())
            {
                MessageBox.Show("当前无任务可以归档");
            }
            foreach (var task in _tasks)
            {
                if (TaskTypecheckedListBox.Items.Contains(task.TaskType))
                {
                    continue;
                }

                TaskTypecheckedListBox.Items.Add(task.TaskType);
            }
        }

        private void Archive_Click(object sender, EventArgs e)
        {
            var taskSourseList = new ArrayList();
            for (var i = 0; i < TaskSoursecheckedListBox.Items.Count; i++)
            {
                if (TaskSoursecheckedListBox.GetItemChecked(i))
                {
                    taskSourseList.Add(TaskSoursecheckedListBox.Items[i]);
                }
            }
            var tasktypeList = new ArrayList();
            for (var i = 0; i < TaskTypecheckedListBox.Items.Count; i++)
            {
                if (TaskTypecheckedListBox.GetItemChecked(i))
                {
                    tasktypeList.Add(TaskTypecheckedListBox.Items[i]);
                }
            }

            List<Wcs.Framework.Events.TaskArchivedEvent> archivedEventList = new List<Framework.Events.TaskArchivedEvent>();
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {

                var deleteObject = unitOfWork
                    .session
                    .Query<Task>()
                    .Where(x => (x.Status == TaskStatus.Cancelled || x.Status == TaskStatus.Completed)
                    ).ToArray();

                var _tasks = deleteObject.Where(x => comparer(taskSourseList, x.Source) || tasktypeList.Contains(x.TaskType)).ToArray();

                if (_tasks.Any())
                {
                    foreach (var task in _tasks)
                    {
                        unitOfWork.session.Delete(task);
                        archivedEventList.Add(new Framework.Events.TaskArchivedEvent(task.Id, task.TaskCode));
                    }
                }
                unitOfWork.Commit();
            }
            Wcs.Framework.EventBus.EventBus.Instance.Publish(archivedEventList.ToArray());
        }

        private bool comparer(ArrayList array, TaskSource taskSource)
        {
            foreach (string o in array)
            {
                var o1 = (TaskSource)System.Enum.Parse(typeof(TaskSource), o);
                if (o1.Equals(taskSource))
                {
                    return true;
                }
            }
            return false;
        }

        private void pop()
        {
            while (true)
            {

            }
        }
    }
}
