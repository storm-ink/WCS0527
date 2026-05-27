using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class WcsLogDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "Wcs日志元数据同步进程"; }
        }

        public override bool Process()
        {
            using (WcsLogsDiagnoseMatedataService.MatedataServiceClient client = new WcsLogsDiagnoseMatedataService.MatedataServiceClient())
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<Client.Logs.WcsLogDiagnosisDataRepository>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1500, null);

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data,Client.Logs.WcsLogDiagnosisDataRepository repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                WcsLogDiagnosisData log = new WcsLogDiagnosisData
                {
                    Level = item.Value<String>("Level"),
                    Exception = item.Value<String>("Exception"),
                    Logger = item.Value<String>("Logger"),
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("LongDate"),
                    MachineName = item.Value<String>("MachineName"),
                    Message = item.Value<String>("Message"),
                    ThreadId = item.Value<Int32>("ThreadId"),
                    ProjectId = 0,
                    Sender = item.Value<String>("Sender"),
                    UserName = item.Value<String>("UserName"),
                    TaskCode = item.Value<String>("TaskCode"),
                    EquipmentTaskId = item.Value<Int32>("EquipmentTaskId")
                };
                repository.Add(log);
            }
        }
    }
}
