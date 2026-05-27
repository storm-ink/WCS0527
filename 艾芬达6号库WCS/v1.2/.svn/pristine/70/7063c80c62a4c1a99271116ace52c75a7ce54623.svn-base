using Client.Cfg;
using Client.RailGuidedVehicle;
using NLog;
using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Client.App
{
    /// <summary>
    /// 报警数据刷新进程
    /// </summary>
    public class RefreshWarningsPeriodicWorker : PeriodicWorker
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        static Type[] types;
        static MethodInfo syncWarningMethodInfo;
        static RefreshWarningsPeriodicWorker()
        {
            types = RemoteDiagnoseSettings.Instance.DeviceDiagnosisDataTypes;
            _logger.Info("共加载了 {0} 个设备元数据类型", types.Length);
        }
        public override string Name
        {
            get { return "报警数据刷新进程"; }
        }

        public RefreshWarningsPeriodicWorker()
        {
            this.Interval = 30 * 1000;
        }

        public override bool Process()
        {
            foreach (var type in types)
            {
                try
                {
                    _logger.Trace("开始刷新 {0} 报警数据...",type);
                    int totals = 0;
                    using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                    {
                        using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                        {
                            var factory = new NhRepositoryFactory(context);
                            var repository = factory.Create<WarningRepository>();
                        
                            if (syncWarningMethodInfo == null)
                            {
                                syncWarningMethodInfo = repository.GetType().GetMethod("SyncWarning");
                            }

                            var m = syncWarningMethodInfo.MakeGenericMethod(new Type[] { type });

                            totals = (Int32)m.Invoke(repository, null);

                            unitOfWork.Commit();
                        }
                    }

                    _logger.Trace("{0} 报警数据刷新结束，共创建了 {1} 条报警数据。", type, totals);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("在刷新 {0} 报警的时候发生了异常:", type,ex.Message),ex);
                }
            }

            return false;
        }
    }
}
