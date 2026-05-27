using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Wcs.WebApi
{
    [RoutePrefix("API/Test")]
    public class TestController : ApiController
    {
        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [Route("test")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetServerTime()
        {
            return Json(new { Message = $"This is Port Test Message from AGV.Component.Tools.WebAPI.TestController, Now is {DateTime.Now.ToString("yyyyy-MM-dd HH:mm:ss.ffff")}" });
        }
    }
}