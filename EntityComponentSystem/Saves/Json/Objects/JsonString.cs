using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public class JsonString : IJsonPrimitive
    {
        public readonly string Value;

        public JsonString() : this("") { }

        public JsonString(string value) => Value = value;

        public string[] Resolve(JsonMaster json) => new string[] { $"\"{Value}\"" };

        internal static Tuple<IJsonValue, int> ParseString(string text, int index)
        {
            GeneralUtil.SkipWhitespace(text, ref index);

            Tuple<string, int> result = GeneralUtil.ParseQuotedString(text, index);
            return new Tuple<IJsonValue, int>(new JsonString(result.Item1), result.Item2);
        }

        internal static bool IsValidStart(char c)
        {
            return c == '"' || c == '\'';
        }
    }
}
