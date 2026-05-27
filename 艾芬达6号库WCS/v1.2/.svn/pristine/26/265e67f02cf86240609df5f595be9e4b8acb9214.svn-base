using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;

namespace Matedata
{
    /// <summary>
    /// 一个创建不同类型通道的工作。允许用户在程序集配置中配置 /system.serviceModel/client/endpoint 详情。然后通过 endpoint.name 创建通道
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MatedataChannelFactory<TChannel,TAssemblyAnyType> : ChannelFactory<TChannel>
    {
        string _assemblyConfigurationFile;

        public MatedataChannelFactory(string endpointConfigurationName) : base(endpointConfigurationName) 
        {
            
        }

        public MatedataChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress) {
        
        }

        /// <summary>
        /// 获取当前程序集的配置文件名称(app.config)
        /// </summary>
        protected virtual String AssemblyConfigurationFile
        {
            get
            {
                String path = String.Concat(typeof(TAssemblyAnyType).Assembly.Location, ".config");
                return path;
            }
        }
        protected override void ApplyConfiguration(string configurationName)
        {
            if (CheckConfigFileExist(AssemblyConfigurationFile))
            {
                ExeConfigurationFileMap execfgMap = new ExeConfigurationFileMap();
                execfgMap.ExeConfigFilename = AssemblyConfigurationFile;
                Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(execfgMap, ConfigurationUserLevel.None);
                ServiceModelSectionGroup servicemodelSections = ServiceModelSectionGroup.GetSectionGroup(cfg);

                if (servicemodelSections.Client.Endpoints.Cast<ChannelEndpointElement>().Any(x => x.Name == configurationName))
                {
                    var m = base.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                        .OrderBy(x => x.Name);


                    var ApplyConfigurationMethod = this.GetType().BaseType.BaseType.GetMethod("ApplyConfiguration", BindingFlags.Instance | BindingFlags.NonPublic |BindingFlags.Public, null, new Type[] { typeof(String), typeof(System.Configuration.Configuration) }, null);

                    ApplyConfigurationMethod.Invoke(this, new object[] { configurationName, cfg });
                }
                else
                {
                    base.ApplyConfiguration(configurationName);
                }
            }
            else
            {
                base.ApplyConfiguration(configurationName);
            }
        }


        private bool CheckConfigFileExist(string configpath)
        {
            if (string.IsNullOrEmpty(configpath)) return false;
            if (!File.Exists(configpath)) return false;
            return true;
        }

    }
}