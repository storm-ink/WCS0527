using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Matedata
{
    /// <summary>
    /// 允许序列化所有属性
    /// </summary>
    public class WriteAllPropertiesContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var result = base.CreateProperties(type, memberSerialization).ToList();
            foreach (var item in result)
            {
                item.Ignored = false;
            }
            return result;
        }

        protected override IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
        {
            var result = base.CreateConstructorParameters(constructor, memberProperties);

            return result;
        }
    }
}
