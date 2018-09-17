using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public class JsonArray : IJsonValue, IEnumerable<IJsonValue>
    {
        private readonly List<IJsonValue> Content = new List<IJsonValue>();

        public IJsonValue this[int index] => Content[index];

        public bool Inline { get; set; } = false;
        public int Length => Content.Count;

        public void Add(short value) => Add(new JsonNumber(value));
        public void Add(int value) => Add(new JsonNumber(value));
        public void Add(double value) => Add(new JsonNumber(value));
        public void Add(long value) => Add(new JsonNumber(value));
        public void Add(string value) => Add(new JsonString(value));
        public void Add(bool value) => Add(new JsonBoolean(value));
        public void Add(JsonObject value) => Add((object)value);
        public void Add(JsonArray value) => Add((object)value);
        public void Add(object value)
        {
            if (value is IJsonValue) Content.Add((IJsonValue)value);
            else
            {
                switch (value)
                {
                    case byte i: Content.Add(new JsonNumber(i)); break;
                    case short i: Content.Add(new JsonNumber(i)); break;
                    case int i: Content.Add(new JsonNumber(i)); break;
                    case float i: Content.Add(new JsonNumber(i)); break;
                    case double i: Content.Add(new JsonNumber(i)); break;
                    case long i: Content.Add(new JsonNumber(i)); break;
                    case string i: Content.Add(new JsonString(i)); break;
                    case bool i: Content.Add(new JsonBoolean(i)); break;
                    default: Content.Add(new JsonUnresolvedValue(value)); break;
                }
            }
        }

        public string[] Resolve(JsonMaster json)
        {
            if (Content.Count == 0) return new string[] { "[]" };
            List<string> parts = new List<string>();
            parts.Add("[");

            bool commaMissing = false;
            foreach (IJsonValue value in Content)
            {
                if (commaMissing)
                {
                    parts[parts.Count - 1] += ",";
                    commaMissing = false;
                }

                string[] inner = value.Resolve(json);
                if (inner.Length == 0) throw new BadMethodReturnException("Value resolution returned no lines: " + value);
                foreach (string line in inner)
                {
                    parts.Add((Inline ? "" : json.Indentation) + line);
                }
                commaMissing = true;
            }

            parts.Add("]");

            if (Inline) return new string[] { string.Join(" ", parts) };
            return parts.ToArray();
        }

        internal static Tuple<IJsonValue, int> ParseArray(string text, int index)
        {
            GeneralUtil.SkipWhitespace(text, ref index);
            if (text[index] != '[') throw new FormatException("Expected array at index " + index);
            index++;
            GeneralUtil.SkipWhitespace(text, ref index);

            if (text[index] == ']') return new Tuple<IJsonValue, int>(new JsonArray(), index + 1);

            JsonArray arr = new JsonArray();

            bool first = true;
            while(first || text[index] == ',')
            {
                if (!first) index++;
                first = false;
                Tuple<IJsonValue, int> result = JsonMaster.ParseValue(text, index);
                arr.Add(result.Item1);
                index = result.Item2;
                GeneralUtil.SkipWhitespace(text, ref index);
            }
            if (text[index] != ']') throw new FormatException("Expected end of array");
            index++;

            return new Tuple<IJsonValue, int>(arr, index);
        }

        internal static bool IsValidStart(char c)
        {
            return c == '[';
        }

        public IEnumerator<IJsonValue> GetEnumerator() => ((IEnumerable<IJsonValue>)Content).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<IJsonValue>)Content).GetEnumerator();
    }
}
