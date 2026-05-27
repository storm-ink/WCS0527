using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;
using System.ServiceModel.Dispatcher;

namespace Matedata
{
    /// <summary>
    /// 用于服务操作的行为扩展
    /// </summary>
    public class WcfServiceOperationBehavior : BehaviorExtensionElement, IServiceBehavior
    {
        public override Type BehaviorType
        {
            get { return typeof(WcfServiceOperationBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new WcfServiceOperationBehavior();
        }

        #region IServiceBehavior 成员

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher chanDisp in serviceHostBase.ChannelDispatchers)  
            {
                chanDisp.ErrorHandlers.Add(new WcfServiceOperationErrorHandler());  
            } 
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
