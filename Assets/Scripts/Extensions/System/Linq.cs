using System;
using System.Collections.Generic;

namespace Extensions.System
{
    public static class Linq
    {
        public static void DoToAll<T>(this IEnumerable<T> thisIEnumerable, Action<T> func)
        {
            foreach (T variable in thisIEnumerable)
            {
                func(variable);
            }
        }

        public static int ToIndex(this int i)
        {
            if (i <= 0)
            {
                return 0;
            }

            return i - 1;
        }
        
        public static int ToCount(this int i)
        {
            if (i <= 0)
            {
                return 1;
            }

            return i + 1;
        }
    }
}