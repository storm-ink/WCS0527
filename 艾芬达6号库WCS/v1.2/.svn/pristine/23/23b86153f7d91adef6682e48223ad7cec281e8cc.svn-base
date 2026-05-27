using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using NLog;
using Matedata;

namespace MatedataServer.App
{
    /// <summary>
    /// Wcf服务宿主
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WcfHosting<T>
        where T : class, new()
    {
        static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private bool _isRunning = false;
        private ManualResetEvent _waitHandle = new ManualResetEvent(false);
        /// <summary>
        /// 获取服务的所有地址
        /// </summary>
        public String[] BaseAddresses { get; private set; }

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
                var type = typeof(T);
                var displayNameAttr = type.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false).Cast<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
                if (displayNameAttr != null)
                {
                    return displayNameAttr.DisplayName;
                }

                var descriptionAttr = type.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false).Cast<System.ComponentModel.DescriptionAttribute>().FirstOrDefault();
                if (descriptionAttr != null)
                {
                    return descriptionAttr.Description;
                }

                return type.Name;
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Launch()
        {
            _logger.Trace("准备启动 {0} 服务...", Name);
            if (_isRunning)
            {
                throw new InvalidOperationException(String.Format("{0} 已启动，无法重复操作", Name));
            }

            {
                ThreadStart ts = new ThreadStart(RunSerivce);
                Thread t = new Thread(ts);
                t.Start();
            }

        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Shutdown()
        {
            _logger.Trace("准备关闭 {0} 服务...", Name);
            _waitHandle.Set();
        }

        private void RunSerivce()
        {
            _isRunning = true;
            _waitHandle.Reset();
            string displayName = Name;

            try
            {
                using (MatedataServiceHost serivceHost = new MatedataServiceHost(typeof(T)))
                {
                    serivceHost.Opened += delegate
                    {
                        _logger.Debug("{0} 服务已启动", displayName);
                    };

                    try
                    {
                        serivceHost.Open();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        throw;
                    }

                    if (serivceHost.BaseAddresses != null && serivceHost.BaseAddresses.Count > 0)
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
                _logger.Error(ex);
            }
            finally
            {
                _isRunning = false;

                _logger.Debug("{0} 服务已关闭", displayName);
            }

        }
    }
}
