using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public class JsonNumber : IJsonPrimitive
    {
        public readonly double Value;

        public JsonNumber() : this(0) { }

        public JsonNumber(double value) => Value = value;

        public string[] Resolve(JsonMaster json) => new string[] { ((decimal)(Value)).ToString() };

        internal static Tuple<IJsonValue, int> ParseNumber(string text, int index)
        {
            GeneralUtil.SkipWhitespace(text, ref index);
            int digits = 0;
            bool decimalPoint = false;
            for(int i = index; i < text.Length; i++)
            {
                if (i == index && text[i] == '-') digits++;
                else if (char.IsDigit(text[i])) digits++;
                else if (i != index && text[i] == '.' && !decimalPoint)
                {
                    digits++;
                    decimalPoint = true;
                }
                else break;
            }
            if (digits == 0) throw new FormatException("Expected number at index " + index);
            double num;
            if (double.TryParse(text.Substring(index, digits), out num)) return new Tuple<IJsonValue, int>(new JsonNumber(num), index + digits);
            else throw new FormatException("Expected number at index " + index);
        }

        internal static bool IsValidStart(char c)
        {
            return c == '-' || char.IsDigit(c);
        }
    }
}
