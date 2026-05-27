using Client.SingleForkCrane;
using NLog;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    /// <summary>
    /// 单伸叉堆垛机状态数据刷新进程
    /// </summary>
    public class SingleForkCraneRefreshStatusPeriodicWorker : PeriodicWorker
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        public override string Name
        {
            get { return "单伸叉堆垛机状态数据刷新进程"; }
        }

        public override bool Process()
        {
            _logger.Trace("开始刷新单伸叉堆垛机状态数据...");

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
                        var repository = factory.Create<ChangeOfStateRepository>();

                        result = repository.Create(null, 1000, startTime, null);

                        unitOfWork.Commit();
                    }
                }

                if (result.Finished)
                {
                    break;
                }

                startTime = result.LastTime;
                refreshTotals += result.CreatedRecords;

                System.Threading.Thread.Sleep(500);
            }

            _logger.Trace("单伸叉堆垛机状态数据刷新结束，本次共产生了 {0} 条状态变化记录。", refreshTotals);

            return false;
        }
    }
}
