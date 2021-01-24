using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> list)
        {
            return list ?? Enumerable.Empty<T>();
        }
    }
}
