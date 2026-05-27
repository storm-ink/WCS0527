using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
namespace Client.App
{
    public class ConveyorLocationDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "输送线货位状态元数据同步进程"; }
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
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Conveyor.Location>>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "LocationDataLog");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.Conveyor.ConveyorRepository<Conveyor.Location> repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.Conveyor.Location log = new Client.Conveyor.Location
                {
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    Name = item.Value<String>("DeviceName"),
                    PosNo = item.Value<Int32>("PosNo"),
                    Status = (Conveyor.LocationStatus)item.Value<Int32>("Status")
                };

                //if (repository.Context.Session.Query<Client.Conveyor.Location>().Any(x => x.LogId == log.LogId))
                //{
                //    continue;
                //}

                repository.Add(log);
            }
        }
    }
}
