using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NLog;

namespace Wcs.App.Plugins
{
    /// <summary>
    /// Wms 服务操作异常处理
    /// </summary>
    public class WcfServiceOperationErrorHandler : System.ServiceModel.Dispatcher.IErrorHandler
    {
        #region IErrorHandler 成员

        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            try
            {
                var action = OperationContext.Current.RequestContext.RequestMessage.Headers.Action;
                FaultException fe = new FaultException(new FaultReason(error.ToString()), null);

                var f = fe.CreateMessageFault();
                fault = Message.CreateMessage(version, f, action);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ProvideFault " + error.ToString());
                Console.WriteLine("Processing Error " + ex.ToString());
            }
        }

        #endregion
    }
}
