using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Wcs;
using Wcs.Framework;
using Wcs.Framework.Exceptions;
using ZHQXC.Client;

namespace ZHQXC
{
    internal class OperationsTaskApplicationService
    {
        public IList<WCSTask> GetTasks()
        {
            List<Task> tasks;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                tasks = unitOfWork.session.Query<Task>().ToList();
                unitOfWork.Commit();
            }

            return tasks.Select(ToTaskDto).ToList();
        }

        public WCSTask GetTaskById(int taskId)
        {
            Task task = LoadTaskById(taskId);
            if (task == null)
            {
                throw new TaskNotFoundException(taskId);
            }

            return ToTaskDto(task);
        }

        public WCSTask GetTaskByCode(string taskCode)
        {
            return ToTaskDto(LoadTaskByCode(taskCode));
        }

        public WCSTask AddTask(WCSTask taskDto)
        {
            if (taskDto == null)
            {
                throw new ArgumentNullException("taskDto");
            }

            return ExecuteWithTaskUpdate(delegate
            {
                if (taskDto.StartLocation == null || string.IsNullOrWhiteSpace(taskDto.StartLocation.UserCode))
                {
                    throw new ArgumentException("任务起点不可为空", "taskDto.StartLocation");
                }

                if (taskDto.EndLocation == null || string.IsNullOrWhiteSpace(taskDto.EndLocation.UserCode))
                {
                    throw new ArgumentException("任务终点不可为空", "taskDto.EndLocation");
                }

                if (string.IsNullOrWhiteSpace(taskDto.Source))
                {
                    throw new ArgumentException("任务来源不可为空", "taskDto.Source");
                }

                if (string.IsNullOrWhiteSpace(taskDto.TaskCode))
                {
                    taskDto.TaskCode = Wcs.Framework.SerialNumberFactory.GenerateManualTaskCode();
                }

                var start = LocationConverter.ToLocationInfo(LocationConverter.UserCodeToLcation(taskDto.StartLocation.UserCode));
                var end = LocationConverter.ToLocationInfo(LocationConverter.UserCodeToLcation(taskDto.EndLocation.UserCode));

                Task task = new Task(taskDto.TaskCode, start, end);
                if (taskDto.ContainerCodes != null)
                {
                    task.ContainerCodes.AddAll(taskDto.ContainerCodes);
                }

                task.AdditionalInfo = taskDto.AdditionalInfo == null
                    ? new Dictionary<string, string>()
                    : taskDto.AdditionalInfo.ToDictionary(x => x.Key, x => x.Value);
                task.Source = (TaskSource)Enum.Parse(typeof(TaskSource), taskDto.Source);
                task.TaskType = taskDto.TaskType;
                task.Description = taskDto.Description;

                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    unitOfWork.session.Save(task);
                    unitOfWork.Commit();
                }

                Wcs.Framework.EventBus.EventBus.Instance.Publish<Wcs.Framework.Events.TaskAddedEvent>(new Wcs.Framework.Events.TaskAddedEvent(task));
                return ToTaskDto(task);
            });
        }

        public WCSTask SuspendTaskById(int taskId)
        {
            return ExecuteTaskMutation(taskId, delegate
            {
                TaskHelper.Suspend(taskId);
            });
        }

        public WCSTask SuspendTaskByCode(string taskCode)
        {
            return ExecuteTaskMutationByCode(taskCode, delegate(int taskId)
            {
                TaskHelper.Suspend(taskId);
            });
        }

        public WCSTask CancelTaskById(int taskId)
        {
            return ExecuteTaskMutation(taskId, delegate
            {
                TaskHelper.CancelTask(taskId);
            });
        }

        public WCSTask CancelTaskByCode(string taskCode)
        {
            return ExecuteTaskMutationByCode(taskCode, delegate(int taskId)
            {
                TaskHelper.CancelTask(taskId);
            });
        }

        public WCSTask CompleteMovementById(int movementId)
        {
            return ExecuteMovementMutation(movementId, delegate
            {
                TaskHelper.CompleteMovement(movementId);
            });
        }

        public WCSTask CancelMovementById(int movementId)
        {
            return ExecuteMovementMutation(movementId, delegate
            {
                TaskHelper.CancleLogicMovement(movementId);
            });
        }

        public WCSTask CompleteActionById(int actionId)
        {
            return ExecuteActionMutation(actionId, delegate
            {
                TaskHelper.CompleteAction(actionId);
            });
        }

        public WCSTask CancelActionById(int actionId)
        {
            return ExecuteActionMutation(actionId, delegate
            {
                TaskHelper.CancleEquipmentAction(actionId);
            });
        }

        public WCSTask CompleteTaskById(int taskId)
        {
            return ExecuteTaskMutation(taskId, delegate
            {
                TaskHelper.Complete(taskId);
            });
        }

        public WCSTask ResumeTaskById(int taskId, string currentUserCode)
        {
            return ExecuteTaskMutation(taskId, delegate
            {
                TaskHelper.Resume(taskId, ResolveCurrentLocation(currentUserCode));
            });
        }

        public WCSTask ResumeTaskByCode(string taskCode, string currentUserCode)
        {
            return ExecuteTaskMutationByCode(taskCode, delegate(int taskId)
            {
                TaskHelper.Resume(taskId, ResolveCurrentLocation(currentUserCode));
            });
        }

        public WCSTask ChangePriorityById(int taskId, int priority)
        {
            return ExecuteTaskMutation(taskId, delegate
            {
                ChangeTaskPriority(taskId, priority);
            });
        }

        public WCSTask ChangePriorityByCode(string taskCode, int priority)
        {
            return ExecuteTaskMutationByCode(taskCode, delegate(int taskId)
            {
                ChangeTaskPriority(taskId, priority);
            });
        }

        public WCSTask ArchiveTaskById(int taskId)
        {
            return ExecuteTaskMutation(taskId, delegate
            {
                TaskHelper.Archive(taskId);
            });
        }

        public WCSTask ArchiveTaskByCode(string taskCode)
        {
            return ExecuteTaskMutationByCode(taskCode, delegate(int taskId)
            {
                TaskHelper.Archive(taskId);
            });
        }

        static Location ResolveCurrentLocation(string currentUserCode)
        {
            if (string.IsNullOrWhiteSpace(currentUserCode))
            {
                return null;
            }

            return LocationConverter.UserCodeToLcation(currentUserCode);
        }

        static void ChangeTaskPriority(int taskId, int priority)
        {
            if (priority < 0)
            {
                throw new ArgumentOutOfRangeException("priority", "priority 必须大于或等于 0");
            }

            Task task = LoadTaskById(taskId);
            if (task == null)
            {
                throw new TaskNotFoundException(taskId);
            }

            if (task.Status == TaskStatus.Cancelled || task.Status == TaskStatus.Completed || task.Status == TaskStatus.Executing)
            {
                throw new InvalidOperationException(string.Format("任务 {0} 当前状态不可修改优先级", task.TaskCode));
            }

            TaskHelper.ChangePriority(priority, task.Id);
        }

        static WCSTask ExecuteTaskMutationByCode(string taskCode, Action<int> operation)
        {
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                throw new ArgumentNullException("taskCode");
            }

            Task task = LoadTaskByCode(taskCode);
            if (task == null)
            {
                throw CreateTaskNotFoundException(taskCode);
            }

            return ExecuteTaskMutation(task.Id, delegate
            {
                operation(task.Id);
            });
        }

        static WCSTask ExecuteTaskMutation(int taskId, Action operation)
        {
            return ExecuteWithTaskUpdate(delegate
            {
                operation();
                return ToTaskDto(LoadTaskById(taskId));
            });
        }

        static WCSTask ExecuteMovementMutation(int movementId, Action operation)
        {
            if (movementId <= 0)
            {
                throw new ArgumentOutOfRangeException("movementId");
            }

            return ExecuteWithTaskUpdate(delegate
            {
                int taskId = LoadTaskIdFromMovement(movementId);
                if (taskId <= 0)
                {
                    throw new ArgumentException(string.Format("未找到逻辑动作 {0}", movementId), "movementId");
                }

                operation();
                return ToTaskDto(LoadTaskById(taskId));
            });
        }

        static WCSTask ExecuteActionMutation(int actionId, Action operation)
        {
            if (actionId <= 0)
            {
                throw new ArgumentOutOfRangeException("actionId");
            }

            return ExecuteWithTaskUpdate(delegate
            {
                int taskId = LoadTaskIdFromAction(actionId);
                if (taskId <= 0)
                {
                    throw new ArgumentException(string.Format("未找到物理动作 {0}", actionId), "actionId");
                }

                operation();
                return ToTaskDto(LoadTaskById(taskId));
            });
        }

        static T ExecuteWithTaskUpdate<T>(Func<T> operation)
        {
            string holderMessage = string.Empty;
            if (!RemoteHandTaskHelper.Hold(holderMessage))
            {
                throw new InvalidOperationException("未获取更新权限，请稍后再试！");
            }

            try
            {
                return operation();
            }
            finally
            {
                RemoteHandTaskHelper.Holder = string.Empty;
            }
        }

        static Task LoadTaskById(int taskId)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                Task task = unitOfWork.session.Get<Task>(taskId);
                unitOfWork.Commit();
                return task;
            }
        }

        static Task LoadTaskByCode(string taskCode)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                Task task = unitOfWork.session.Query<Task>().FirstOrDefault(x => x.TaskCode == taskCode);
                unitOfWork.Commit();
                return task;
            }
        }

        static int LoadTaskIdFromMovement(int movementId)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                LogicMovement movement = unitOfWork.session.Get<LogicMovement>(movementId);
                int taskId = movement == null || movement.Task == null ? 0 : movement.Task.Id;
                unitOfWork.Commit();
                return taskId;
            }
        }

        static int LoadTaskIdFromAction(int actionId)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                EquipmentAction action = unitOfWork.session.Get<EquipmentAction>(actionId);
                int taskId = action == null || action.Movement == null || action.Movement.Task == null
                    ? 0
                    : action.Movement.Task.Id;
                unitOfWork.Commit();
                return taskId;
            }
        }

        static WCSTask ToTaskDto(Task task)
        {
            if (task == null)
            {
                return null;
            }

            return new WCSTask(task);
        }

        static Exception CreateTaskNotFoundException(string taskCode)
        {
            return new TaskNotFoundException(taskCode);
        }
    }
}
