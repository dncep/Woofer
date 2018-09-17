using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public class JsonObject : IJsonValue, IEnumerable<KeyValuePair<string, IJsonValue>>
    {
        private readonly Dictionary<string, IJsonValue> Properties = new Dictionary<string, IJsonValue>();

        public void AddProperty(string key, short value) => AddProperty(key, new JsonNumber(value));
        public void AddProperty(string key, int value) => AddProperty(key, new JsonNumber(value));
        public void AddProperty(string key, double value) => AddProperty(key, new JsonNumber(value));
        public void AddProperty(string key, long value) => AddProperty(key, new JsonNumber(value));
        public void AddProperty(string key, string value) => AddProperty(key, new JsonString(value));
        public void AddProperty(string key, bool value) => AddProperty(key, new JsonBoolean(value));
        public void AddProperty(string key, JsonObject value) => AddProperty(key, (object)value);
        public void AddProperty(string key, JsonArray value) => AddProperty(key, (object)value);
        public void AddProperty(string key, object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value is IJsonValue) Properties[key] = (IJsonValue)value;
            else
            {
                switch(value)
                {
                    case byte i: Properties[key] = new JsonNumber(i); break;
                    case short i: Properties[key] = new JsonNumber(i); break;
                    case int i: Properties[key] = new JsonNumber(i); break;
                    case float i: Properties[key] = new JsonNumber(i); break;
                    case double i: Properties[key] = new JsonNumber(i); break;
                    case long i: Properties[key] = new JsonNumber(i); break;
                    case string i: Properties[key] = new JsonString(i); break;
                    case bool i: Properties[key] = new JsonBoolean(i); break;
                    default: Properties[key] = new JsonUnresolvedValue(value); break;
                }
            }
        }

        public T Get<T>(string key) where T : new()
        {
            return Get<T>(null, key);
        }

        public T Get<T>(JsonMaster json, string key)
        {
            if (!Properties.ContainsKey(key)) return default(T);
            IJsonValue property = Properties[key];
            if (property is T) return (T)property;
            else if (json != null) return json.ConvertFromValue<T>(property);
            else return default(T);
        }

        public string[] Resolve(JsonMaster json)
        {
            if (Properties.Count == 0) return new string[] { "{}" };
            List<string> parts = new List<string>();
            parts.Add("{");

            bool commaMissing = false;
            foreach(KeyValuePair<string, IJsonValue> property in Properties)
            {
                if(commaMissing)
                {
                    parts[parts.Count - 1] += ",";
                    commaMissing = false;
                }

                string[] inner = property.Value.Resolve(json);
                if (inner.Length == 0) throw new BadMethodReturnException("Value resolution returned no lines: " + property.Value);
                inner[0] = "\"" + property.Key + "\": " + inner[0];
                foreach(string line in inner)
                {
                    parts.Add(json.Indentation + line);
                }
                commaMissing = true;
            }

            parts.Add("}");

            return parts.ToArray();
        }

        internal static Tuple<IJsonValue, int> ParseObject(string text, int index)
        {
            GeneralUtil.SkipWhitespace(text, ref index);
            if (text[index] != '{') throw new FormatException("Expected object at index " + index);
            index++;
            GeneralUtil.SkipWhitespace(text, ref index);

            if (text[index] == '}') return new Tuple<IJsonValue, int>(new JsonObject(), index + 1);

            JsonObject obj = new JsonObject();

            bool first = true;
            while (first || text[index] == ',')
            {
                if (!first) index++;
                first = false;
                Tuple<string, IJsonValue, int> result = ParseProperty(text, index);
                obj.AddProperty(result.Item1, result.Item2);
                index = result.Item3;
                GeneralUtil.SkipWhitespace(text, ref index);
            }
            if (text[index] != '}') throw new FormatException("Expected end of object");
            index++;

            return new Tuple<IJsonValue, int>(obj, index);
        }

        private static Tuple<string, IJsonValue, int> ParseProperty(string text, int index)
        {
            GeneralUtil.SkipWhitespace(text, ref index);
            Tuple<string, int> keyResult = GeneralUtil.ParseQuotedString(text, index);
            index = keyResult.Item2;
            string key = keyResult.Item1;
            GeneralUtil.SkipWhitespace(text, ref index);

            if (text[index] != ':') throw new FormatException("Expected key/value separator");
            index++;

            Tuple<IJsonValue, int> valueResult = JsonMaster.ParseValue(text, index);

            IJsonValue value = valueResult.Item1;

            return new Tuple<string, IJsonValue, int>(key, value, valueResult.Item2);
        }

        internal static bool IsValidStart(char c)
        {
            return c == '{';
        }

        public override string ToString() => string.Join("\n",Resolve(new JsonMaster()));

        public IEnumerator<KeyValuePair<string, IJsonValue>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, IJsonValue>>)Properties).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<string, IJsonValue>>)Properties).GetEnumerator();
    }
}
