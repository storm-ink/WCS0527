using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace Client.App
{
    public class RailGuidedVehicleDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "单货位穿梭车元数据同步进程"; }
        }

        public override bool Process()
        {
            using (RailGuidedVehicleDataDiagnoseMatedataService.MatedataServiceClient client = new RailGuidedVehicleDataDiagnoseMatedataService.MatedataServiceClient())
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<Client.RailGuidedVehicle.RailGuidedVehicleDiagnosisDataRepository>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, null);

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.RailGuidedVehicle.RailGuidedVehicleDiagnosisDataRepository repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.RailGuidedVehicle.RailGuidedVehicleDiagnosisData log = new Client.RailGuidedVehicle.RailGuidedVehicleDiagnosisData
                {
                    ContainerCode = item.Value<Int32>("ContainerCode"),
                    AtStation = item.Value<Boolean>("AtStation"),
                    CurrentStation = item.Value<Int32>("CurrentStation"),
                    ErrorCode = item.Value<Int32>("ErrorCode"),
                    Event = (Client.RailGuidedVehicle.RailGuidedVehicleEvent)item.Value<Int32>("Event"),
                    FromStation = item.Value<Int32>("FromStation"),
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    Name = item.Value<String>("DeviceName"),
                    Position = item.Value<Int32>("Position"),
                    State = (Client.RailGuidedVehicle.RailGuidedVehicleStatus)item.Value<Int32>("State"),
                    TaskId = item.Value<String>("TaskId"),
                    TaskMode = (Client.RailGuidedVehicle.RailGuidedVehicleTaskMode)item.Value<Int32>("TaskMode"),
                    Telex = item.Value<String>("Telex"),
                    ToStation = item.Value<Int32>("ToStation"),
                    ProjectId = 0
                };

                //if (repository.Context.Session.Query<Client.RailGuidedVehicle.RailGuidedVehicleDiagnosisData>().Any(x => x.LogId == log.LogId))
                //{
                //    continue;
                //}

                repository.Add(log);
            }
        }
    }
}
