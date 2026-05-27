using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace Client.App
{
    public class SingleForkCraneDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "单货位堆垛机元数据同步进程"; }
        }

        public override bool Process()
        {
            using (SingleForkCraneStatusDiagnoseMatedataService.MatedataServiceClient client = new SingleForkCraneStatusDiagnoseMatedataService.MatedataServiceClient())
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<Client.SingleForkCrane.StatusDiagnosisDataRepository>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "RequestStateCommandReplyDataLog");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.SingleForkCrane.StatusDiagnosisDataRepository repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.SingleForkCrane.StatusDiagnosisData log = new Client.SingleForkCrane.StatusDiagnosisData
                {
                    Level = item.Value<Int32>("Level"),
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    AtStation = item.Value<Boolean>("AtStation"),
                    Column = item.Value<Int32>("Column"),
                    ErrorCode = item.Value<Int32>("ErrorCode"),
                    Event = (Client.SingleForkCrane.CraneEvent)item.Value<Int32>("Event"),
                    ForkHorizontalPosition = (Client.SingleForkCrane.ForkHorizontalPosition)item.Value<Int32>("ForkHorizontalPosition"),
                    ForkVerticalPosition = (Client.SingleForkCrane.ForkVerticalPosition)item.Value<Int32>("ForkVerticalPosition"),
                    Name = item.Value<String>("DeviceName"),
                    State = (Client.SingleForkCrane.CraneStatus)item.Value<Int32>("State"),
                    TaskId = item.Value<String>("TaskId"),
                    Telex = item.Value<String>("Telex")
                };


                //if (repository.Context.Session.Query<Client.SingleForkCrane.StatusDiagnosisData>().Any(x => x.LogId == log.LogId))
                //{
                //    continue;
                //}


                repository.Add(log);
            }
        }
    }
}
