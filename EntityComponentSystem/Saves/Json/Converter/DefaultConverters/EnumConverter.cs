using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves.Json.Objects;

namespace EntityComponentSystem.Saves.Json.Converter.DefaultConverters
{
    public class EnumConverter<T> : IJsonConverter
    {
        public Type GetWorkingType() => typeof(T);
        public K FromJson<K>(JsonMaster json, IJsonValue value)
        {
            JsonString str = value as JsonString;
            foreach(T o in typeof(T).GetEnumValues())
            {
                if (typeof(T).GetEnumName(o) == str.Value) return (K)Convert.ChangeType(o, typeof(K));
            }
            return default(K);
        }
        public IJsonValue ToJson(JsonMaster json, object obj)
        {
            return new JsonString(typeof(T).GetEnumName(obj));
        }
    }
}
