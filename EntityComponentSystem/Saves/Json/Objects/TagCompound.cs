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
    public class TagCompound : ITag, IEnumerable<KeyValuePair<string, ITag>>
    {
        internal const byte TypeId = 10;
        private readonly Dictionary<string, ITag> Properties = new Dictionary<string, ITag>();

        public void AddProperty(string key, short value) => AddProperty(key, new TagDouble(value));
        public void AddProperty(string key, int value) => AddProperty(key, new TagDouble(value));
        public void AddProperty(string key, double value) => AddProperty(key, new TagDouble(value));
        public void AddProperty(string key, long value) => AddProperty(key, new TagDouble(value));
        public void AddProperty(string key, string value) => AddProperty(key, new TagString(value));
        public void AddProperty(string key, bool value) => AddProperty(key, new TagBoolean(value));
        public void AddProperty(string key, TagCompound value) => AddProperty(key, (object)value);
        public void AddProperty(string key, TagList value) => AddProperty(key, (object)value);
        public void AddProperty(string key, object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (value is ITag) Properties[key] = (ITag)value;
            else
            {
                switch(value)
                {
                    case bool i: Properties[key] = new TagBoolean(i); break;
                    case byte i: Properties[key] = new TagByte(i); break;
                    case short i: Properties[key] = new TagShort(i); break;
                    case int i: Properties[key] = new TagInt(i); break;
                    case float i: Properties[key] = new TagFloat(i); break;
                    case long i: Properties[key] = new TagLong(i); break;
                    case double i: Properties[key] = new TagDouble(i); break;
                    case string i: Properties[key] = new TagString(i); break;
                    default: Properties[key] = new TagCustom(value); break;
                }
            }
        }

        public T Get<T>(string key) where T : new()
        {
            return Get<T>(null, key);
        }

        public T Get<T>(TagMaster json, string key)
        {
            if (!Properties.ContainsKey(key)) return default(T);
            ITag property = Properties[key];
            if (property is T) return (T)property;
            else if (json != null) return json.ConvertFromValue<T>(property);
            else return default(T);
        }

        public int Resolve(TagMaster json, BinaryWriter writer)
        {
            writer.Write(TypeId);
            int size = 1;
            foreach(KeyValuePair<string, ITag> property in Properties)
            {
                writer.Write(true);
                size++;
                size += TagMaster.WriteString(property.Key, writer);
                size += property.Value.Resolve(json, writer);
            }
            writer.Write(false);
            size++;
            return size;
        }

        internal static ITag Parse(BinaryReader reader)
        {
            TagCompound obj = new TagCompound();
            while(reader.ReadBoolean())
            {
                string key = TagMaster.ReadString(reader);
                ITag value = TagMaster.ParseValue(reader);
                obj.AddProperty(key, value);
            }
            return obj;
        }

        internal static bool IsValidStart(byte id)
        {
            return id == TypeId;
        }

        public IEnumerator<KeyValuePair<string, ITag>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, ITag>>)Properties).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<string, ITag>>)Properties).GetEnumerator();
    }
}
