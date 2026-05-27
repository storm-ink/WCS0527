using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class ConveyorAlarmDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "输送线货位报警元数据同步进程"; }
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
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Conveyor.Alarm>>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "AlarmDataLog");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.Conveyor.ConveyorRepository<Conveyor.Alarm> repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.Conveyor.Alarm log = new Client.Conveyor.Alarm
                {
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    Name = item.Value<String>("DeviceName"),
                    Breaker = item.Value<Boolean>("Breaker"),
                    Isolator = item.Value<Boolean>("Isolator"),
                    Lift_MotorBraker = item.Value<Boolean>("Lift_MotorBraker"),
                    Lift_MotorContactor = item.Value<Boolean>("Lift_MotorContactor"),
                    Manual = item.Value<Boolean>("Manual"),
                    MotorUseStatus = item.Value<Int32>("MotorUseStatus"),
                    OccupyOvertime = item.Value<Boolean>("OccupyOvertime"),
                    Photocell = item.Value<Boolean>("Photocell"),
                    PosNo = item.Value<Int32>("PosNo"),
                    RunOvertime = item.Value<Boolean>("RunOvertime"),
                    Spare = item.Value<Int32>("Spare"),
                    TaskNoGoods = item.Value<Boolean>("TaskNoGoods"),
                    X_MotorBraker = item.Value<Boolean>("X_MotorBraker"),
                    X_MotorContactor = item.Value<Boolean>("X_MotorContactor"),
                    X_MotorVAF = item.Value<Boolean>("X_MotorVAF"),
                    Y_MotorBraker = item.Value<Boolean>("Y_MotorBraker"),
                    Y_MotorContactor = item.Value<Boolean>("Y_MotorContactor"),
                    Y_MotorVAF = item.Value<Boolean>("Y_MotorVAF")         
                };

                repository.Add(log);
            }
        }
    }
}
