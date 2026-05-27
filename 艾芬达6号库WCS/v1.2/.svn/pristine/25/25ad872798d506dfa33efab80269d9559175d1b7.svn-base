using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;

namespace Matedata
{
    public abstract class DiagnoseMatedataServiceProxy<TChannel, TAssemblyAnyType> : DiagnoseMatedataService, IMatedataServiceProxy
        where TChannel:IMatedataService
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public abstract String EndpointConfigurationName { get; }
        protected ChannelFactory<TChannel> CreateChannelFactory()
        {
            ChannelFactory<TChannel> factory = new MatedataChannelFactory<TChannel, TAssemblyAnyType>(EndpointConfigurationName);

            return factory;
        }

        public override string SelectCommandText
        {
            get { throw new NotImplementedException(); }
        }

        public override string GetDiagnoseMatedatas(int minDataId, int batchSize, string appInfo)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                var result = service.GetDiagnoseMatedatas(minDataId, batchSize, appInfo);

                factory.Close();

                _logger.Info("minDataId = {0}, batchSize = {1}, appInfo = {2},ip = {3}:{4}", minDataId, batchSize, appInfo, this.CurrentEndpoint.Address, this.CurrentEndpoint.Port);

                return result;
            }
            catch (Exception ex)
            {
                _logger.Info("minDataId = {0}, batchSize = {1}, appInfo = {2},ip = {3}, {4}:{5}", minDataId, batchSize, appInfo, this.CurrentEndpoint.Address, this.CurrentEndpoint.Port,ex);

                factory.Abort();

                throw;
            }
        }
    }
}
