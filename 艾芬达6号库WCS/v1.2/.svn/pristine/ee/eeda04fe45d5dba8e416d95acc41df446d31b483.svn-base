using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.WebUI.Areas.Conveyor.Controllers
{
    public class ConveyorDeviceController : Controller
    {
        public ContentResult GetStatus(string deviceName, string fullTypeName)
        {
            using (App.ConveyorDeviceServiceProxy.ConveyorServiceClient client = new App.ConveyorDeviceServiceProxy.ConveyorServiceClient())
            {
                return Content(client.GetStatus(deviceName, fullTypeName));
            }
        }

        public void ClearTask(string deviceName, int equipmentTaskId, int? atDBBlockIndex)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(string deviceName, int equipmentTaskId, int? atDBBlockIndex)
        {
            throw new NotImplementedException();
        }

        public void ClearLocationTask(string deviceName, int posNo, int equipmentTaskId)
        {
            throw new NotImplementedException();
        }

        public void AddTask(string deviceName, int equipmentTaskId, int? routeNo, string startLocationUserCode, string endLocationUserCode)
        {
            throw new NotImplementedException();
        }
    }
}
