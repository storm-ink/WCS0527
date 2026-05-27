using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class ConveyorTaskDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "输送线任务元数据同步进程"; }
        }

        public override bool Process()
        {
            using (ConveyorDataDiagnoseMatedataService.MatedataServiceClient client = new ConveyorDataDiagnoseMatedataService.MatedataServiceClient())
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Conveyor.Task>>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "TaskDataLog");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.Conveyor.ConveyorRepository<Conveyor.Task> repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.Conveyor.Task log = new Client.Conveyor.Task
                {
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    Name = item.Value<String>("DeviceName"),
                    AssignmentID = item.Value<Int32>("AssignmentID"),
                    DestinationNo = item.Value<Int32>("DestinationNo"),
                    HandShake = (Conveyor.TaskHandShake)item.Value<Int32>("HandShake"),
                    IO_Data = item.Value<Int32>("IO_Data"),
                    ReadTask = item.Value<Int32>("ReadTask"),
                    RotingNo = item.Value<Int32>("RotingNo"),
                    Spare = item.Value<Int32>("Spare"),
                    StartMotorNo = item.Value<Int32>("StartMotorNo"),
                    Status = (Conveyor.TaskStatus)item.Value<Int32>("TaskStatus"),
                    TU_ID = item.Value<Int32>("TU_ID"),
                    TU_Type = item.Value<Int32>("TU_Type")
                };

                repository.Add(log);
            }
        }
    }
}
