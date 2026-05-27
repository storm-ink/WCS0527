using Client.Logs;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    public class ConveyorLocationTaskDiagnosisDataPeriodicWorker : PeriodicWorker
    {
        public override string Name
        {
            get { return "输送线货位任务元数据同步进程"; }
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
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Conveyor.LocationTask>>();
                        var maxId = repository.GetTransmissionDataId();

                        var result = client.GetDiagnoseMatedatas(maxId, 1000, "LocationTaskDataLog");

                        saveData(result, repository);

                        unitOfWork.Commit();
                    }
                }
            }

            return false;
        }

        void saveData(String data, Client.Conveyor.ConveyorRepository<Conveyor.LocationTask> repository)
        {
            var jObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(data);

            foreach (var item in jObjects)
            {
                Client.Conveyor.LocationTask log = new Client.Conveyor.LocationTask
                {
                    LogId = item.Value<Int32>("Id"),
                    LongDate = item.Value<DateTime>("CreatedAt"),
                    ProjectId = 0,
                    Name = item.Value<String>("DeviceName"),
                    PosNo = item.Value<Int32>("PosNo"),
                    Fnh_Rcv_X = item.Value<Boolean>("Fnh_Rcv_X"),
                    Fnh_Rcv_Y = item.Value<Boolean>("Fnh_Rcv_Y"),
                    Rcv_Rdy = item.Value<Boolean>("Rcv_Rdy"),
                    Rqs_Snt = item.Value<Boolean>("Rqs_Snt"),
                    Str_Rcv_X = item.Value<Boolean>("Str_Rcv_X"),
                    Str_Rcv_Y = item.Value<Boolean>("Str_Rcv_Y"),
                    TaskNo = item.Value<Int32>("TaskNo"),
                    TUID = item.Value<Int32>("TUID")
                };

                repository.Add(log);
            }
        }
    }
}
