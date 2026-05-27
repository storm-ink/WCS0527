using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class ConveyorAppearanceInspectionDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "输送线货位外形检测元数据同步进程"; }
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
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Conveyor.AppearanceInspection>>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "ProfileMeasurementResultNetTransferObjectLogData");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.Conveyor.ConveyorRepository<Conveyor.AppearanceInspection> repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.Conveyor.AppearanceInspection log = new Client.Conveyor.AppearanceInspection
                {
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    Name = item.Value<String>("DeviceName"),
                    No = item.Value<Int32>("No"),
                    Back_Over = item.Value<Boolean>("Back_Over"),
                    Front_Over = item.Value<Boolean>("Front_Over"),
                    Left_Over = item.Value<Boolean>("Left_Over"),
                    Result = (Conveyor.AppearanceInspectionResult)item.Value<Int32>("Result"),
                    Right_Over = item.Value<Boolean>("Right_Over"),
                    Too_High = item.Value<Boolean>("Too_High")
                };

                repository.Add(log);
            }
        }
    }
}
