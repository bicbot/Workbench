using System;

namespace SchoolsApp.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            return source?.IndexOf(toCheck, comparison) !=-1;
        }
    }
}
