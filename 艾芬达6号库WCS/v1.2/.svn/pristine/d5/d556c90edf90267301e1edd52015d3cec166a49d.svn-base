using Matedata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceServer.Services.SingleForkCraneDeviceService
{
    public class CraneServiceProxy : DeviceServiceProxy<ICraneService, CraneService>, ICraneService
    {
        public List<CraneInfo> LoadCraneInfos()
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                var result = service.LoadCraneInfos();

                factory.Close();

                return result;
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public Dictionary<string, LA> ReadStatus()
        {
            var factory = CreateChannelFactory(); 
            try
            {
                var service = factory.CreateChannel();
                var result = service.ReadStatus();

                factory.Close();

                return result;
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void BackToTheOrigin(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.BackToTheOrigin(craneName, userName, ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void EmergencyStop(string craneName)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.EmergencyStop(craneName);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void CancelEmergencyStop(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.CancelEmergencyStop( craneName,  userName,  ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Move(string craneName, string userName, string ipAddress, int userColumn, int userLevel)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Move(craneName, userName, ipAddress, userColumn, userLevel);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Pick(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Pick(craneName, userName, ipAddress, forkDirection);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Putdown(string craneName, string userName, string ipAddress, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Putdown(craneName, userName, ipAddress, forkDirection);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Lock(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Lock(craneName, userName, ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Unlock(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Unlock(craneName, userName, ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Up(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Up(craneName, userName, ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Down(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Down(craneName, userName, ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Forward(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Forward(craneName, userName, ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }

        public void Back(string craneName, string userName, string ipAddress)
        {
            var factory = CreateChannelFactory();
            try
            {
                var service = factory.CreateChannel();
                service.Back(craneName, userName, ipAddress);

                factory.Close();
            }
            catch (Exception)
            {

                factory.Abort();

                throw;
            }
        }
    }
}
