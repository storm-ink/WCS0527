using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using NLog;

namespace Wcs.App.Plugins
{
    public class WcfHosting<T>
        where T : class, new()
    {
        static Logger _logger=NLog.LogManager.GetCurrentClassLogger();
        private ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private bool _isRunning = false;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }

        public String Name
        {
            get
            {
                return typeof(T).GetDisplayName();
            }
        }

        /// <summary>
        /// 获取服务的所有地址
        /// </summary>
        public String[] BaseAddresses { get; private set; }
        
        private void RunSerivce()
        {
            _isRunning = true;
            _waitHandle.Reset();
            string displayName = Name;

            try
            {
                using (ServiceHost serivceHost = new ServiceHost(typeof(T)))
                {
                    serivceHost.Opened += delegate
                    {
                        _logger.Debug1(string.Format("{0} 服务已启动", displayName), this);
                    };

                    try
                    {
                        serivceHost.Open();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error1(ex, this);
                        throw;
                    }

                    if (serivceHost.BaseAddresses != null && serivceHost.BaseAddresses.Count>0)
                    {
                        this.BaseAddresses = serivceHost.BaseAddresses.Select(x => x.ToString()).ToArray();
                    }
                    else
                    {
                        this.BaseAddresses = serivceHost.Description.Endpoints.Select(x => x.Address.ToString()).ToArray();
                    }

                    _waitHandle.WaitOne();

                    serivceHost.Close();

                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
            }
            finally
            {
                _isRunning = false;

                _logger.Debug1(string.Format("{0} 服务已关闭", displayName), this);
            }

        }
        
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Launch()
        {
            _logger.Trace1(string.Format("准备启动 {0} 服务...", Name), this);
            if (_isRunning)
            {
                throw new InvalidOperationException(string.Format("{0} 已启动，无法重复操作", Name));
            }

            {
                ThreadStart ts = new ThreadStart(RunSerivce);
                Thread t = new Thread(ts);
                t.Name = string.Format("{0}服务宿主", Name);
                t.StartAndManaged();
            }

        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Shutdown()
        {
            _logger.Trace1(string.Format("准备关闭 {0} 服务...", Name), this);
            _waitHandle.Set();
        }
    }
}
