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
    /// 为元数据服务提供服务的主机。
    /// 服务的配置信息将先从当前程序集的配置文件中查找，如果没有则尝试从应用程序配置中查找。如果不存在任何配置文件，将引发异常
    /// </summary>
    public class MatedataServiceHost : ServiceHost
    {
        public MatedataServiceHost(Type serviceType, params Uri[] baseAddresses) :
            base(serviceType, baseAddresses)
        {
        }

        /// <summary>
        /// 获取当前程序集的配置文件名称(app.config)
        /// </summary>
        protected virtual String AssemblyConfigurationFile
        {
            get
            {
                String path = String.Concat(this.Description.ServiceType.Assembly.Location, ".config");
                return path;
            }
        }
        protected override void ApplyConfiguration()
        {
            // Check user config invalidation  
            if (!CheckConfigExist(AssemblyConfigurationFile))
            {
                // Use default config  
                base.ApplyConfiguration();
                return;
            }
            //base.ApplyConfiguration();  
            // Use user config  
            ExeConfigurationFileMap execfgMap = new ExeConfigurationFileMap();
            // Set user config FilePath  
            execfgMap.ExeConfigFilename = AssemblyConfigurationFile;
            // Config info  
            Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(execfgMap, ConfigurationUserLevel.None);
            // Gets all service model config sections  
            ServiceModelSectionGroup servicemodelSections = ServiceModelSectionGroup.GetSectionGroup(cfg);

            // Find serivce section matched with the name "this.Description.ServiceType.FullName"   
            if (!ApplySectionInfo(this.Description.ServiceType.FullName, servicemodelSections))
            {
                throw new Exception("ConfigApply Error : There is no endpoint existed in your config!! Please check your config file!");
            }
            this.ApplyMultiBehaviors(servicemodelSections);

        }

        /// <summary>
        /// Add behaviors
        /// </summary>
        /// <param name="servicemodelSections"></param>
        /// <returns></returns>
        private bool ApplyMultiBehaviors(ServiceModelSectionGroup servicemodelSections)
        {
            if (servicemodelSections == null) return false;
            foreach (ServiceBehaviorElement element in servicemodelSections.Behaviors.ServiceBehaviors)
            {
                foreach (BehaviorExtensionElement behavior in element)
                {
                    BehaviorExtensionElement behaviorEx = behavior;
                    object extention = behaviorEx.GetType().InvokeMember("CreateBehavior",
                        BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        behaviorEx,
                        null);
                    if (extention == null) continue;
                    IServiceBehavior isb = (IServiceBehavior)extention;
                    //if (base.Description.Behaviors.Contains(isb)) break;
                    bool isbehaviorExisted = false;
                    foreach (IServiceBehavior i in base.Description.Behaviors)
                    {
                        if (i.GetType().Name == isb.GetType().Name)
                        {
                            isbehaviorExisted = true;
                            break;
                        }
                    }
                    if (isbehaviorExisted) break;
                    base.Description.Behaviors.Add((IServiceBehavior)extention);
                }
            }
            return true;
        }

        /// <summary>
        /// Apply section info
        /// </summary>
        /// <param name="serviceFullName"></param>
        /// <param name="servicemodelSections"></param>
        /// <returns></returns>
        private bool ApplySectionInfo(string serviceFullName, ServiceModelSectionGroup servicemodelSections)
        {
            // Check config sections (!including one section at least!)
            if (servicemodelSections == null) return false;
            // Service name can't be none!
            if (string.IsNullOrEmpty(serviceFullName)) return false;
            bool isElementExist = false;
            foreach (ServiceElement element in servicemodelSections.Services.Services)
            {
                if (element.Name == serviceFullName)
                {
                    // Find successfully & apply section info of config file
                    base.LoadConfigurationSection(element);
                    // Find service element successfully
                    isElementExist = true;
                    break;
                }
            }
            return isElementExist;
        }

        /// <summary>
        /// Check config file! 
        /// </summary>
        /// <param name="configpath"></param>
        /// <returns></returns>
        private bool CheckConfigExist(string configpath)
        {
            if (string.IsNullOrEmpty(configpath)) return false;
            if (!File.Exists(configpath)) return false;
            return true;
        }
    }
}
