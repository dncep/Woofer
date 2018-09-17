using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Saves.Json.Objects;

namespace EntityComponentSystem.Saves.Json.Converter
{
    public interface IJsonConverter
    {
        Type GetWorkingType();
        IJsonValue ToJson(JsonMaster json, object obj);
        T FromJson<T>(JsonMaster json, IJsonValue value);
    }
}
