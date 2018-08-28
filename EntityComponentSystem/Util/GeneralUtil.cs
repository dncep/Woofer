using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Util
{
    public sealed class GeneralUtil
    {
        public static string EnumerableToString<T>(IEnumerable<T> arr)
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
    }
}
