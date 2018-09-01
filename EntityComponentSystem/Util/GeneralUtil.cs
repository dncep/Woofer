using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Util
{
    public static class GeneralUtil
    {
        public static string EnumerableToString<T>(this IEnumerable<T> arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            bool empty = true;
            foreach (T obj in arr)
            {
                empty = false;
                sb.Append(obj);
                sb.Append(", ");
            }
            if (!empty) sb.Length -= 2;
            sb.Append(']');
            return sb.ToString();
        }

        public static double SubtractAngles(double a, double b)
        {
            double diff = a - b;
            diff += (diff > Math.PI) ? -2 * Math.PI : (diff < -Math.PI) ? 2 * Math.PI : 0;
            return Math.Abs(diff);
        }

        public static double EuclideanMod(double x, double y)
        {
            double r = Math.Abs(x) % Math.Abs(y);
            r *= Math.Sign(x);
            r = (r + Math.Abs(y)) % Math.Abs(y);
            return r;
        }

        public static double FloorMod(double a, double n)
        {
            return (a % n + n) % n;
        }

        public static T RequireAttribute<T>(Type type) where T : Attribute
        {
            object[] attributesFound = type.GetCustomAttributes(typeof(T), false);
            if (attributesFound.Count() != 1)
            {
                throw new AttributeException(
                    $"Required attribute of class '{typeof(T)}' " +
                    ((attributesFound.Count() == 0) ? "not found" : "found more than once") +
                    $" in derived class '{type}'");
            }
            else return attributesFound.First() as T;
        }
    }
    
    [Serializable]
    public sealed class AttributeException : Exception
    {
        public AttributeException() { }
        public AttributeException(string message) : base(message) { }
        public AttributeException(string message, Exception inner) : base(message, inner) { }
    }
}
