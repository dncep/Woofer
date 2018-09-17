using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public class TagCustom : ITag
    {
        public readonly object Value;

        public TagCustom(object value) => Value = value;

        public int Resolve(TagMaster json, BinaryWriter writer)
        {
            return json.ConvertToValue(Value).Resolve(json, writer);
        }
    }
}
