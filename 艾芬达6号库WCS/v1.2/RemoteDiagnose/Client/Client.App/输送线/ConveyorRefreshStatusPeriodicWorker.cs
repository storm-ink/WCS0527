using Client.Conveyor;
using NLog;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    /// <summary>
    /// 输送线状态数据刷新进程
    /// </summary>
    public class ConveyorRefreshStatusPeriodicWorker : PeriodicWorker
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        public override string Name
        {
            get { return "输送线状态数据刷新进程"; }
        }

        public override bool Process()
        {
            _logger.Trace("开始刷新输送线状态数据...");

            CreateChangeOfStatusResult result;
            DateTime? startTime = null;
            Int32 refreshTotals = 0;
            while (true)
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        var factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<LocationChangeOfStateRepository>();

                        result = repository.Create(null, 1000, startTime, null);

                        unitOfWork.Commit();
                    }
                }

                if (result.Finished)
                {
                    break;
                }

                
                startTime = Convert.ToDateTime(result.LastTime);
                refreshTotals += result.RefreshCount;

                System.Threading.Thread.Sleep(500);
            }

            _logger.Trace("输送线状态数据刷新结束，本次共产生了 {0} 条状态变化记录。", refreshTotals);

            return false;
        }
    }
}
