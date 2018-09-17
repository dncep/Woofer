using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Saves.Json.Converter;
using EntityComponentSystem.Saves.Json.Objects;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Saves.Json
{
    public class JsonMaster
    {
        private readonly Dictionary<Type, IJsonConverter> Converters = new Dictionary<Type, IJsonConverter>();
        private readonly IJsonConverter PersistentObjectConverter = new PersistentObjectConverter();

        public string Indentation { get; internal set; } = "    ";

        public string ToJson(JsonObject root)
        {
            return string.Join("\n", root.Resolve(this));
        }

        public JsonObject FromJson(string text)
        {
            return JsonObject.ParseObject(text, 0).Item1 as JsonObject;
        }

        public IJsonValue ConvertToValue(object value)
        {
            if(value.GetType().GetCustomAttributes(typeof(PersistentObjectAttribute), true).Length > 0)
            {
                return PersistentObjectConverter.ToJson(this, value);
            }
            foreach(KeyValuePair<Type, IJsonConverter> converterPair in Converters)
            {
                if(converterPair.Key.IsAssignableFrom(value.GetType()))
                {
                    IJsonConverter converter = converterPair.Value;
                    IJsonValue converted = converter.ToJson(this, value);
                    if (converted is JsonUnresolvedValue)
                    {
                        throw new InvalidOperationException("Converter for type " + converterPair.Key.FullName + " does not convert to a valid value");
                    }
                    return converted;
                }
            }
            throw new ArgumentException("No converter available for such type: " + value.GetType().FullName);
        }

        public T ConvertFromValue<T>(IJsonValue property)
        {
            if (typeof(T).GetCustomAttributes(typeof(PersistentObjectAttribute), true).Length > 0)
            {
                return PersistentObjectConverter.FromJson<T>(this, property);
            }
            foreach(KeyValuePair<Type, IJsonConverter> converterPair in Converters)
            {
                if (converterPair.Key.IsAssignableFrom(typeof(T)))
                {
                    IJsonConverter converter = converterPair.Value;
                    T converted = converter.FromJson<T>(this, property);
                    if (converted is JsonUnresolvedValue)
                    {
                        throw new InvalidOperationException("Converter for type " + converterPair.Key.FullName + " does not convert to a valid value");
                    }
                    return converted;
                }
            }
            throw new ArgumentException("No converter available for such type: " + typeof(T).FullName);
        }
        public object ConvertFromValue(IJsonValue value, Type expectedType)
        {
            if (expectedType.IsValueType || expectedType == typeof(string) || expectedType.GetConstructor(new Type[0]) != null)
            {
                MethodInfo method = typeof(JsonMaster).GetMethod("ConvertFromValue", new Type[] { typeof(IJsonValue) });
                MethodInfo genericMethod = method.MakeGenericMethod(expectedType);
                return genericMethod.Invoke(this, new object[] { value });
            }
            else throw new Exception("Type '" + expectedType + " doesn't have a public parameterless constructor");
        }

        public void RegisterConverter(IJsonConverter converter) => Converters[converter.GetWorkingType()] = converter;

        internal static Tuple<IJsonValue, int> ParseValue(string text, int index)
        {
            GeneralUtil.SkipWhitespace(text, ref index);
            char c = text[index];
            Tuple<IJsonValue, int> result =
                JsonNumber.IsValidStart(c) ? JsonNumber.ParseNumber(text, index) :
                JsonBoolean.IsValidStart(c) ? JsonBoolean.ParseBoolean(text, index) :
                JsonString.IsValidStart(c) ? JsonString.ParseString(text, index) :
                JsonObject.IsValidStart(c) ? JsonObject.ParseObject(text, index) :
                JsonArray.IsValidStart(c) ? JsonArray.ParseArray(text, index) : null;
            if (result == null) throw new FormatException("Expected value at: " + text.Substring(index, Math.Min(10, text.Length - index)) + "(index  " + index + ")");
            return result;
        }
    }
}
