using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves.Json.Objects;

namespace EntityComponentSystem.Saves.Json.Converter.DefaultConverters
{
    public class ListConverter<T> : IJsonConverter
    {
        public Type GetWorkingType() => typeof(IEnumerable<T>);
        public K FromJson<K>(JsonMaster json, IJsonValue value)
        {
            JsonArray arr = value as JsonArray;
            List<T> list = new List<T>();
            foreach(IJsonValue element in arr)
            {
                list.Add(json.ConvertFromValue<T>(element));
            }
            if (typeof(K).IsArray) return (K)Convert.ChangeType(list.ToArray(), typeof(K));
            else return (K)Convert.ChangeType(list, typeof(K));
        }
        public IJsonValue ToJson(JsonMaster json, object rawList)
        {
            IEnumerable<T> list = (IEnumerable<T>)rawList;
            JsonArray arr = new JsonArray();
            foreach(T t in list)
            {
                arr.Add(t);
            }
            return arr;
        }
    }
}
