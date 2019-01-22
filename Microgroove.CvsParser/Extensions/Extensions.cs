using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.CvsParser.Extensions
{
    public static class Extensions
    {
        public static bool In(this char ch, params char[] list)
        {
            return list.Any(c => c == ch);
        }

        public static string TrimFirstLast(this string value, char ch)
        {
            return value.TrimStart(ch).TrimEnd(ch);
        }

        public static string TrimQuotes(this string value)
        {
            return value.TrimFirstLast('"');
        }

        public static DateTime ParseDateTime(this string dateString, string format)
        {
            return DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        }

        public static long ParseLong(this string longString)
        {
            return long.Parse(longString);
        }

        public static DateTime ParseDateTime(this string dateString)
        {
            return dateString.ParseDateTime("MM/dd/yyyy");
        }
    }
}
