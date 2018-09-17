using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves.Json.Objects;

namespace EntityComponentSystem.Saves.Json.Converter.DefaultConverters
{
    public class NumberConverter<T> : IJsonConverter
    {
        public Type GetWorkingType() => typeof(T);
        public K FromJson<K>(JsonMaster json, IJsonValue value) {
            if (value is JsonNumber num)
            {
                return (K)Convert.ChangeType(num.Value, typeof(K));
            }
            else return default(K);
        }
        public IJsonValue ToJson(JsonMaster json, object obj) => throw new NotImplementedException();
    }
}
