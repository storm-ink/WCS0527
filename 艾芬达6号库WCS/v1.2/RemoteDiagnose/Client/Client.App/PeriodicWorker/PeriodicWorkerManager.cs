using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.App
{
    /// <summary>
    /// 周期性的工作线程管理器
    /// </summary>
    public class PeriodicWorkerManager
    {
        static List<PeriodicWorker> _workers = new List<PeriodicWorker>();
        public static PeriodicWorker[] Workers
        {
            get
            {
                return _workers.ToArray();
            }
        }

        public static TPeriodicWork Item<TPeriodicWork>() where TPeriodicWork : PeriodicWorker, new()
        {
            lock (typeof(PeriodicWorkerManager))
            {
                var result = _workers.SingleOrDefault(x => x is TPeriodicWork);
                if (result == null)
                {
                    result = new TPeriodicWork();
                    _workers.Add(result);
                }

                return (TPeriodicWork)result;
            }
        }
    }
}
