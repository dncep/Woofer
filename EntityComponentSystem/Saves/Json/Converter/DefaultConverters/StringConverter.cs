using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Saves.Json.Objects;

namespace EntityComponentSystem.Saves.Json.Converter.DefaultConverters
{
    public class StringConverter : IJsonConverter
    {
        public Type GetWorkingType() => typeof(string);
        public K FromJson<K>(JsonMaster json, IJsonValue value)
        {
            if (value is JsonString str)
            {
                return (K)Convert.ChangeType(str.Value, typeof(K));
            }
            else return default(K);
        }
        public IJsonValue ToJson(JsonMaster json, object obj) => throw new NotImplementedException();
    }
}
