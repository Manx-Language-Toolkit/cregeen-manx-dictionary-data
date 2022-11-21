using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cregeen
{
    class Json
    {
        public class ShouldSerializeContractResolver : DefaultContractResolver
        {
            public static readonly ShouldSerializeContractResolver Instance = new();

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);

                if (property.PropertyType != typeof(string))
                {
                    if (property.PropertyType.GetInterface("IEnumerable") != null)
                        property.ShouldSerialize =
                            instance => (instance?.GetType().GetProperty(property.PropertyName).GetValue(instance) as IEnumerable<object>)?.Count() > 0;
                }
                return property;
            }
        }

        public static JsonSerializerSettings JsonSettings = new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = ShouldSerializeContractResolver.Instance,
        };
    }
}
