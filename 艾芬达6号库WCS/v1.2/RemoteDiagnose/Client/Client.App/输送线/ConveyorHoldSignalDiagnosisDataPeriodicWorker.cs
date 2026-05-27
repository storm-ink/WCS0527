using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class ConveyorHoldSignalDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "输送线占位元数据同步进程"; }
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
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Conveyor.HoldSignal>>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "OccupyDataLog");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.Conveyor.ConveyorRepository<Conveyor.HoldSignal> repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.Conveyor.HoldSignal log = new Client.Conveyor.HoldSignal
                {
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    Name = item.Value<String>("DeviceName"),
                    AssignmentID = item.Value<Int32>("AssignmentID"),
                    HandShake = (Conveyor.HoldSignalHandShake)item.Value<Int32>("HandShake"),
                    IO_Data = item.Value<Int32>("IO_Data"),
                    PosNo = item.Value<Int32>("PosNo"),
                    TU_ID = item.Value<Int32>("TU_ID"),
                    TU_Type = item.Value<Int32>("TU_Type")
                };

                repository.Add(log);
            }
        }
    }
}
