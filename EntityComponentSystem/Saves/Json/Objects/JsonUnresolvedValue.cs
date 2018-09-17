using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public class JsonUnresolvedValue : IJsonValue
    {
        public readonly object Value;

        public JsonUnresolvedValue(object value) => Value = value;

        public string[] Resolve(JsonMaster json)
        {
            return json.ConvertToValue(Value).Resolve(json);
        }
    }
}
