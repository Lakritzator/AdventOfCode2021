using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Support
{
    internal static class StringExtensions
    {
        public static string[] SplitClean(this string value, char separator) => value.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        public static string[] SplitClean(this string value, string separator) => value.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        /// <summary>
        /// Return the intersection of both strings (abc & bcd = bc)
        /// </summary>
        /// <param name="a">string 1</param>
        /// <param name="b">string 2</param>
        /// <returns>string with union</returns>
        public static string Union(this string a, string b) => string.Join("", a.ToCharArray().Intersect(b.ToCharArray()));

        /// <summary>
        /// Return the difference of both strings (abc & bcd = a)
        /// </summary>
        /// <param name="a">string 1</param>
        /// <param name="b">string 2</param>
        /// <returns>string with difference</returns>
        public static string Difference(this string a, string b) => string.Join("", a.ToCharArray().Except(b.ToCharArray()));

        /// <summary>
        /// Remove char(s) from the supplied string (abc & a = abc)
        /// </summary>
        /// <param name="a">string 1</param>
        /// <param name="b">multiple chars</param>
        /// <returns>string with chars removed</returns>
        public static string RemoveChars(this string a, params char[] b) => string.Join("", a.Where(c => !b.Contains(c)));

        /// <summary>
        /// Sort the supplied string by characters
        /// </summary>
        /// <param name="a">string to sort</param>
        /// <returns></returns>
        public static string Order(this string a) => string.Join("", a.ToCharArray().OrderBy(c => c));
    }
}
