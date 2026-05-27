using Matedata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceServer.Services.RailGuidedVehicleDeviceService
{
    public class RailGuidedVehicleDeviceServiceProxy : DiagnoseMatedataServiceProxy<DeviceServer.Services.RailGuidedVehicleDeviceService.RailGuidedVehicleService.IRailGuidedVehicleService, RailGuidedVehicleDeviceServiceProxy>, DeviceServer.Services.RailGuidedVehicleDeviceService.RailGuidedVehicleService.IRailGuidedVehicleService
    {
        public string Diagnose(string name)
        {

            using (RailGuidedVehicleDeviceService.RailGuidedVehicleService.RailGuidedVehicleServiceClient client = new RailGuidedVehicleService.RailGuidedVehicleServiceClient())
            {
                return client.Diagnose(name);
            }
        }

        public string GetAllStatus()
        {
            using (RailGuidedVehicleDeviceService.RailGuidedVehicleService.RailGuidedVehicleServiceClient client = new RailGuidedVehicleService.RailGuidedVehicleServiceClient())
            {
                return client.GetAllStatus();
            }
        }

        public void Move(string name, int station)
        {
            using (RailGuidedVehicleDeviceService.RailGuidedVehicleService.RailGuidedVehicleServiceClient client = new RailGuidedVehicleService.RailGuidedVehicleServiceClient())
            {
                client.Move(name,station);
            }
        }

        public void Picking(string name, int direction)
        {
            using (RailGuidedVehicleDeviceService.RailGuidedVehicleService.RailGuidedVehicleServiceClient client = new RailGuidedVehicleService.RailGuidedVehicleServiceClient())
            {
                client.Picking(name,direction);
            }
        }

        public void Putting(string name, int direction)
        {
            using (RailGuidedVehicleDeviceService.RailGuidedVehicleService.RailGuidedVehicleServiceClient client = new RailGuidedVehicleService.RailGuidedVehicleServiceClient())
            {
                client.Putting(name, direction);
            }
        }

        public void ClearTask(string name)
        {
            using (RailGuidedVehicleDeviceService.RailGuidedVehicleService.RailGuidedVehicleServiceClient client = new RailGuidedVehicleService.RailGuidedVehicleServiceClient())
            {
                client.ClearTask(name);
            }
        }

        public override string EndpointConfigurationName
        {
            get { return "RailGuidedVehicleDeviceServiceClient"; }
        }
    }
}
