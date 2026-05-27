using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class ConveyorOccupyDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "输送线光电信号元数据同步进程"; }
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
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Conveyor.Occupy>>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "OccupyDataLog");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.Conveyor.ConveyorRepository<Conveyor.Occupy> repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.Conveyor.Occupy log = new Client.Conveyor.Occupy
                {
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    Name = item.Value<String>("DeviceName"),
                    PosNo = item.Value<Int32>("PosNo"),
                    AftPosPotocell = item.Value<Boolean>("AftPosPotocell"),
                    AftProPotocell = item.Value<Boolean>("AftProPotocell"),
                    AftSloPotocell = item.Value<Boolean>("AftSloPotocell"),
                    DownPotocell = item.Value<Boolean>("DownPotocell"),
                    FroPosPotocell = item.Value<Boolean>("FroPosPotocell"),
                    FroProPotocell = item.Value<Boolean>("FroProPotocell"),
                    FroSloPotocell = item.Value<Boolean>("FroSloPotocell"),
                    PhocllUseStatus = item.Value<Int32>("PhocllUseStatus"),
                    UpPotocell = item.Value<Boolean>("UpPotocell"),
                };

                repository.Add(log);
            }
        }
    }
}
