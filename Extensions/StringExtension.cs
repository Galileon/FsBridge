using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FsConnect
{
    public static class StringExtension
    {
     
        public static string FirstLine(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            var newLinePos = str.IndexOf(Environment.NewLine, StringComparison.CurrentCulture);
            return newLinePos > 0 ? str.Substring(0, newLinePos) : str;
        }

        /// <summary>
        /// Take a string value from comma separater freeswitch parameters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetFreeswitchCommandParamsValue(this string input, string paramName)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var indexOfParameter = input.IndexOf(paramName);
            if (indexOfParameter < 0) return string.Empty;
            var indexOfComma = input.IndexOf(',', indexOfParameter + paramName.Length);
            if (indexOfParameter < 0) return string.Empty;

            var ret = input.Substring(indexOfParameter + paramName.Length + 1, indexOfComma - indexOfParameter - 1 - paramName.Length);
            return ret;
        }

    }
}
