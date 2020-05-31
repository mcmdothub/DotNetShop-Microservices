using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetFlix.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string str)
            => string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }
}
