using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Matedata
{
    public abstract class MatedataService
    {
        public MatedataService()
        {
            if (File.Exists(AssemblyConfigurationFile))
            {
                AssemblyConfiguration = ReadConfiguration(AssemblyConfigurationFile);
            }

            if (File.Exists(ExecutingConfigurationFile))
            {
                ExecutingConfiguration = ReadConfiguration(ExecutingConfigurationFile);
            }

            if (AssemblyConfiguration == null && ExecutingConfigurationFile==null)
            {
                throw new FileNotFoundException("未找到任何可用的配置文件.");
            }
        }

        /// <summary>
        /// 获取当前程序集配置文件
        /// </summary>
        public virtual String AssemblyConfigurationFile
        {
            get
            {
                return String.Concat(GetType().Assembly.Location, ".config");
            }
        }

        /// <summary>
        /// 获取当前程序集所属的应用程序配置文件
        /// </summary>
        public virtual String ExecutingConfigurationFile
        {
            get
            {
                return String.Concat(System.Reflection.Assembly.GetEntryAssembly().Location, ".config");
            }
        }


        /// <summary>
        /// 获取当前配置信息
        /// </summary>
        protected virtual Configuration AssemblyConfiguration { get; set; }

        /// <summary>
        /// 获取当前配置信息
        /// </summary>
        protected virtual Configuration ExecutingConfiguration { get; set; }

        /// <summary>
        /// 从指定的配置信息中读取连接字符串
        /// </summary>
        /// <param name="cfg">配置信息</param>
        /// <param name="name">连接字符串名称</param>
        /// <returns></returns>
        protected virtual String GetConnectionString(Configuration cfg,String name)
        {
            if (cfg == null)
            {
                return null;
            }
            else
            {
                var ele = cfg.ConnectionStrings
                    .ConnectionStrings[name];
                if (ele == null)
                {
                    return null;
                }
                else
                {
                    if(string.IsNullOrEmpty(ele.ConnectionString.Trim()))
                    {
                        return null;
                    }

                    return ele.ConnectionString;
                }
            }
        }

        /// <summary>
        /// 从配置信息中获取指定名称的连接字符串。
        /// 优先级：
        /// 1、应用程序集.config
        /// 2、可执行文件.config
        /// </summary>
        /// <param name="name">连接字符串名称</param>
        /// <returns></returns>
        public virtual String GetConnectionString(String name)
        {
            var aConnectionString = GetConnectionString(AssemblyConfiguration, name);
            var eConnectionString = GetConnectionString(ExecutingConfiguration, name);

            if (String.IsNullOrEmpty(aConnectionString) && String.IsNullOrEmpty(eConnectionString))
            {
                throw new Exception("diagnoseMatedata 连接字符串未配置");
            }

            if (!string.IsNullOrEmpty(aConnectionString) && !string.IsNullOrEmpty(eConnectionString))
            {
                return aConnectionString;
            }

            if (!string.IsNullOrEmpty(aConnectionString))
            {
                return aConnectionString;
            }

            return eConnectionString;
        }


        /// <summary>
        /// 从指定的配置信息中读取连接字符串
        /// </summary>
        /// <param name="cfg">配置信息</param>
        /// <param name="name">连接字符串名称</param>
        /// <returns></returns>
        protected virtual String GetAppSetting(Configuration cfg, String name)
        {
            if (cfg == null)
            {
                return null;
            }
            else
            {
                var ele = cfg.AppSettings.Settings[name];
                if (ele == null)
                {
                    return null;
                }
                else
                {
                    if (string.IsNullOrEmpty(ele.Value.Trim()))
                    {
                        return null;
                    }

                    return ele.Value;
                }
            }
        }

        /// <summary>
        /// 从配置信息中获取指定名称的AppSettting值
        /// 优先级：
        /// 1、应用程序集.config
        /// 2、可执行文件.config
        /// </summary>
        /// <param name="name">键名</param>
        /// <returns></returns>
        public virtual T GetAppSetting<T>(String name, T defaultValue = default(T))
        {
            var aAppSetting = GetAppSetting(AssemblyConfiguration, name);
            var eAppSetting = GetAppSetting(ExecutingConfiguration, name);

            if (String.IsNullOrEmpty(aAppSetting) && String.IsNullOrEmpty(eAppSetting))
            {
                return defaultValue;
            }

            if (!string.IsNullOrEmpty(aAppSetting) && !string.IsNullOrEmpty(eAppSetting))
            {
                return (T)Convert.ChangeType(aAppSetting,typeof(T));
            }

            if (!string.IsNullOrEmpty(aAppSetting))
            {
                return (T)Convert.ChangeType(aAppSetting, typeof(T));
            }

            return (T)Convert.ChangeType(eAppSetting,typeof(T));
        }
        
        protected Configuration ReadConfiguration(String configurationFile)
        {
            ExeConfigurationFileMap execfgMap = new ExeConfigurationFileMap();
            execfgMap.ExeConfigFilename = configurationFile;
            Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(execfgMap, ConfigurationUserLevel.None);

            return cfg;
        }

        public System.ServiceModel.Channels.RemoteEndpointMessageProperty CurrentEndpoint
        {
            get
            {
                //提供方法执行的上下文环境
                System.ServiceModel.OperationContext context = System.ServiceModel.OperationContext.Current;
                //获取传进的消息属性
                System.ServiceModel.Channels.MessageProperties properties = context.IncomingMessageProperties;
                //获取消息发送的远程终结点IP和端口
                System.ServiceModel.Channels.RemoteEndpointMessageProperty endpoint = properties[System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name] 
                    as System.ServiceModel.Channels.RemoteEndpointMessageProperty;

                return endpoint;
            }
        }
    }
}
