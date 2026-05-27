using System;

using System.Collections.Generic;

using System.IO;

using System.Net;

using System.Text;

using Newtonsoft.Json;

using NLog;

using Wcs;

using Wcs.Framework.Cfg;



namespace Wcs.DefaultImplementCollection.AGV

{

    /// <summary>

    /// WCS 作为客户端调用 AGV 侧 HTTP 接口。

    /// </summary>

    public static class AgvHttpApiClient

    {

        static readonly Logger _logger = LogManager.GetCurrentClassLogger();



        public static AgvApiResult DispatchTask(AgvDispatchTaskRequest request)

        {

            if (request == null)

                throw new ArgumentNullException(nameof(request));



            var baseUrl = WcsConfiguration.Instance.SettingCollection.GetSetting<string>("agvApiBaseUrl", "").TrimEnd('/');

            var path = WcsConfiguration.Instance.SettingCollection.GetSetting<string>("agvDispatchTaskApiPath", "/api/agv/DispatchTask");

            if (!path.StartsWith("/"))

                path = "/" + path;



            var method = WcsConfiguration.Instance.SettingCollection.GetSetting<string>("agvHttpApiMethod", "POST").ToUpperInvariant();

            var url = BuildUrlWithQuery(baseUrl + path, ToQuery(request));

            _logger.Info1($"WCS-->AGV 下发任务 {url}", typeof(AgvHttpApiClient));



            var json = Invoke(url, method, null);

            _logger.Info1($"WCS-->AGV 下发任务回复 {json}", typeof(AgvHttpApiClient));



            var result = JsonConvert.DeserializeObject<AgvApiResult>(json);

            if (result == null)

                throw new Exception($"AGV 下发任务接口返回无法解析: {json}");

            if (!result.IsSuccess)

                throw new Exception($"AGV 下发任务失败 code={result.Code}, msg={result.Msg}");

            return result;

        }



        static Dictionary<string, string> ToQuery(AgvDispatchTaskRequest request)

        {

            var q = new Dictionary<string, string>

            {

                ["TaskId"] = request.TaskId ?? "",

                ["TaskOwner"] = request.TaskOwner ?? "wcs",

                ["ContainerCode"] = request.ContainerCode ?? "",

                ["StartLocationCode"] = request.StartLocationCode ?? "",

                ["EndLocationCode"] = request.EndLocationCode ?? ""

            };

            if (request.AdditionalInfo != null && request.AdditionalInfo.Count > 0)

                q["AdditionalInfo"] = JsonConvert.SerializeObject(request.AdditionalInfo);

            return q;

        }



        static string BuildUrlWithQuery(string url, Dictionary<string, string> query)

        {

            var sb = new StringBuilder(url);

            sb.Append(url.Contains("?") ? "&" : "?");

            var first = true;

            foreach (var kv in query)

            {

                if (!first)

                    sb.Append("&");

                first = false;

                sb.Append(Uri.EscapeDataString(kv.Key));

                sb.Append("=");

                sb.Append(Uri.EscapeDataString(kv.Value ?? ""));

            }

            return sb.ToString();

        }



        static string Invoke(string url, string method, string bodyJson)

        {

            var timeout = WcsConfiguration.Instance.SettingCollection.GetSetting<int>("agvHttpApiTimeout", 30000);

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = method;

            request.Accept = "application/json";

            request.Timeout = timeout;

            request.ReadWriteTimeout = timeout;



            if (method == "POST" && !string.IsNullOrEmpty(bodyJson))

            {

                request.ContentType = "application/json";

                var buffer = Encoding.UTF8.GetBytes(bodyJson);

                request.ContentLength = buffer.Length;

                using (var stream = request.GetRequestStream())

                    stream.Write(buffer, 0, buffer.Length);

            }



            using (var response = (HttpWebResponse)request.GetResponse())

            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))

                return reader.ReadToEnd();

        }

    }

}


