using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Client.WebUI.Areas.SingleForkCrane.Controllers
{
    [Authorize]
    public class DeviceServiceController : Controller
    {
        String RealName
        {
            get
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    NhRepositoryFactory factory = new NhRepositoryFactory(context);
                    var userRepository = factory.Create<Spiral.Base.UserRepositoryBase>();

                    var user = userRepository.GetByUserName(this.HttpContext.User.Identity.Name);

                    return user.RealName;
                }
            }
        }

        String IP
        {
            get
            {
                return this.HttpContext.Request.Url.Authority;
            }
        }


        [WmsOperation("单货位堆垛机.控制服务.诊断指令")]
        public ContentResult Diagnose(string name)
        {
            //using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
            //{
            //    return Content(client.Diagnose(name));
            //}
            throw new NotImplementedException();
        }

        [WmsOperation("单货位堆垛机.控制服务.加载设备信息")]
        [HttpGet]
        public JsonResult LoadCraneInfos()
        {
            using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
            {
                return Json(client.LoadCraneInfos(),JsonRequestBehavior.AllowGet);
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.获取实时状态")]
        [HttpGet]
        public JsonResult ReadStatus()
        {
            using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
            {
                List<object> result = new List<object>();
                foreach (var item in client.ReadStatus())
                {
                    if (item.Value!= null)
                    {
                        result.Add(new
                        {
                            Name = item.Key,
                            AtStation = item.Value.AtStation,
                            ErrorCode = item.Value.ErrorCode,
                            ErrorDescription = item.Value.ErrorDescription,
                            ErrorName = item.Value.ErrorName,
                            ErrorSolution = item.Value.ErrorSolution,
                            Event = Convert.ToInt32(item.Value.Event),
                            ForkHorizontalPosition = Convert.ToInt32(item.Value.ForkHorizontalPosition),
                            ForkVerticalPosition = Convert.ToInt32(item.Value.ForkVerticalPosition),
                            LockerIp = item.Value.LockerIp,
                            LockerUser = item.Value.LockerUser,
                            State = Convert.ToInt32(item.Value.State),
                            TaskId = item.Value.TaskId,
                            UserColumn = item.Value.UserColumn,
                            UserLevel = item.Value.UserLevel
                        });
                    }
                    else
                    {
                        result.Add(new
                        {
                            Name = item.Key,
                            AtStation = false,
                            ErrorCode = 0,
                            ErrorDescription = "未连接",
                            ErrorName = "未连接",
                            ErrorSolution = "",
                            Event = 0,
                            ForkHorizontalPosition = 0,
                            ForkVerticalPosition = 0,
                            //LockerIp = "",
                            //LockerUser = "",
                            State = 0,
                            TaskId = "00000000",
                            UserColumn = "0",
                            UserLevel = "0"
                        });
                    }
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.回原点指令")]
        [HttpPost]
        public JsonResult BackToTheOrigin(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.BackToTheOrigin(craneName, this.RealName, this.IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 回原点指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.急停指令")]
        [HttpPost]
        public JsonResult EmergencyStop(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.EmergencyStop(craneName);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 急停指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.取消急停指令")]
        [HttpPost]
        public JsonResult CancelEmergencyStop(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.CancelEmergencyStop(craneName,RealName,IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 取消急停指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.移动指令")]
        [HttpPost]
        public JsonResult Move(string craneName, int userColumn, int userLevel)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Move(craneName, RealName, IP, userColumn,userLevel);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 移动指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.取货指令")]
        [HttpPost]
        public JsonResult Pick(string craneName, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Pick(craneName, RealName, IP, forkDirection);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 取货指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.放货指令")]
        [HttpPost]
        public JsonResult Putdown(string craneName, Wcs.DefaultImpls.Crane.ForkDirection forkDirection)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Putdown(craneName, RealName, IP, forkDirection);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 放货指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.锁定")]
        [HttpPost]
        public JsonResult Lock(string craneName)
        {
            try
            {
                 using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Lock(craneName, this.RealName, this.IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 锁定成功",craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success=false,
                    msg=ex.Message
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.解除锁定")]
        [HttpPost]
        public JsonResult Unlock(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Unlock(craneName, this.RealName, this.IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 解锁成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.提升指令")]
        [HttpPost]
        public JsonResult Up(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Up(craneName, RealName, IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 提升指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.下降指令")]
        [HttpPost]
        public JsonResult Down(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Down(craneName, RealName, IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 下降指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.前进指令")]
        [HttpPost]
        public JsonResult Forward(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Forward(craneName, RealName, IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 前进指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位堆垛机.控制服务.后退指令")]
        [HttpPost]
        public JsonResult Back(string craneName)
        {
            try
            {
                using (App.CraneDeviceServiceProxy.CraneServiceClient client = new App.CraneDeviceServiceProxy.CraneServiceClient())
                {
                    client.Back(craneName, RealName, IP);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 后退指令发送成功", craneName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        String getRemoteExceptionMessage(Exception ex)
        {
            var msg = getRemoteExceptionMessage(ex);

            var match = Regex.Match(msg, @"System\.ServiceModel\.FaultException\:\s*(?<msg>.*?)\r\n");
            if (match != null)
            {
                return match.Groups["msg"].Value;
            }
            else
            {
                return msg;
            }
        }
    }
}
