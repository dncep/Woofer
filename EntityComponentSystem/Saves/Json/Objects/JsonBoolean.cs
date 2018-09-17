using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public class JsonBoolean : IJsonPrimitive
    {
        public readonly bool Value;

        public JsonBoolean() : this(false) { }

        public JsonBoolean(bool value) => Value = value;

        public string[] Resolve(JsonMaster json) => new string[] { Value.ToString().ToLower() };

        internal static Tuple<IJsonValue, int> ParseBoolean(string text, int index)
        {
            GeneralUtil.SkipWhitespace(text, ref index);
            if (index >= text.Length - "false".Length) throw new FormatException("Expected boolean at index" + index);
            if(text.Substring(index, 4) == "true")
            {
                return new Tuple<IJsonValue, int>(new JsonBoolean(true), index + 4);
            } else if(text.Substring(index, 5) == "false")
            {
                return new Tuple<IJsonValue, int>(new JsonBoolean(false), index + 5);
            }
            throw new FormatException("Expected boolean at index" + index);
        }

        internal static bool IsValidStart(char c)
        {
            return c == 't' || c == 'f';
        }
    }
}
