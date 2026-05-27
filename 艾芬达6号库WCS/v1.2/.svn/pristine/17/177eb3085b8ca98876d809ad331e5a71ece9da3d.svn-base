using Matedata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Wcs;
using Wcs.DefaultImpls.Conveyor;
using Wcs.Framework;
using NHibernate.Linq;

namespace DeviceServer.Services.ConveyorDeviceService
{
    public class ConveyorServiceProxy : DiagnoseMatedataServiceProxy<IConveyorService, ConveyorService>, IConveyorService
    {
        public string GetStatus(string deviceName, string fullTypeName)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                var result = service.GetStatus(deviceName, fullTypeName);

                factory.Close();

                return result;
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void ClearTask(string deviceName, int equipmentTaskId, int? atDBBlockIndex)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.ClearTask(deviceName, equipmentTaskId, atDBBlockIndex);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void DeleteTask(string deviceName, int equipmentTaskId, int? atDBBlockIndex)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.DeleteTask(deviceName, equipmentTaskId, atDBBlockIndex);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void ClearLocationTask(string deviceName, int posNo, int equipmentTaskId)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.ClearLocationTask(deviceName, posNo, equipmentTaskId);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void AddTask(string deviceName, int equipmentTaskId, int? routeNo, string startLocationUserCode, string endLocationUserCode)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.AddTask(deviceName, equipmentTaskId, routeNo, startLocationUserCode, endLocationUserCode);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public override string EndpointConfigurationName
        {
            get { return typeof(ConveyorService).Name; }
        }
    }
}
