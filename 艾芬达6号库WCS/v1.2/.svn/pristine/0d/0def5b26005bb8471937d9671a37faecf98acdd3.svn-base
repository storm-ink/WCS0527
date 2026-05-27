using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Configuration;
using NLog;

namespace Client.App
{
    /// <summary>
    /// 表示一个周期性的工作线程
    /// </summary>
    public abstract class PeriodicWorker
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        Boolean _abortProcess;
        Thread _thread;

        protected PeriodicWorker()
        {
            Interval = 2000;
        }

        /// <summary>
        /// 周期间隔时间
        /// </summary>
        public Int32 Interval { get; set; }

        /// <summary>
        /// 获取一个值，指示是否已暂停
        /// </summary>
        public bool IsPaused { get; protected set; }

        /// <summary>
        /// 获取一个值，指示此监视线程是否在工作。当调用 Pause 后，仍会返回 true
        /// </summary>
        public Boolean IsRunning
        {
            get
            {
                return _thread != null && _thread.IsAlive;
            }
        }
        /// <summary>
        /// 获取一个值，指示此工作进程是否正处于执行状态.
        /// </summary>
        public Boolean IsExecuting { get; protected set; }

        public abstract String Name { get; }
        /// <summary>
        /// 暂停后继续
        /// </summary>
        public void Continue()
        {
            IsPaused = false;
            _logger.Info("{0} 已继续", this.Name);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            IsPaused = true;
            _logger.Info("{0} 已暂停", this.Name);
        }
        /// <summary>
        /// 表示该工作线程的处理过程。返回值为 true，则不等待，立即进行下一轮循环。返回值为 false，则等待时间间隔过去后再进行下一轮循环
        /// </summary>
        public abstract bool Process();

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (this.IsRunning)
            {
                if (_logger.IsErrorEnabled)
                {
                    _logger.Error("无法重复启动 {0}", this.Name);
                }
                return;
            }

            _thread = new Thread(threadstart);
            _thread.IsBackground = true;
            _thread.Start();
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void Stop()
        {
            _abortProcess = true;
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("尝试停止 {0}", this.Name);
            }
        }
        private void threadstart()
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info("{0} 已启动", this.Name);
            }

            while (!_abortProcess)
            {

                if (IsPaused)
                {
                    Thread.Sleep(1);
                    continue;
                }

                try
                {
                    IsExecuting = true;

                    bool immediateNext = Process();

                    IsExecuting = false;

                    if (immediateNext)
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    if (_logger.IsErrorEnabled)
                    {
                        _logger.ErrorException(this.Name + "出错。", ex);
                    }
                }

                IsExecuting = false;

                Thread.Sleep(Interval);
            }


            if (_logger.IsInfoEnabled)
            {
                _logger.Info("{0} 已停止", this.Name);
            }
            _abortProcess = false;
            _thread = null;
        }
    }

}
