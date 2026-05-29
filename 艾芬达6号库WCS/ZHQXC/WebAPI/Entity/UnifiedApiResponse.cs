using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ZHQXC
{
    public class UnifiedApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("traceId", NullValueHandling = NullValueHandling.Ignore)]
        public string TraceId { get; set; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<UnifiedApiError> Errors { get; set; }

        public static UnifiedApiResponse Ok(object data, string message = "操作成功")
        {
            return new UnifiedApiResponse
            {
                Success = true,
                Code = "ok",
                Message = message,
                Data = data
            };
        }

        public static UnifiedApiResponse Fail(string code, string message, params UnifiedApiError[] errors)
        {
            return new UnifiedApiResponse
            {
                Success = false,
                Code = code,
                Message = message,
                Errors = errors == null ? null : errors.Where(x => x != null).ToList()
            };
        }
    }

    public class UnifiedApiError
    {
        public UnifiedApiError()
        {
        }

        public UnifiedApiError(string field, string message)
        {
            Field = field;
            Message = message;
        }

        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
